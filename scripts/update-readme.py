#!/usr/bin/env python3
"""Generate README progress and solution links from the solution directories."""

from __future__ import annotations

import argparse
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
README = ROOT / "README.md"
SOLUTIONS = ROOT / "CSharp" / "Solutions"

# Advent of Code had 25 days through 2024 and 12 days in 2025.
# Add an override here only when a future event has a different length.
DAYS_PER_YEAR = {2025: 12}
DEFAULT_DAYS = 25

WRITE_UPS = {
    2015: {
        day: f"https://dmatrix.dev/posts/advent-of-code-year-2015-day-{day}/"
        for day in range(1, 21)
    }
}


def completed_days(year_dir: Path) -> list[int]:
    """Return days containing at least one C# solution file."""
    return sorted(
        int(day_dir.name)
        for day_dir in year_dir.iterdir()
        if day_dir.is_dir()
        and day_dir.name.isdigit()
        and any(day_dir.glob("*.cs"))
    )


def read_progress() -> dict[int, list[int]]:
    progress = {
        int(year_dir.name): completed_days(year_dir)
        for year_dir in SOLUTIONS.iterdir()
        if year_dir.is_dir() and year_dir.name.isdigit()
    }

    for year, days in progress.items():
        total = DAYS_PER_YEAR.get(year, DEFAULT_DAYS)
        invalid = [day for day in days if not 1 <= day <= total]
        if invalid:
            raise ValueError(
                f"{year} has days outside its configured 1-{total} range: {invalid}"
            )

    return progress


def render_badge(solved: int, total: int) -> str:
    return (
        f'  <img alt="Progress: {solved} of {total} days" '
        f'src="https://img.shields.io/badge/days%20solved-'
        f'{solved}%20%2F%20{total}-brightgreen">'
    )


def render_progress(progress: dict[int, list[int]]) -> str:
    solved = sum(len(days) for days in progress.values())
    total = sum(DAYS_PER_YEAR.get(year, DEFAULT_DAYS) for year in progress)
    percentage = round(solved / total * 100)

    lines = [
        f"**{solved} of {total} days solved ({percentage}%)**",
        "",
        "Each block represents one day: `█` solved, `░` not solved. "
        "Select a year to browse its solutions.",
        "",
        "| Year | Progress | Solved |",
        "|:----:|:---------|-------:|",
    ]

    for year in sorted(progress, reverse=True):
        total_days = DAYS_PER_YEAR.get(year, DEFAULT_DAYS)
        completed = set(progress[year])
        bar = "".join("█" if day in completed else "░" for day in range(1, total_days + 1))
        status = " ✅" if len(completed) == total_days else ""
        lines.append(
            f"| [{year}](CSharp/Solutions/{year}) | `{bar}` | "
            f"{len(completed)} / {total_days}{status} |"
        )

    return "\n".join(lines)


def render_solutions(progress: dict[int, list[int]]) -> str:
    sections: list[str] = []

    for year in sorted(progress, reverse=True):
        links = " · ".join(
            f"[{day:02}](CSharp/Solutions/{year}/{day})" for day in progress[year]
        )
        sections.extend([f"### {year}", "", links])

        write_ups = WRITE_UPS.get(year)
        if write_ups:
            links = " · ".join(
                f"[{day:02}]({url})" for day, url in sorted(write_ups.items())
            )
            sections.extend(["", f"**Write-ups:** {links}"])

        sections.append("")

    return "\n".join(sections).rstrip()


def replace_section(document: str, name: str, body: str) -> str:
    start = f"<!-- {name}:start -->"
    end = f"<!-- {name}:end -->"

    if document.count(start) != 1 or document.count(end) != 1:
        raise ValueError(f"README must contain exactly one {start} and {end}")

    before, remainder = document.split(start, 1)
    _, after = remainder.split(end, 1)
    return f"{before}{start}\n{body.rstrip()}\n{end}{after}"


def generate(document: str) -> tuple[str, int, int]:
    progress = read_progress()
    solved = sum(len(days) for days in progress.values())
    total = sum(DAYS_PER_YEAR.get(year, DEFAULT_DAYS) for year in progress)

    document = replace_section(document, "progress-badge", render_badge(solved, total))
    document = replace_section(document, "progress", render_progress(progress))
    document = replace_section(document, "solutions", render_solutions(progress))
    return document, solved, total


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument(
        "--check",
        action="store_true",
        help="fail when README.md is not up to date instead of rewriting it",
    )
    args = parser.parse_args()

    current = README.read_text()
    generated, solved, total = generate(current)

    if generated == current:
        print(f"README.md is up to date ({solved}/{total} days).")
        return 0

    if args.check:
        print("README.md is out of date. Run ./scripts/update-readme.py")
        return 1

    README.write_text(generated)
    print(f"Updated README.md ({solved}/{total} days).")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
