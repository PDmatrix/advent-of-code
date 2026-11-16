"""Load and execute standalone solution modules without network access."""

import sys
from dataclasses import dataclass
from hashlib import sha256
from importlib.util import module_from_spec, spec_from_file_location
from pathlib import Path
from time import perf_counter
from types import ModuleType

from advent.repository import AdventError, Repository


@dataclass(frozen=True)
class PartResult:
    part: int
    answer: int | str | None
    seconds: float
    error: str | None = None


def load_solution(path: Path) -> ModuleType:
    """Load a solution by path; also usable from generated sample tests."""
    path = path.resolve()
    if not path.is_file():
        raise AdventError(f"Solution not found: {path}. Run advent prepare first.")
    name = "_advent_solution_" + sha256(str(path).encode()).hexdigest()
    spec = spec_from_file_location(name, path)
    if spec is None or spec.loader is None:
        raise AdventError(f"Cannot load solution: {path}.")
    module = module_from_spec(spec)
    # Registration supports constructs such as dataclasses inside solutions.
    sys.modules[name] = module
    try:
        spec.loader.exec_module(module)
    except Exception as exc:
        sys.modules.pop(name, None)
        raise AdventError(
            f"Could not load {path}: {type(exc).__name__}: {exc}"
        ) from exc
    return module


def run(
    repo: Repository,
    year: int,
    day: int,
    *,
    sample: bool = False,
    input_path: Path | None = None,
    part: int | None = None,
) -> list[PartResult]:
    repo.validate_day(year, day)
    if sample and input_path is not None:
        raise AdventError("Choose either sample input or a custom input file.")
    if part not in (None, 1, 2):
        raise AdventError("Part must be 1 or 2.")
    directory = repo.day_dir(year, day)
    path = (
        input_path
        if input_path is not None
        else (directory / "sample.txt" if sample else repo.input_path(year, day))
    )
    if not path.is_file():
        raise AdventError(f"Input not found: {path}.")
    with path.open(encoding="utf-8", newline="") as handle:
        raw = handle.read()
    if not raw.strip():
        raise AdventError(f"Input is empty: {path}. Populate it before running.")
    module = load_solution(directory / "solution.py")
    results = []
    for number in (part,) if part is not None else (1, 2):
        function = getattr(module, f"part{number}", None)
        start = perf_counter()
        answer = None
        error = None
        try:
            if not callable(function):
                raise TypeError(f"solution.py must define part{number}(raw)")
            answer = function(raw)
            if type(answer) not in (int, str):
                raise TypeError("a part must return int or str")
        except NotImplementedError:
            error = "not implemented"
        except Exception as exc:  # noqa: BLE001 — isolate failures between puzzle parts
            error = f"failed: {type(exc).__name__}: {exc}"
        results.append(PartResult(number, answer, perf_counter() - start, error))
    return results
