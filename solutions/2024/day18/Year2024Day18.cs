using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._18;

[UsedImplicitly]
public class Year2024Day18 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, string>();

		const int takeN = 1024;
		const int height = 70;
		const int width = 70;
		
		for (var i = 0; i < takeN; i++)
		{
			var split = input.ToList()[i].Split(",");
			grid[new Point(int.Parse(split[0]), int.Parse(split[1]))] = "#";
		}

		for (var y = 0; y <= height; y++)
		{
			for (var x = 0; x <= width; x++)
			{
				var p = new Point(x, y);
				grid.TryAdd(p, ".");
			}
		}

		var start = new Point(0, 0);
		var end = new Point(width, height);

		var (minSteps, _) = FindPath(start, end, grid, true);

		return minSteps;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, string>();

		const int takeN = 1024;
		const int height = 70;
		const int width = 70;

		for (var i = 0; i < takeN; i++)
		{
			var split = input.ToList()[i].Split(",");
			grid[new Point(int.Parse(split[0]), int.Parse(split[1]))] = "#";
		}

		for (var y = 0; y <= height; y++)
		{
			for (var x = 0; x <= width; x++)
			{
				var p = new Point(x, y);
				grid.TryAdd(p, ".");
			}
		}

		for (var i = takeN; i < input.Count(); i++)
		{
			var split = input.ToList()[i].Split(",");
			grid[new Point(int.Parse(split[0]), int.Parse(split[1]))] = "#";
			
			var start = new Point(0, 0);
			var end = new Point(width, height);
			
			var (_, pathFound) = FindPath(start, end, grid);

			if (!pathFound)
				return input.ToList()[i];
		}

		throw new Exception("No solution is found");
	}

	private static (int, bool) FindPath(Point start, Point end, Dictionary<Point, string> grid, bool findMin = false)
	{
		var visited = new HashSet<Point>();
		var queue = new Queue<(Point, int)>();
		queue.Enqueue((start, 0));

		var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

		var pathFound = false;
		var minSteps = int.MaxValue;
		while (queue.Count != 0)
		{
			var (current, steps) = queue.Dequeue();

			if (current == end)
			{
				if (findMin)
				{
					minSteps = Math.Min(minSteps, steps);
					continue;
				}
				
				pathFound = true;
				break;
			}

			if (!visited.Add(current))
				continue;

			foreach (var (dx, dy) in dirs)
			{
				var newPoint = new Point(current.X + dx, current.Y + dy);
				if (!grid.ContainsKey(newPoint))
					continue;

				if (grid.TryGetValue(newPoint, out var value) && value == "#")
					continue;

				queue.Enqueue((newPoint, steps + 1));
			}
		}

		return (minSteps, pathFound);
	}
}