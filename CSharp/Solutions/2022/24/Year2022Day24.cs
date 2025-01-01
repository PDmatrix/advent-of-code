using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._24;

[UsedImplicitly]
public partial class Year2022Day24 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		var blizzards = grid.Where(x => new[] { ">", "<", "v", "^" }.Contains(x.Value))
			.Select(x => x.Key)
			.ToDictionary(x => x, x => new List<Point> { DirectionToPoint(grid[x]) });

		var globalBlizzards = new Dictionary<int, Dictionary<Point, List<Point>>>
		{
			[0] = blizzards,
			[1] = ComputeBlizzard(blizzards, grid)
		};

		for (var i = 1; i <= 500; i++)
		{
			globalBlizzards[i] = ComputeBlizzard(globalBlizzards[i - 1], grid);
		}

		var dirs = new[] { (0, 1), (1, 0), (0, -1), (-1, 0), (0, 0) };
		
		var minY = grid.Min(x => x.Key.Y);
		var maxY = grid.Max(x => x.Key.Y);

		var start = grid.Where(x => x.Key.Y == minY).Single(x => x.Value == ".").Key;
		var end = grid.Where(x => x.Key.Y == maxY).Single(x => x.Value == ".").Key;
		
		return FindMin(start, end, globalBlizzards, grid, 0);
	}

	private static Dictionary<Point, List<Point>> ComputeBlizzard(Dictionary<Point, List<Point>> currentBlizzards, Dictionary<Point, string> grid)
	{
		var b = new Dictionary<Point, List<Point>>();
		foreach (var blizzard in currentBlizzards)
		{
			foreach (var dir in blizzard.Value)
			{
				var newPoint = new Point(blizzard.Key.X + dir.X, blizzard.Key.Y + dir.Y);
				if (grid.TryGetValue(newPoint, out var v) && v == "#")
					newPoint = Wrap(grid, blizzard.Key, dir);
			
				if (!b.ContainsKey(newPoint))
					b[newPoint] = new List<Point>();
					
				b[newPoint].Add(dir);
			}
		}

		return b;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		var blizzards = grid.Where(x => new[] { ">", "<", "v", "^" }.Contains(x.Value))
			.Select(x => x.Key)
			.ToDictionary(x => x, x => new List<Point> { DirectionToPoint(grid[x]) });

		var globalBlizzards = new Dictionary<int, Dictionary<Point, List<Point>>>
		{
			[0] = blizzards,
			[1] = ComputeBlizzard(blizzards, grid)
		};

		for (var i = 1; i <= 1000; i++)
		{
			globalBlizzards[i] = ComputeBlizzard(globalBlizzards[i - 1], grid);
		}

		var minY = grid.Min(x => x.Key.Y);
		var maxY = grid.Max(x => x.Key.Y);

		var start = grid.Where(x => x.Key.Y == minY).Single(x => x.Value == ".").Key;
		var end = grid.Where(x => x.Key.Y == maxY).Single(x => x.Value == ".").Key;

		return FindMin(start, end, globalBlizzards, grid, FindMin(end, start, globalBlizzards, grid, FindMin(start, end, globalBlizzards, grid, 0)));
	}

	private static int FindMin(Point start, Point end, Dictionary<int, Dictionary<Point, List<Point>>> globalBlizzards, Dictionary<Point, string> grid, int t)
	{
		var minY = grid.Min(x => x.Key.Y);
		var dirs = new[] { (0, 1), (1, 0), (0, -1), (-1, 0), (0, 0) };
		var cache = new HashSet<(int, Point)>();
		var q = new Queue<(Point, int time)>();
		q.Enqueue((start, t));
		var min = int.MaxValue;
		while (q.Count != 0)
		{
			var (current, time) = q.Dequeue();
			
			if (!cache.Add((time, current)))
				continue;
			
			if (time >= min)
				continue;
			
			if (current == end)
			{
				min = Math.Min(min, time);
				continue;
			}

			var b = globalBlizzards[time + 1];
			foreach (var (dx, dy) in dirs)
			{
				var newPoint = new Point(current.X + dx, current.Y + dy);
				if (newPoint.Y < minY || grid.TryGetValue(newPoint, out var v) && v == "#")
					continue;
				
				if (b.ContainsKey(newPoint))
					continue;

				q.Enqueue((newPoint, time + 1));
			}
		}

		return min;
	}

	private static Point Wrap(Dictionary<Point, string> grid, Point current, Point direction)
	{
		switch (direction)
		{
			case { X: 0, Y: 1 }:
			{
				var minY = grid.Min(x => x.Key.Y);
				return current with { Y = minY + 1 };
			}
			case { X: 0, Y: -1 }:
			{
				var maxY = grid.Max(x => x.Key.Y);
				return current with { Y = maxY - 1 };
			}
			case { X: 1, Y: 0 }:
			{
				var minX = grid.Min(x => x.Key.X);
				return current with { X = minX + 1 };
			}
			case { X: -1, Y: 0 }:
			{
				var maxX = grid.Max(x => x.Key.X);
				return current with { X = maxX - 1 };
			}
			default:
				return Point.Empty;
		}
	}

	private static Point DirectionToPoint(string direction)
	{
		return direction switch
		{
			">" => new Point(1, 0),
			"<" => new Point(-1, 0),
			"v" => new Point(0, 1),
			"^" => new Point(0, -1),
		};
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
}