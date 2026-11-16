using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._14;

[UsedImplicitly]
public class Year2023Day14 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		return CountGrid(MoveRocks(ParseInput(input), Direction.North));
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		var iter = 1;
		var hashedStates = new Dictionary<int, int>();
		const int total = 1000000000;
		var length = 0;

		while (iter < total)
		{
			grid = MoveRocks(grid, Direction.North);
			grid = MoveRocks(grid, Direction.West);
			grid = MoveRocks(grid, Direction.South);
			grid = MoveRocks(grid, Direction.East);

			var newHash = GetHash(grid);
			if (!hashedStates.TryGetValue(newHash, out var state))
			{
				hashedStates[newHash] = iter;
				iter++;
				continue;
			}

			length = iter - state;
			break;
		}

		if (length <= 0) 
			return CountGrid(grid);
		
		var remaining = (total - iter) % length;
		for (var i = 0; i < remaining; i++)
		{
			grid = MoveRocks(grid, Direction.North);
			grid = MoveRocks(grid, Direction.West);
			grid = MoveRocks(grid, Direction.South);
			grid = MoveRocks(grid, Direction.East);
		}

		return CountGrid(grid);
	}

	private enum Direction
	{
		North,
		South,
		West,
		East
	}

	private static Dictionary<Point, string> MoveRocks(
		Dictionary<Point, string> grid,
		Direction direction)
	{
		var maxY = grid.Keys.Max(p => p.Y);
		var maxX = grid.Keys.Max(p => p.X);

		var result = new Dictionary<Point, string>(grid.Count);
		for (var y = 0; y <= maxY; y++)
		{
			for (var x = 0; x <= maxX; x++)
			{
				result[new Point(x, y)] = ".";
			}
		}

		switch (direction)
		{
			case Direction.North:
				for (var x = 0; x <= maxX; x++)
				{
					var index = 0;
					for (var y = 0; y <= maxY; y++)
					{
						var p = new Point(x, y);
						var val = grid[p];

						switch (val)
						{
							case "O":
								result[new Point(x, index)] = "O";
								index++;
								break;
							case "#":
								result[p] = "#";
								index = y + 1;
								break;
						}
					}
				}

				break;

			case Direction.South:
				for (var x = 0; x <= maxX; x++)
				{
					var index = maxY;
					for (var y = maxY; y >= 0; y--)
					{
						var p = new Point(x, y);
						var val = grid[p];

						switch (val)
						{
							case "O":
								result[new Point(x, index)] = "O";
								index--;
								break;
							case "#":
								result[p] = "#";
								index = y - 1;
								break;
						}
					}
				}

				break;

			case Direction.West:
				for (var y = 0; y <= maxY; y++)
				{
					var index = 0;
					for (var x = 0; x <= maxX; x++)
					{
						var p = new Point(x, y);
						var val = grid[p];

						switch (val)
						{
							case "O":
								result[new Point(index, y)] = "O";
								index++;
								break;
							case "#":
								result[p] = "#";
								index = x + 1;
								break;
						}
					}
				}

				break;

			case Direction.East:
				for (var y = 0; y <= maxY; y++)
				{
					var index = maxX;
					for (var x = maxX; x >= 0; x--)
					{
						var p = new Point(x, y);
						var val = grid[p];

						switch (val)
						{
							case "O":
								result[new Point(index, y)] = "O";
								index--;
								break;
							case "#":
								result[p] = "#";
								index = x - 1;
								break;
						}
					}
				}

				break;
		}

		return result;
	}

	private static Dictionary<Point, string> ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, string>();
		var y = 0;
		foreach (var line in input)
		{
			for (var x = 0; x < line.Length; x++)
			{
				grid[new Point(x, y)] = line[x].ToString();
			}

			y++;
		}

		return grid;
	}

	private static int CountGrid(Dictionary<Point, string> grid)
	{
		var count = 0;
		var maxX = grid.Keys.Max(x => x.X);
		var maxY = grid.Keys.Max(x => x.Y);
		for (var y = 0; y <= maxY; y++)
		{
			for (var x = 0; x <= maxX; x++)
			{
				if (grid[new Point(x, y)] == "O")
					count += (maxY + 1) - y;
			}
		}

		return count;
	}

	private static int GetHash(Dictionary<Point, string> grid)
	{
		unchecked
		{
			var hash = 19;
			foreach (var kv in grid)
				hash = hash * 31 + kv.Key.GetHashCode() + kv.Value.GetHashCode();

			return hash;
		}
	}
}