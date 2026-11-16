import subprocess
import sys
from pathlib import Path
from unittest.mock import Mock

import pytest

from advent.repository import AdventError
from advent.scaffold import prepare


@pytest.fixture
def download(monkeypatch):
    download = Mock(return_value="  personal input\n\n")
    monkeypatch.setattr("advent.scaffold.download_input", download)
    return download


def test_prepare_creates_files(repo, download):
    directory = prepare(repo, 2026, 1)
    download.assert_called_once_with(2026, 1)
    assert {path.name for path in directory.iterdir()} == {
        "solution.py",
        "test_solution.py",
        "sample.txt",
        "README.md",
    }
    assert (directory / "sample.txt").read_text() == ""
    assert "raise NotImplementedError" in (directory / "solution.py").read_text()
    assert (
        "https://adventofcode.com/2026/day/1" in (directory / "README.md").read_text()
    )
    assert repo.input_path(2026, 1).read_text() == "  personal input\n\n"


def test_repeat_preserves_all_day_files_and_refreshes_input(repo, download):
    directory = prepare(repo, 2026, 1)
    for path in directory.iterdir():
        path.write_text(f"user content: {path.name}")
    download.return_value = "new input\n"
    prepare(repo, 2026, 1)
    for path in directory.iterdir():
        assert path.read_text() == f"user content: {path.name}"
    assert repo.input_path(2026, 1).read_text() == "new input\n"
    assert download.call_count == 2


def test_repeat_restores_only_missing_scaffold_files(repo, download):
    directory = prepare(repo, 2026, 1)
    (directory / "sample.txt").unlink()
    (directory / "solution.py").write_text("my solution")
    prepare(repo, 2026, 1)
    assert (directory / "sample.txt").read_text() == ""
    assert (directory / "solution.py").read_text() == "my solution"


def test_download_failure_creates_nothing(repo, download):
    download.side_effect = AdventError("unavailable")
    with pytest.raises(AdventError, match="unavailable"):
        prepare(repo, 2026, 1)
    assert not (repo.root / "Python").exists()


def test_failed_refresh_preserves_files(repo, download):
    prepare(repo, 2026, 1)
    before = {
        path: path.read_bytes() for path in repo.root.rglob("*") if path.is_file()
    }
    download.side_effect = AdventError("unavailable")
    with pytest.raises(AdventError):
        prepare(repo, 2026, 1)
    assert before == {
        path: path.read_bytes() for path in repo.root.rglob("*") if path.is_file()
    }


def test_atomic_input_replace_failure_preserves_previous_input(
    repo, download, monkeypatch
):
    prepare(repo, 2026, 1)
    download.return_value = "replacement"

    def fail_replace(self, target):
        raise OSError("disk error")

    monkeypatch.setattr(Path, "replace", fail_replace)
    with pytest.raises(OSError, match="disk error"):
        prepare(repo, 2026, 1)
    path = repo.input_path(2026, 1)
    assert path.read_text() == "  personal input\n\n"
    assert list(path.parent.iterdir()) == [path]


def test_invalid_day_never_downloads(repo, download):
    with pytest.raises(AdventError):
        prepare(repo, 2026, 13)
    download.assert_not_called()


def test_generated_tests_collect_across_days(repo, download):
    prepare(repo, 2026, 1)
    prepare(repo, 2026, 2)
    result = subprocess.run(
        [
            sys.executable,
            "-m",
            "pytest",
            "--import-mode=importlib",
            "-q",
            str(repo.root / "Python"),
        ],
        cwd=repo.root,
        text=True,
        capture_output=True,
        check=False,
    )
    assert result.returncode == 0, result.stdout + result.stderr
    assert "4 skipped" in result.stdout
