using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._15;

[UsedImplicitly]
public class Year2024Day15 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (grid, movements) = ParseInput(input);

		var robot = grid.Single(x => x.Value == "@").Key;
		var conversion = new Dictionary<string, (int X, int Y)>
		{
			{ "<", (-1, 0) },
			{ "^", (0, -1) },
			{ ">", (1, 0) },
			{ "v", (0, 1) },
		};
		
		foreach (var move in movements)
		{
			var nextPoint = new Point(robot.X + conversion[move].X, robot.Y + conversion[move].Y);
			if (grid[nextPoint] == "#")
				continue;

			if (grid[nextPoint] == ".")
			{
				grid[nextPoint] = "@";
				grid[robot] = ".";
				robot = nextPoint;
				continue;
			}
			
			var nextEmptySpace = new Point(nextPoint.X + conversion[move].X, nextPoint.Y + conversion[move].Y);
			while (grid[nextEmptySpace] != "." && grid[nextEmptySpace] != "#")
				nextEmptySpace = new Point(nextEmptySpace.X + conversion[move].X, nextEmptySpace.Y + conversion[move].Y);

			if (grid[nextEmptySpace] == "#")
				continue;

			grid[nextEmptySpace] = "O";
			grid[nextPoint] = "@";
			grid[robot] = ".";
			robot = nextPoint;
		}

		return grid.Where(x => x.Value == "O").Sum(x => x.Key.Y * 100 + x.Key.X);
	}

	private static (Dictionary<Point, string>, List<string>) ParseInput(IEnumerable<string> input)
	{
		var enumerable = input.ToList();
		
		var inputGrid = enumerable.TakeWhile(line => !string.IsNullOrEmpty(line)).ToList();
		var grid = ParseGrid(inputGrid);
		
		var movements = new List<string>();
		foreach (var line in enumerable.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1))
			movements.AddRange(line.Select(x => x.ToString()));

		return (grid, movements);
	}

	public object Part2(IEnumerable<string> input)
	{
		var (grid, movements) = ParseInput(input);

		grid = DoubleGrid(grid);;
		
		var robot = grid.Single(x => x.Value == "@").Key;
		var conversion = new Dictionary<string, (int X, int Y)>
		{
			{ "<", (-1, 0) },
			{ "^", (0, -1) },
			{ ">", (1, 0) },
			{ "v", (0, 1) },
		};
		
		foreach (var move in movements)
		{
			var nextPoint = new Point(robot.X + conversion[move].X, robot.Y + conversion[move].Y);
			if (grid[nextPoint] == "#")
				continue;

			if (grid[nextPoint] == ".")
			{
				grid[nextPoint] = "@";
				grid[robot] = ".";
				robot = nextPoint;
				continue;
			}
			
			if (!IsMovable(grid, nextPoint, (conversion[move].X, conversion[move].Y)))
				continue;
			
			Move(grid, nextPoint, (conversion[move].X, conversion[move].Y));
			
			grid[nextPoint] = "@";
			grid[robot] = ".";
			robot = nextPoint;
		}

		return Math.Min(grid.Where(x => x.Value == "[").Sum(x => x.Key.Y * 100 + x.Key.X),
			grid.Where(x => x.Value == "]").Sum(x => x.Key.Y * 100 + x.Key.X));
	}

	private static Dictionary<Point, string> DoubleGrid(Dictionary<Point, string> grid)
	{
		var newGrid = new Dictionary<Point, string>();
		var maxX = grid.Max(x => x.Key.X);
		var maxY = grid.Max(x => x.Key.Y);
		for (var y = 0; y <= maxY; y++)
		{
			for (var x = 0; x <= maxX; x++)
			{
				var orig = new Point(x, y);
				var p = new Point(x + x, y);
				if (grid[orig] == "#")
				{
					newGrid[p] = "#";
					newGrid[new Point(p.X + 1, y)] = "#";
					continue;
				}

				if (grid[orig] == "O")
				{
					newGrid[p] = "[";
					newGrid[new Point(p.X + 1, y)] = "]";
					continue;
				}

				if (grid[orig] == ".")
				{
					newGrid[p] = ".";
					newGrid[new Point(p.X + 1, y)] = ".";
					continue;
				}

				if (grid[orig] == "@")
				{
					newGrid[p] = "@";
					newGrid[new Point(p.X + 1, y)] = ".";
					continue;
				}
			}
		}

		return newGrid;
	}

	private static bool IsMovable(Dictionary<Point, string> grid, Point start, (int X, int Y) direction)
	{
		var left = Point.Empty;
		var right = Point.Empty;
		if (grid[start] == "[")
		{
			left = start;
			right = start with { X = start.X + 1 };
		}
		else
		{
			right = start;
			left = start with { X = start.X - 1 };
		}

		// up or down
		if (direction.X == 0)
		{
			if (grid[left with { Y = left.Y + direction.Y }] == "#" || grid[right with { Y = left.Y + direction.Y }] == "#")
				return false;

			var isLeftMovable = true;
			if (grid[left with { Y = left.Y + direction.Y }] == "[" ||
			    grid[left with { Y = left.Y + direction.Y }] == "]")
			{
				isLeftMovable = IsMovable(grid, left with { Y = left.Y + direction.Y }, direction);
			}

			var isRightMovable = true;
			if (grid[right with { Y = right.Y + direction.Y }] == "[" ||
			    grid[right with { Y = right.Y + direction.Y }] == "]")
			{
				isRightMovable = IsMovable(grid, right with { Y = right.Y + direction.Y }, direction);
			}
			
			if (!isLeftMovable || !isRightMovable)
				return false;
		}
		// left or right
		else
		{
			// left
			if (direction.X == -1)
			{
				if (grid[left with { X = left.X - 1 }] == "#")
					return false;

				var isLeftMovable = true;
				if (grid[left with { X = left.X - 1 }] == "]")
				{
					isLeftMovable = IsMovable(grid, left with{X = left.X - 1}, direction);
				}
				
				if (!isLeftMovable)
					return false;
			}
			
			// right
			if (direction.X == 1)
			{
				if (grid[right with { X = right.X + 1 }] == "#")
					return false;

				var isRightMovable = true;
				if (grid[right with { X = right.X + 1 }] == "[")
				{
					isRightMovable = IsMovable(grid, right with{X = right.X + 1}, direction);
				}
				
				if (!isRightMovable)
					return false;
			}
		}

		return true;
	}

	private static void Move(Dictionary<Point, string> grid, Point start, (int X, int Y) direction)
	{
		var left = Point.Empty;
		var right = Point.Empty;
		if (grid[start] == "[")
		{
			left = start;
			right = start with { X = start.X + 1 };
		}
		else
		{
			right = start;
			left = start with { X = start.X - 1 };
		}

		// up or down
		if (direction.X == 0)
		{
			var nextLeft = left with { Y = left.Y + direction.Y };
			var nextRight = right with { Y = right.Y + direction.Y };
			if (grid[nextLeft] == "]")
			{
				Move(grid, nextLeft, direction);
			}

			if (grid[nextRight] == "[")
			{
				Move(grid, nextRight, direction);
			}

			if (grid[nextLeft] == "[" && grid[nextRight] == "]")
			{
				Move(grid, nextLeft, direction);
			}
			if (grid[nextLeft] == "." &&
			    grid[nextRight] == ".")
			{
				grid[nextLeft] = "[";
				grid[nextRight] = "]";
				grid[left] = ".";
				grid[right] = ".";
			}

		}
		// left or right
		else
		{
			// left
			if (direction.X == -1)
			{
				var nextLeft = left with { X = left.X - 1 };
				
				if (grid[nextLeft] == "]")
				{
					Move(grid, nextLeft, direction);
				}
				
				if (grid[nextLeft] == ".")
				{
					grid[nextLeft] = "[";
					grid[left] = "]";
					grid[right] = ".";
					return;
				}
			}

			// right
			if (direction.X == 1)
			{
				var nextRight = right with { X = right.X + 1 };
				if (grid[nextRight] == "[")
				{
					Move(grid, nextRight, direction);
				}
				
				if (grid[nextRight] == ".")
				{
					grid[nextRight] = "]";
					grid[right] = "[";
					grid[left] = ".";
				}
			}
		}

	}

	private static Dictionary<Point, string>  ParseGrid(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, string>();
		var enumerable = input as string[] ?? input.ToArray();
		for (var y = 0; y < enumerable.Length; y++)
		{
			for (var x = 0; x < enumerable[y].Length; x++)
			{
				var c = enumerable[y][x];
				var p = new Point(x, y);
				grid.Add(p, c.ToString());
			}
		}

		return grid;
	}
	
	private static void ShowScreen(Dictionary<Point, string> grid)
	{
		var minX = grid.Min(x => x.Key.X);
		var maxX = grid.Max(x => x.Key.X);
		var minY = grid.Min(x => x.Key.Y);
		var maxY = grid.Max(x => x.Key.Y);
		var sb = new StringBuilder();
		sb.AppendLine();
		for (int y = minY; y <= maxY; y++)
		{
			for (int x = minX; x <= maxX; x++)
			{
				var tile = grid.GetValueOrDefault(new Point(x, y), ".");
				sb.Append(tile);
			}

			sb.AppendLine();
		}

		Console.Write(sb.ToString());
	}
}