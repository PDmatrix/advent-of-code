import subprocess
import sys
from unittest.mock import Mock

import pytest

from advent.cli import main
from advent.scaffold import prepare


@pytest.fixture
def prepared(repo, monkeypatch):
    monkeypatch.chdir(repo.root)
    monkeypatch.setattr("advent.scaffold.download_input", Mock(return_value="abc\n"))
    return prepare(repo, 2026, 1)


def test_prepare_next_and_repeat(repo, prepared, capsys):
    assert main(["prepare", "--year", "2026", "--next"]) == 0
    assert repo.day_dir(2026, 2).is_dir()
    assert "day 02" in capsys.readouterr().out
    assert main(["prepare", "--year", "2026", "--day", "2"]) == 0
    assert not repo.day_dir(2026, 3).exists()


def test_next_prepares_python_in_existing_csharp_day(repo, monkeypatch, capsys):
    directory = repo.day_dir(2015, 1)
    directory.mkdir(parents=True)
    csharp = directory / "Year2015Day01.cs"
    csharp.write_text("// Keep this solution\n")
    monkeypatch.chdir(repo.root)
    monkeypatch.setattr("advent.scaffold.download_input", Mock(return_value="input\n"))
    assert main(["prepare", "--year", "2015", "--next"]) == 0
    assert csharp.read_text() == "// Keep this solution\n"
    assert (directory / "solution.py").is_file()
    output = capsys.readouterr().out
    assert "solutions/2015/day01" in output
    assert "inputs/2015/day01.txt" in output


def test_run_from_nested_directory_without_session(repo, prepared, monkeypatch, capsys):
    (prepared / "solution.py").write_text(
        "def part1(raw):\n    return 0\n\ndef part2(raw):\n    return 'done'\n"
    )
    monkeypatch.chdir(prepared)
    assert main(["run", "--year", "2026", "--day", "1"]) == 0
    output = capsys.readouterr().out
    assert "Part 1: 0" in output
    assert "Part 2: done" in output
    assert "ms)" in output


def test_custom_input_is_relative_to_cwd(repo, prepared, monkeypatch, capsys):
    (prepared / "solution.py").write_text("def part1(raw):\n    return raw\n")
    (prepared / "custom.txt").write_text("local custom")
    monkeypatch.chdir(prepared)
    assert (
        main(
            [
                "run",
                "--year",
                "2026",
                "--day",
                "1",
                "--part",
                "1",
                "--input",
                "custom.txt",
            ]
        )
        == 0
    )
    assert "local custom" in capsys.readouterr().out


def test_unimplemented_returns_nonzero(prepared, capsys):
    assert main(["run", "--year", "2026", "--day", "1"]) == 1
    assert capsys.readouterr().out.count("not implemented") == 2


def test_empty_sample_returns_actionable_error(prepared, capsys):
    assert main(["run", "--year", "2026", "--day", "1", "--sample"]) == 1
    assert "Input is empty" in capsys.readouterr().err


def test_missing_session_is_actionable(repo, monkeypatch, capsys):
    monkeypatch.chdir(repo.root)
    assert main(["prepare", "--year", "2026", "--day", "1"]) == 1
    assert "AOC_SESSION" in capsys.readouterr().err
    assert not repo.day_dir(2026, 1).exists()


@pytest.mark.parametrize(
    "arguments",
    [
        [],
        ["prepare", "--year", "2026"],
        ["prepare", "--year", "2026", "--day", "1", "--next"],
        ["run", "--year", "2026", "--day", "1", "--sample", "--input", "x"],
        ["run", "--year", "2026", "--day", "1", "--part", "3"],
    ],
)
def test_invalid_arguments(arguments):
    with pytest.raises(SystemExit) as error:
        main(arguments)
    assert error.value.code == 2


def test_module_entrypoint_help_works_outside_repo(tmp_path):
    result = subprocess.run(
        [sys.executable, "-m", "advent", "--help"],
        cwd=tmp_path,
        capture_output=True,
        text=True,
        check=False,
    )
    assert result.returncode == 0
    assert "prepare" in result.stdout and "run" in result.stdout
