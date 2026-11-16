import pytest

from advent.repository import AdventError
from advent.runner import load_solution, run


@pytest.fixture
def solution(repo):
    directory = repo.day_dir(2026, 1)
    directory.mkdir(parents=True)
    path = directory / "solution.py"
    path.write_text(
        "def part1(raw):\n    return raw\n\ndef part2(raw):\n    return len(raw)\n"
    )
    personal = repo.input_path(2026, 1)
    personal.parent.mkdir(parents=True)
    personal.write_bytes(b"  personal\r\n\r\n")
    (directory / "sample.txt").write_text("sample\n")
    return path


def test_personal_input_preserves_exact_whitespace(repo, solution):
    results = run(repo, 2026, 1)
    assert [result.answer for result in results] == ["  personal\r\n\r\n", 14]
    assert all(result.error is None and result.seconds >= 0 for result in results)


def test_sample_selection(repo, solution):
    assert run(repo, 2026, 1, sample=True)[0].answer == "sample\n"


def test_custom_input_selection(repo, solution, tmp_path):
    custom = tmp_path / "custom.txt"
    custom.write_text("custom\n")
    assert run(repo, 2026, 1, input_path=custom)[0].answer == "custom\n"


def test_only_requested_part_executes(repo, solution):
    solution.write_text(
        'def part1(raw):\n    raise RuntimeError("must not run")\n\ndef part2(raw):\n    return 42\n'
    )
    results = run(repo, 2026, 1, part=2)
    assert len(results) == 1
    assert results[0].part == 2
    assert results[0].answer == 42


def test_unimplemented_part_does_not_prevent_other_part(repo, solution):
    solution.write_text(
        "def part1(raw):\n    raise NotImplementedError\n\ndef part2(raw):\n    return 0\n"
    )
    results = run(repo, 2026, 1)
    assert results[0].error == "not implemented"
    assert results[1].answer == 0


@pytest.mark.parametrize(
    "body, message",
    [
        ('raise ValueError("oops")', "failed: ValueError: oops"),
        ("return None", "must return int or str"),
        ("return True", "must return int or str"),
    ],
)
def test_failed_parts_are_distinct_from_unimplemented(repo, solution, body, message):
    solution.write_text(f"def part1(raw):\n    {body}\n")
    assert message in run(repo, 2026, 1, part=1)[0].error


def test_missing_function(repo, solution):
    solution.write_text("# no functions\n")
    assert "must define part1(raw)" in run(repo, 2026, 1, part=1)[0].error


def test_missing_solution(repo, solution):
    solution.unlink()
    with pytest.raises(AdventError, match="Solution not found"):
        run(repo, 2026, 1)


def test_missing_input(repo, solution):
    repo.input_path(2026, 1).unlink()
    with pytest.raises(AdventError, match="Input not found"):
        run(repo, 2026, 1)


def test_empty_sample(repo, solution):
    solution.with_name("sample.txt").write_text("")
    with pytest.raises(AdventError, match="Input is empty"):
        run(repo, 2026, 1, sample=True)


def test_module_load_failure(repo, solution):
    solution.write_text("def broken(\n")
    with pytest.raises(AdventError, match="SyntaxError"):
        run(repo, 2026, 1)


def test_dataclass_support(solution):
    solution.write_text(
        "from dataclasses import dataclass\n\n@dataclass\nclass Point:\n    x: int\n"
    )
    module = load_solution(solution)
    assert module.Point(3).x == 3
