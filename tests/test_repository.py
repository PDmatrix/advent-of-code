import pytest

from advent.repository import AdventError, Repository


def test_discovers_root_from_nested_directory(repo):
    nested = repo.root / "solutions" / "2026"
    nested.mkdir(parents=True)
    assert Repository.discover(nested) == repo


def test_ignores_unrelated_pyproject(repo):
    nested = repo.root / "nested"
    nested.mkdir()
    (nested / "pyproject.toml").write_text('[project]\nname = "unrelated"\n')
    assert Repository.discover(nested) == repo


def test_missing_root(tmp_path):
    with pytest.raises(AdventError, match="inside the repository"):
        Repository.discover(tmp_path)


def test_historical_and_configured_day_counts(repo):
    assert repo.day_count(2024) == 25
    assert repo.day_count(2025) == 12
    assert repo.day_count(2026) == 12
    (repo.root / "pyproject.toml").write_text(
        "[tool.advent]\ndefault-days = 15\n[tool.advent.days-per-year]\n2026 = 10\n"
    )
    configured = Repository.discover(repo.root)
    assert configured.day_count(2026) == 10
    assert configured.day_count(2027) == 15
    assert configured.day_count(2024) == 25


@pytest.mark.parametrize("year, day", [(2014, 1), (2026, 0), (2026, 13), (2024, 26)])
def test_rejects_invalid_dates(repo, year, day):
    with pytest.raises(AdventError):
        repo.validate_day(year, day)


@pytest.mark.parametrize(
    "setting",
    [
        "default-days = 0",
        "default-days = true",
        'default-days = "12"',
        "days-per-year = {bad = 12}",
    ],
)
def test_invalid_config(repo, setting):
    (repo.root / "pyproject.toml").write_text(f"[tool.advent]\n{setting}\n")
    with pytest.raises(AdventError, match="Invalid"):
        Repository.discover(repo.root)


def test_shared_paths(repo):
    assert repo.day_dir(2026, 1) == repo.root / "solutions/2026/day01"
    assert repo.input_path(2026, 1) == repo.root / "inputs/2026/day01.txt"


def test_next_day_fills_first_gap_and_stops_at_limit(repo):
    for day in (1, 3):
        repo.day_dir(2026, day).mkdir(parents=True)
        (repo.day_dir(2026, day) / "solution.py").touch()
    assert repo.next_day(2026) == 2
    for day in range(1, 13):
        repo.day_dir(2026, day).mkdir(exist_ok=True)
        (repo.day_dir(2026, day) / "solution.py").touch()
    with pytest.raises(AdventError, match="All 12 days.*have Python solutions"):
        repo.next_day(2026)


def test_next_day_ignores_csharp_and_empty_directories(repo):
    directory = repo.day_dir(2015, 1)
    directory.mkdir(parents=True)
    (directory / "Year2015Day01.cs").touch()
    repo.day_dir(2015, 2).mkdir()
    assert repo.next_day(2015) == 1
    (directory / "solution.py").touch()
    assert repo.next_day(2015) == 2
