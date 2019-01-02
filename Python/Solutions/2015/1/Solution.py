import os

YEAR = 2015
DAY = 1


def part1(inp):
    floor = 0
    for direction in inp:
        floor = floor + (1 if direction == '(' else -1)
    return floor


def part2(inp):
    floor = 0
    for idx, direction in enumerate(inp):
        floor = floor + (1 if direction == '(' else -1)
        if floor == -1:
            return idx + 1
    raise Exception("Solution not found")


def parse():
    path = os.path.relpath(r'../../../', os.path.dirname(__file__))
    with open(f'{path}/Input/{YEAR}/Day{DAY:02}.in') as f:
        return f.read()


def solve():
    inp = parse()
    print(f"First Part: {part1(inp)}")
    print(f"Second Part: {part2(inp)}")


if __name__ == '__main__':
    solve()
