import pytest

from advent.repository import Repository


@pytest.fixture(autouse=True)
def no_session(monkeypatch):
    monkeypatch.delenv("AOC_SESSION", raising=False)


@pytest.fixture
def repo(tmp_path):
    (tmp_path / "pyproject.toml").write_text("[tool.advent]\ndefault-days = 12\n")
    return Repository(tmp_path)
