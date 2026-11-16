using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._11;

[UsedImplicitly]
public class Year2021Day11 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		var flashCount = 0;
		for (int step = 0; step < 100; step++)
		{
			foreach (var kv in grid)
				grid[kv.Key]++;

			var flashed = new HashSet<Point>();
			var stack = new Stack<Point>();
			foreach (var kv in grid.Where(x => x.Value > 9))
				stack.Push(kv.Key);

			while (stack.Count != 0)
			{
				var current = stack.Pop();
				if (flashed.Contains(current))
					continue;

				flashed.Add(current);
				grid[current] = 0;
				flashCount++;
				foreach (var adjacent in GetAdjacent(grid, current))
				{
					if (flashed.Contains(adjacent))
						continue;

					grid[adjacent]++;
					if (grid[adjacent] > 9)
						stack.Push(adjacent);
				}
			}
		}
		
		return flashCount;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		var steps = 0;
		while (true)
		{
			var flashCount = 0;
			foreach (var kv in grid)
				grid[kv.Key]++;

			var flashed = new HashSet<Point>();
			var stack = new Stack<Point>();
			foreach (var kv in grid.Where(x => x.Value > 9))
				stack.Push(kv.Key);

			while (stack.Count != 0)
			{
				var current = stack.Pop();
				if (flashed.Contains(current))
					continue;

				flashed.Add(current);
				grid[current] = 0;
				flashCount++;
				foreach (var adjacent in GetAdjacent(grid, current))
				{
					if (flashed.Contains(adjacent))
						continue;

					grid[adjacent]++;
					if (grid[adjacent] > 9)
						stack.Push(adjacent);
				}
			}
			
			steps++;
			
			if (flashCount == 100)
				break;
		}
		
		return steps;
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
	
	private static IEnumerable<Point> GetAdjacent(Dictionary<Point, int> grid, Point point)
	{
		var diff = new[]
		{
			(0, -1), (1, 0), (0, 1), (-1, 0),
			(1, 1), (-1, 1), (1, -1), (-1, -1)
		};
		foreach (var (dx, dy) in diff)
		{
			var newPoint = new Point(point.X + dx, point.Y + dy);
			if (grid.ContainsKey(newPoint))
				yield return newPoint;
		}
	}
}