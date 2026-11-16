"""Repository discovery, event configuration, and conventional paths."""

import tomllib
from dataclasses import dataclass
from pathlib import Path


class AdventError(Exception):
    """An actionable CLI error, safe to display without a traceback."""


@dataclass(frozen=True)
class Repository:
    root: Path
    default_days: int = 12
    # Stored as pairs to keep configuration immutable.
    days_per_year: tuple[tuple[int, int], ...] = ()

    @classmethod
    def discover(cls, start: Path | None = None) -> "Repository":
        start = (start or Path.cwd()).resolve()
        for root in (start, *start.parents):
            config = root / "pyproject.toml"
            if not config.is_file():
                continue
            try:
                document = tomllib.loads(config.read_text(encoding="utf-8"))
            except tomllib.TOMLDecodeError as exc:
                raise AdventError(f"Invalid TOML in {config}.") from exc
            settings = document.get("tool", {}).get("advent")
            if settings is None:
                continue
            try:
                default = settings.get("default-days", 12)
                overrides = tuple(
                    (int(year), days)
                    for year, days in settings.get("days-per-year", {}).items()
                )
                if any(
                    type(days) is not int or not 1 <= days <= 25
                    for days in (default, *(days for _, days in overrides))
                ) or any(year < 2015 for year, _ in overrides):
                    raise ValueError
            except (AttributeError, TypeError, ValueError) as exc:
                raise AdventError(
                    "Invalid [tool.advent] configuration: years must be >= 2015 "
                    "and day counts must be integers from 1 to 25."
                ) from exc
            return cls(root, default, overrides)
        raise AdventError("Run advent inside the repository containing [tool.advent].")

    def day_count(self, year: int) -> int:
        if year < 2015:
            raise AdventError("Year must be 2015 or later.")
        return dict(self.days_per_year).get(
            year, 25 if year <= 2024 else self.default_days
        )

    def validate_day(self, year: int, day: int) -> None:
        count = self.day_count(year)
        if not 1 <= day <= count:
            raise AdventError(f"Day must be between 1 and {count} for {year}.")

    def day_dir(self, year: int, day: int) -> Path:
        return self.root / "solutions" / str(year) / f"day{day:02}"

    def input_path(self, year: int, day: int) -> Path:
        return self.root / "inputs" / str(year) / f"day{day:02}.txt"

    def next_day(self, year: int) -> int:
        for day in range(1, self.day_count(year) + 1):
            if not (self.day_dir(year, day) / "solution.py").is_file():
                return day
        raise AdventError(
            f"All {self.day_count(year)} days of {year} have Python solutions."
        )
