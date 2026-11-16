using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._10;

[UsedImplicitly]
public class Year2024Day10 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (grid, trailheads) = ParseGrid(input);

		var dirs = new List<(int, int)> { (0, 1), (0, -1), (1, 0), (-1, 0) };
		var score = 0;
		foreach (var trailhead in trailheads)
		{
			var queue = new Queue<Point>();
			queue.Enqueue(trailhead);
			var reached = new HashSet<Point>();
			while (queue.Count != 0)
			{
				var current = queue.Dequeue();

				if (grid.GetValueOrDefault(current, -1) == 9)
				{
					reached.Add(current);
					continue;
				}

				foreach (var (dx, dyy) in dirs)
				{
					var newPoint = new Point(current.X + dx, current.Y + dyy);
					if (!grid.TryGetValue(newPoint, out var value))
						continue;

					if (value - grid[current] != 1)
						continue;

					queue.Enqueue(newPoint);
				}
			}

			score += reached.Count;
		}

		return score;
	}

	public object Part2(IEnumerable<string> input)
	{
		var (grid, trailheads) = ParseGrid(input);

		var dirs = new List<(int, int)> { (0, 1), (0, -1), (1, 0), (-1, 0) };
		var score = 0;
		foreach (var trailhead in trailheads)
		{
			var queue = new Queue<Point>();
			queue.Enqueue(trailhead);
			var localScore = 0;
			while (queue.Count != 0)
			{
				var current = queue.Dequeue();

				if (grid.GetValueOrDefault(current, -1) == 9)
				{
					localScore++;
					continue;
				}

				foreach (var (dx, dyy) in dirs)
				{
					var newPoint = new Point(current.X + dx, current.Y + dyy);
					if (!grid.TryGetValue(newPoint, out var value))
						continue;

					if (value - grid[current] != 1)
						continue;

					queue.Enqueue(newPoint);
				}
			}

			score += localScore;
		}

		return score;
	}

	private static (Dictionary<Point, int>, List<Point>) ParseGrid(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, int>();
		var trailheads = new List<Point>();
		var enumerable = input as string[] ?? input.ToArray();
		for (var y = 0; y < enumerable.Length; y++)
		{
			for (var x = 0; x < enumerable[y].Length; x++)
			{
				var c = enumerable[y][x];
				var p = new Point(x, y);
				grid.Add(p, int.Parse(c.ToString()));
				if (c == '0')
					trailheads.Add(p);
			}
		}

		return (grid, trailheads);
	}
}