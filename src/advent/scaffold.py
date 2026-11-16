"""Create missing day files and refresh personal input safely."""

import tempfile
from importlib.resources import files
from pathlib import Path

from advent.inputs import download_input
from advent.repository import Repository


def _create_once(path: Path, content: str) -> None:
    try:
        with path.open("x", encoding="utf-8", newline="") as handle:
            handle.write(content)
    except FileExistsError:
        pass


def _replace_input(path: Path, raw: str) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    temporary = None
    try:
        with tempfile.NamedTemporaryFile(
            mode="w", encoding="utf-8", newline="", dir=path.parent, delete=False
        ) as handle:
            temporary = Path(handle.name)
            handle.write(raw)
        temporary.replace(path)
    finally:
        if temporary is not None:
            temporary.unlink(missing_ok=True)


def prepare(repo: Repository, year: int, day: int) -> Path:
    repo.validate_day(year, day)
    # Fail before creating any files if credentials, network, or release checks fail.
    raw = download_input(year, day)
    directory = repo.day_dir(year, day)
    templates = files("advent").joinpath("templates")
    contents = {
        "solution.py": templates.joinpath("solution.py.txt").read_text(
            encoding="utf-8"
        ),
        "test_solution.py": templates.joinpath("test_solution.py.txt").read_text(
            encoding="utf-8"
        ),
        "sample.txt": "",
        "README.md": (
            f"# {year} — Day {day}\n\n"
            f"[Puzzle](https://adventofcode.com/{year}/day/{day})\n\n"
            "Paste the example input into `sample.txt`. Set the expected answers in\n"
            "`test_solution.py` to enable the sample tests. Inputs stay local.\n\n"
            "```bash\n"
            f"uv run advent run --year {year} --day {day} --sample\n"
            f"uv run advent run --year {year} --day {day}\n"
            f"uv run pytest Python/solutions/y{year}/day{day:02}\n"
            "```\n"
        ),
    }
    directory.mkdir(parents=True, exist_ok=True)
    for name, content in contents.items():
        _create_once(directory / name, content)
    _replace_input(repo.input_path(year, day), raw)
    return directory
