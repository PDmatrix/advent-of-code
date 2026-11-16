"""The advent console entry point."""

import argparse
import sys
from pathlib import Path

from advent.repository import AdventError, Repository
from advent.runner import run
from advent.scaffold import prepare


def parser() -> argparse.ArgumentParser:
    result = argparse.ArgumentParser(
        prog="advent", description="Prepare and run Python Advent of Code solutions."
    )
    commands = result.add_subparsers(dest="command", required=True)

    scaffold = commands.add_parser(
        "prepare", help="scaffold a day and download its input"
    )
    scaffold.add_argument("--year", type=int, required=True)
    selection = scaffold.add_mutually_exclusive_group(required=True)
    selection.add_argument("--day", type=int)
    selection.add_argument(
        "--next", action="store_true", help="select the first unscaffolded day"
    )

    execute = commands.add_parser("run", help="run a solution using local input")
    execute.add_argument("--year", type=int, required=True)
    execute.add_argument("--day", type=int, required=True)
    execute.add_argument("--part", type=int, choices=(1, 2), help="run only this part")
    inputs = execute.add_mutually_exclusive_group()
    inputs.add_argument(
        "--sample", action="store_true", help="use the day's sample.txt"
    )
    inputs.add_argument(
        "--input", type=Path, help="use a file relative to the current directory"
    )
    return result


def main(argv: list[str] | None = None) -> int:
    args = parser().parse_args(argv)
    try:
        repo = Repository.discover()
        if args.command == "prepare":
            day = repo.next_day(args.year) if args.next else args.day
            directory = prepare(repo, args.year, day)
            print(
                f"Prepared {args.year} day {day:02}: {directory.relative_to(repo.root)}"
            )
            print(
                f"Downloaded input: {repo.input_path(args.year, day).relative_to(repo.root)}"
            )
            print(
                f"Paste sample input into: {directory.relative_to(repo.root) / 'sample.txt'}"
            )
            return 0

        results = run(
            repo,
            args.year,
            args.day,
            sample=args.sample,
            input_path=args.input,
            part=args.part,
        )
        print(f"{args.year} day {args.day:02}")
        for result in results:
            value = result.error if result.error is not None else result.answer
            print(f"Part {result.part}: {value} ({result.seconds * 1000:.3f} ms)")
        return 1 if any(result.error is not None for result in results) else 0
    except (AdventError, OSError, UnicodeError) as exc:
        print(f"advent: {exc}", file=sys.stderr)
        return 1
