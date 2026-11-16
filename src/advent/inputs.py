"""Download personal inputs without exposing the session cookie."""

import os
from http.client import HTTPException
from urllib.error import HTTPError, URLError
from urllib.request import HTTPRedirectHandler, Request, build_opener

from advent.repository import AdventError


class NoRedirect(HTTPRedirectHandler):
    # Never forward the session cookie to a redirected URL.
    def redirect_request(self, req, fp, code, msg, headers, newurl):
        return None


def download_input(year: int, day: int) -> str:
    session = os.environ.get("AOC_SESSION", "").strip()
    if not session:
        raise AdventError("Set AOC_SESSION to your Advent of Code session cookie.")
    if any(char.isspace() or ord(char) < 32 or ord(char) > 126 for char in session):
        raise AdventError("AOC_SESSION must contain only the session cookie value.")

    request = Request(
        f"https://adventofcode.com/{year}/day/{day}/input",
        headers={
            "Cookie": f"session={session}",
            "User-Agent": "advent-repo/0.1 (personal Advent of Code CLI)",
            "Accept": "text/plain",
        },
    )
    try:
        with build_opener(NoRedirect()).open(request, timeout=30) as response:
            if response.status != 200:
                raise AdventError(f"Input download failed (HTTP {response.status}).")
            if response.headers.get_content_type() != "text/plain":
                raise AdventError("Expected plain-text input; check AOC_SESSION.")
            raw = response.read().decode("utf-8")
    except HTTPError as exc:
        status = exc.code
        exc.close()
        if status in (400, 401, 403):
            message = "Input access denied; check AOC_SESSION and your account."
        elif status == 404:
            message = "Puzzle input is unavailable; check the date, year, and day."
        elif status == 429:
            message = "Advent of Code rate-limited the download; try again later."
        else:
            message = f"Input download failed (HTTP {status}); try again later."
        raise AdventError(message) from None
    except (URLError, OSError, ValueError, HTTPException):
        # Do not include exception details: request data can contain credentials.
        raise AdventError(
            "Could not download input; check your connection and retry."
        ) from None

    if not raw.strip():
        raise AdventError(
            "Downloaded input was empty; existing files were not changed."
        )
    if raw.lstrip().lower().startswith(("<!doctype html", "<html")) or raw.startswith(
        "Puzzle inputs differ by user."
    ):
        raise AdventError("Received an error page instead of input; check AOC_SESSION.")
    return raw
