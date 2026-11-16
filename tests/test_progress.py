from importlib.util import module_from_spec, spec_from_file_location
from pathlib import Path

import pytest


@pytest.fixture
def progress(tmp_path, monkeypatch):
    script = Path(__file__).resolve().parents[1] / "scripts" / "update-readme.py"
    spec = spec_from_file_location("update_readme", script)
    module = module_from_spec(spec)
    spec.loader.exec_module(module)
    monkeypatch.setattr(module, "SOLUTIONS", tmp_path / "solutions")
    module.SOLUTIONS.mkdir()
    return module


def add_solution(root, year, day, filename):
    directory = root / str(year) / f"day{day:02}"
    directory.mkdir(parents=True, exist_ok=True)
    (directory / filename).touch()
    return directory


def test_progress_counts_csharp_once_per_day(progress):
    add_solution(progress.SOLUTIONS, 2015, 1, "Year2015Day01.cs")
    add_solution(progress.SOLUTIONS, 2015, 1, "Helper.cs")
    add_solution(progress.SOLUTIONS, 2015, 1, "solution.py")
    add_solution(progress.SOLUTIONS, 2015, 10, "Year2015Day10.cs")
    add_solution(progress.SOLUTIONS, 2015, 2, "Year2015Day02.cs")
    assert progress.read_progress() == {2015: [1, 2, 10]}


def test_python_scaffolds_do_not_change_progress(progress):
    add_solution(progress.SOLUTIONS, 2025, 1, "Year2025Day01.cs")
    add_solution(progress.SOLUTIONS, 2025, 2, "solution.py")
    add_solution(progress.SOLUTIONS, 2026, 1, "solution.py")
    assert progress.read_progress() == {2025: [1]}
    assert "1 of 12 days" in progress.render_progress(progress.read_progress())


def test_progress_uses_shared_layout_links(progress):
    directory = add_solution(progress.SOLUTIONS, 2015, 1, "Year2015Day01.cs")
    data = progress.read_progress()
    assert "[2015](solutions/2015)" in progress.render_progress(data)
    assert "[01](solutions/2015/day01)" in progress.render_solutions(data)
    assert directory.is_dir()


def test_progress_rejects_out_of_range_days(progress):
    add_solution(progress.SOLUTIONS, 2025, 13, "Year2025Day13.cs")
    with pytest.raises(ValueError, match="outside its configured"):
        progress.read_progress()


def test_progress_ignores_non_day_directories(progress):
    year = progress.SOLUTIONS / "2015"
    for name in ("notes", "__pycache__", "1", "day1"):
        directory = year / name
        directory.mkdir(parents=True)
        (directory / "Helper.cs").touch()
    assert progress.read_progress() == {}
    assert "0 of 0 days solved (0.0%)" in progress.render_progress({})
