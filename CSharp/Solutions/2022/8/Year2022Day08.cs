using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._8;

[UsedImplicitly]
public class Year2022Day08 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		var maxX = grid.Max(x => x.Key.X);
		var maxY = grid.Max(x => x.Key.Y);
		var dirs = new List<Point>
		{
			new(1, 0),
			new(0, 1),
			new(-1, 0),
			new(0, -1)
		};
		var answer = 0;
		for (var y = 0; y <= maxY; y++)
		{
			for (var x = 0; x <= maxX; x++)
			{
				foreach (var dir in dirs)
				{
					if (!IsVisible(grid, new Point(x, y), dir))
						continue;
					
					answer += 1;
					break;
				}
			}
		}

		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		var maxX = grid.Max(x => x.Key.X);
		var maxY = grid.Max(x => x.Key.Y);
		var dirs = new List<Point>
		{
			new(1, 0),
			new(0, 1),
			new(-1, 0),
			new(0, -1)
		};
		var answer = 0;
		for (var y = 0; y <= maxY; y++)
		{
			for (var x = 0; x <= maxX; x++)
			{
				var howMany = 1;
				foreach (var dir in dirs)
					howMany *= HowManyVisible(grid, new Point(x, y), dir, 0);
				
				answer = Math.Max(answer, howMany);
			}
		}

		return answer;
	}

	private static bool IsVisible(Dictionary<Point, int> grid, Point current, Point direction)
	{
		var newPoint = current with { X = current.X + direction.X, Y = current.Y + direction.Y };
		if (!grid.ContainsKey(newPoint))
			return true;

		if (grid[current] <= grid[newPoint])
			return false;

		if (direction.X > 0)
			direction.X += 1;
		if (direction.X < 0)
			direction.X -= 1;
		
		if (direction.Y > 0)
			direction.Y += 1;
		if (direction.Y < 0)
			direction.Y -= 1;


		return IsVisible(grid, current, direction);
	}
	
	private static int HowManyVisible(Dictionary<Point, int> grid, Point current, Point direction, int c)
	{
		var newPoint = current with { X = current.X + direction.X, Y = current.Y + direction.Y };
		if (!grid.ContainsKey(newPoint))
			return c;

		if (grid[current] <= grid[newPoint])
			return c + 1;

		if (direction.X > 0)
			direction.X += 1;
		if (direction.X < 0)
			direction.X -= 1;
		
		if (direction.Y > 0)
			direction.Y += 1;
		if (direction.Y < 0)
			direction.Y -= 1;


		return 1 + HowManyVisible(grid, current, direction, c);
	}

	private static Dictionary<Point, int> ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, int>();
		var y = 0;
		foreach (var line in input)
		{
			var x = 0;
			foreach (var c in line)
			{
				grid[new Point(x, y)] = int.Parse(c.ToString());
				x++;
			}

			y++;
		}

		return grid;
	}
}