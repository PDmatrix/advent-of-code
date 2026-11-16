using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._10;

[UsedImplicitly]
public class Year2023Day10 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		
		var start = grid.Single(x => x.Value == 'S').Key;
		
		var seen = new HashSet<Point>();
		var queue = new Queue<(Point, int)>();
		queue.Enqueue((start, 0));
		seen.Add(start);
		while (queue.Count > 0)
		{
			var (current, steps) = queue.Dequeue();
			var paths = GetValidPaths(current, grid).ToList();

			var next = paths.FirstOrDefault(x => !seen.Contains(x));
			if (next == Point.Empty)
				return (steps + 1) / 2;
			
			if (!seen.Add(next))
				continue;
			
			queue.Enqueue((next, steps + 1));
		}
		
		return 1;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		
		var start = grid.Single(x => x.Value == 'S').Key;
		
		var seen = new HashSet<Point>();
		var queue = new Queue<(Point, int)>();
		queue.Enqueue((start, 0));
		seen.Add(start);
		while (queue.Count > 0)
		{
			var (current, steps) = queue.Dequeue();
			var paths = GetValidPaths(current, grid).ToList();

			var next = paths.FirstOrDefault(x => !seen.Contains(x));
			if (next == Point.Empty)
				break;
			
			if (!seen.Add(next))
				continue;
			
			queue.Enqueue((next, steps + 1));
		}

		var answer = 0;
		foreach (var (p, c) in grid)
		{
			if (seen.Contains(p))
				continue;

			var newPoint = p;
			var intersect = 0;
			while (newPoint.X > 0)
			{
				newPoint.X--;
				if (seen.Contains(newPoint) && (grid[newPoint] == '|' || grid[newPoint] == 'L' || grid[newPoint] == 'J' || grid[newPoint] == 'S'))
					intersect++;
			}

			if (intersect % 2 == 1)
				answer++;
		}

		return answer;
	}

	private static IEnumerable<Point> GetValidPaths(Point start, Dictionary<Point, char> grid)
	{
		var valid = new Dictionary<Point, string>
		{
			[new Point(0, 1)] = "|JLS",
			[new Point(0, -1)] = "|F7S",
			[new Point(1, 0)] = "-J7S",
			[new Point(-1, 0)] = "-FLS",
		};

		var validDirections = new Dictionary<char, List<Point>>
		{
			['|'] = new() { new Point(0, 1), new Point(0, -1) },
			['J'] = new() { new Point(-1, 0), new Point(0, -1) },
			['L'] = new() { new Point(1, 0), new Point(0, -1) },
			['F'] = new() { new Point(1, 0), new Point(0, 1) },
			['7'] = new() { new Point(-1, 0), new Point(0, 1) },
			['-'] = new() { new Point(-1, 0), new Point(1, 0) },
			['S'] = new() { new Point(-1, 0), new Point(1, 0), new Point(0, 1), new Point(0, -1) },
		};
		
		var current = grid[start];

		foreach (var direction in validDirections[current])
		{
			var newPoint = new Point(start.X + direction.X, start.Y + direction.Y);
			if (!grid.ContainsKey(newPoint) || !valid[direction].Contains(grid[newPoint]))
				continue;
			
			yield return newPoint;
		}
	}
	
	private static Dictionary<Point, char> ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, char>();
		var y = 0;
		foreach (var line in input)
		{
			var x = 0;
			foreach (var c in line)
			{
				grid[new Point(x, y)] = c;
				x++;
			}

			y++;
		}

		return grid;
	}
}