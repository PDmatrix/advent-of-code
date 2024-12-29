using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._22;

[UsedImplicitly]
public partial class Year2022Day22 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (grid, moves) = ParseInput(input);
		var direction = new Point(1, 0);
		var newDirection = direction;
		var mStr = "";
		var mCount = 0;
		var minY = grid.Min(x => x.Key.Y);
		var current = new Point(grid.Where(x => x.Key.Y == minY).Min(x => x.Key.X), minY);

		for (var i = 0; i < moves.Length; i++)
		{
			var move = moves[i];
			mCount = 0;

			if (int.TryParse(move.ToString(), out _))
			{
				mStr += move;
			}
			else
			{
				newDirection = Rotate(direction, move.ToString());
				mCount = int.Parse(mStr);
				mStr = "";
			}

			if (i == moves.Length - 1)
				mCount = int.Parse(mStr);

			for (var j = 0; j < mCount; j++)
			{
				//ShowScreen(grid, current);
				var (possible, newCurrent) = Possible(grid, current, direction);
				if (possible)
					current = newCurrent;
				else
					break;
			}

			direction = newDirection;
		}

		return 1000 * (current.Y + 1) + 4 * (current.X + 1) + DirectionToNum(direction);
	}

	public object Part2(IEnumerable<string> input)
	{
		// cudos to u/Perska_
		// https://github.com/Perska/AoC2022/blob/master/AoC2022/Days/Day22.cs
		return Solve(
			input,
			"""
			A -> B0 C0 D2 F1
			B -> E2 C1 A0 F0
			C -> B3 E0 D3 A0
			D -> E0 F0 A2 C1
			E -> B2 F1 D0 C0
			F -> E3 B0 A3 D0
			""");
	}

	private static int DirectionToNum(Point direction)
	{
		return direction switch
		{
			{ X: 1, Y: 0 } => 0,
			{ X: 0, Y: 1 } => 1,
			{ X: -1, Y: 0 } => 2,
			{ X: 0, Y: -1 } => 3,
			_ => throw new Exception("Invalid direction")
		};
	}

	private static Point Rotate(Point current, string direction)
	{
		if (direction == "R")
		{
			return current switch
			{
				{ X: 0, Y: 1 } => new Point(-1, 0),
				{ X: 1, Y: 0 } => new Point(0, 1),
				{ X: 0, Y: -1 } => new Point(1, 0),
				{ X: -1, Y: 0 } => new Point(0, -1),
				_ => throw new Exception("Invalid direction")
			};
		}

		return current switch
		{
			{ X: 0, Y: 1 } => new Point(1, 0),
			{ X: 1, Y: 0 } => new Point(0, -1),
			{ X: 0, Y: -1 } => new Point(-1, 0),
			{ X: -1, Y: 0 } => new Point(0, 1),
			_ => throw new Exception("Invalid direction")
		};
	}

	private static (bool, Point) Possible(Dictionary<Point, string> grid, Point current, Point direction)
	{
		if (grid.TryGetValue(new Point(current.X + direction.X, current.Y + direction.Y), out var c) && c == ".")
			return (true, new Point(current.X + direction.X, current.Y + direction.Y));

		if (c == "#")
			return (false, new Point());

		if (direction is { X: 0, Y: 1 })
		{
			var minY = grid.Where(x => x.Key.X == current.X).Min(x => x.Key.Y);
			if (grid.TryGetValue(current with { Y = minY }, out var p) && p == ".")
				return (true, current with { Y = minY });
		}

		if (direction is { X: 0, Y: -1 })
		{
			var maxY = grid.Where(x => x.Key.X == current.X).Max(x => x.Key.Y);
			if (grid.TryGetValue(current with { Y = maxY }, out var p) && p == ".")
				return (true, current with { Y = maxY });
		}

		if (direction is { X: 1, Y: 0 })
		{
			var minX = grid.Where(x => x.Key.Y == current.Y).Min(x => x.Key.X);
			if (grid.TryGetValue(current with { X = minX }, out var p) && p == ".")
				return (true, current with { X = minX });
		}

		if (direction is { X: -1, Y: 0 })
		{
			var maxX = grid.Where(x => x.Key.Y == current.Y).Max(x => x.Key.X);
			if (grid.TryGetValue(current with { X = maxX }, out var p) && p == ".")
				return (true, current with { X = maxX });
		}

		return (false, new Point());
	}

	private static (Dictionary<Point, string>, string) ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, string>();
		var y = 0;
		var parseMoves = false;
		var moves = "";
		foreach (var line in input)
		{
			if (string.IsNullOrEmpty(line))
			{
				parseMoves = true;
				continue;
			}

			if (parseMoves)
			{
				moves = line;
				break;
			}

			var x = 0;
			foreach (var c in line)
			{
				if (c != ' ')
					grid.Add(new Point(x, y), c.ToString());
				x++;
			}

			y++;
		}

		return (grid, moves);
	}

	const int blockSize = 50;
	const int right = 0;
	const int down = 1;
	const int left = 2;
	const int up = 3;

	int Solve(IEnumerable<string> input, string topology)
	{
		var (map, cmds) = Parse(input);
		var state = new State("A", new Coord(0, 0), right);

		foreach (var cmd in cmds)
		{
			switch (cmd)
			{
				case Left:
					state = state with { dir = (state.dir + 3) % 4 };
					break;
				case Right:
					state = state with { dir = (state.dir + 1) % 4 };
					break;
				case Forward(var n):
					for (var i = 0; i < n; i++)
					{
						var stateNext = Step(topology, state);
						var global = ToGlobal(stateNext);
						if (map[global.irow][global.icol] == '.')
						{
							state = stateNext;
						}
						else
						{
							break;
						}
					}

					break;
			}
		}

		return 1000 * (ToGlobal(state).irow + 1) +
		       4 * (ToGlobal(state).icol + 1) +
		       state.dir;
	}

	Coord ToGlobal(State state) =>
		state.block switch
		{
			"A" => state.coord + new Coord(0, blockSize),
			"B" => state.coord + new Coord(0, 2 * blockSize),
			"C" => state.coord + new Coord(blockSize, blockSize),
			"D" => state.coord + new Coord(2 * blockSize, 0),
			"E" => state.coord + new Coord(2 * blockSize, blockSize),
			"F" => state.coord + new Coord(3 * blockSize, 0),
			_ => throw new Exception()
		};

	State Step(string topology, State state)
	{

		bool wrapsAround(Coord coord) =>
			coord.icol < 0 || coord.icol >= blockSize ||
			coord.irow < 0 || coord.irow >= blockSize;

		var (srcBlock, coord, dir) = state;
		var dstBlock = srcBlock;

		// take one step, if there is no wrap around we are all right
		coord = dir switch
		{
			left => coord with { icol = coord.icol - 1 },
			down => coord with { irow = coord.irow + 1 },
			right => coord with { icol = coord.icol + 1 },
			up => coord with { irow = coord.irow - 1 },
			_ => throw new Exception()
		};

		if (wrapsAround(coord))
		{
			// check the topology, select the dstBlock and rotate coord and dir 
			// as much as needed this is easier to follow through an example
			// if srcBlock: "C", dir: 2

			var line = topology.Split('\n').Single(x => x.StartsWith(srcBlock));
			// line: C -> B3 E0 D3 A0

			var mapping = line.Split(" -> ")[1].Split(" ");
			// mapping: B3 E0 D3 A0

			var neighbour = mapping[dir];
			// neighbour: D3

			dstBlock = neighbour.Substring(0, 1);
			// dstBlock: D

			var rotate = int.Parse(neighbour.Substring(1));
			// rotate: 3

			// go back to the 0..49 range first, then rotate as much as needed
			coord = coord with
			{
				irow = (coord.irow + blockSize) % blockSize,
				icol = (coord.icol + blockSize) % blockSize,
			};

			for (var i = 0; i < rotate; i++)
			{
				coord = coord with
				{
					irow = coord.icol,
					icol = blockSize - coord.irow - 1
				};
				dir = (dir + 1) % 4;
			}
		}

		return new State(dstBlock, coord, dir);
	}

	(string[] map, Cmd[] path) Parse(IEnumerable<string> input)
	{
		var map = input.TakeWhile(x => !string.IsNullOrEmpty(x)).ToArray();

		var commands = Regex
			.Matches(input.Last(), @"(\d+)|L|R")
			.Select<Match, Cmd>(m =>
				m.Value switch
				{
					"L" => new Left(),
					"R" => new Right(),
					string n => new Forward(int.Parse(n)),
				})
			.ToArray();

		return (map, commands);
	}

	record State(string block, Coord coord, int dir);

	record Coord(int irow, int icol)
	{
		public static Coord operator +(Coord a, Coord b) =>
			new Coord(a.irow + b.irow, a.icol + b.icol);

		public static Coord operator -(Coord a, Coord b) =>
			new Coord(a.irow - b.irow, a.icol - b.icol);
	}

	interface Cmd
	{
	}

	record Forward(int n) : Cmd;

	record Right() : Cmd;

	record Left() : Cmd;
}