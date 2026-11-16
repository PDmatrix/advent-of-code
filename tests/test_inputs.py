from email.message import Message
from http.client import IncompleteRead
from io import BytesIO
from unittest.mock import Mock
from urllib.error import HTTPError, URLError

import pytest

from advent.inputs import NoRedirect, download_input
from advent.repository import AdventError


@pytest.fixture
def opener(monkeypatch):
    monkeypatch.setenv("AOC_SESSION", "private-test-token")
    opener = Mock()
    monkeypatch.setattr("advent.inputs.build_opener", lambda *args: opener)
    return opener


def response(body=b"  abc\n\n", content_type="text/plain", status=200):
    result = BytesIO(body)
    result.status = status
    result.headers = Message()
    result.headers["Content-Type"] = content_type
    return result


def test_requires_session_before_network(monkeypatch):
    network = Mock()
    monkeypatch.setattr("advent.inputs.build_opener", network)
    with pytest.raises(AdventError, match="Set AOC_SESSION"):
        download_input(2026, 1)
    network.assert_not_called()


def test_download_keeps_whitespace_and_authenticates(opener):
    opener.open.return_value = response()
    assert download_input(2026, 1) == "  abc\n\n"
    request = opener.open.call_args.args[0]
    assert request.full_url == "https://adventofcode.com/2026/day/1/input"
    assert request.get_header("Cookie") == "session=private-test-token"
    assert request.get_header("User-agent")
    assert opener.open.call_args.kwargs["timeout"] == 30


@pytest.mark.parametrize(
    "status, message",
    [
        (400, "access denied"),
        (401, "access denied"),
        (403, "access denied"),
        (404, "unavailable"),
        (429, "rate-limited"),
        (500, "HTTP 500"),
        (302, "HTTP 302"),
    ],
)
def test_http_errors_do_not_leak_credentials(opener, status, message):
    opener.open.side_effect = HTTPError(
        "https://adventofcode.com", status, "private-test-token", {}, None
    )
    with pytest.raises(AdventError, match=message) as error:
        download_input(2026, 1)
    assert "private-test-token" not in str(error.value)


@pytest.mark.parametrize(
    "failure",
    [
        URLError("private-test-token"),
        TimeoutError("private-test-token"),
        IncompleteRead(b"partial"),
    ],
)
def test_network_errors_are_safe(opener, failure):
    opener.open.side_effect = failure
    with pytest.raises(AdventError, match="connection") as error:
        download_input(2026, 1)
    assert "private-test-token" not in str(error.value)


@pytest.mark.parametrize(
    "body, content_type",
    [
        (b"", "text/plain"),
        (b" \n", "text/plain"),
        (b"<html>login", "text/html"),
        (b"<!DOCTYPE html>login", "text/plain"),
        (b"Puzzle inputs differ by user. Please log in", "text/plain"),
        (b"\xff", "text/plain"),
    ],
)
def test_rejects_invalid_responses(opener, body, content_type):
    opener.open.return_value = response(body, content_type)
    with pytest.raises(AdventError):
        download_input(2026, 1)


def test_rejects_header_injection(opener, monkeypatch):
    monkeypatch.setenv("AOC_SESSION", "cookie\r\nInjected: value")
    with pytest.raises(AdventError, match="cookie value"):
        download_input(2026, 1)
    opener.open.assert_not_called()


def test_redirects_are_disabled():
    assert (
        NoRedirect().redirect_request(None, None, 302, "", {}, "https://elsewhere.test")
        is None
    )
