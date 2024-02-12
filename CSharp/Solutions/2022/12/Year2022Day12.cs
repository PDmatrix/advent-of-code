using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._12;

[UsedImplicitly]
public class Year2022Day12 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input, out var start, out var end);

		return ShortestPath(start, grid, end);
	}

	private static Dictionary<Point, char> ParseInput(IEnumerable<string> input, out Point start, out Point end)
	{
		var grid = new Dictionary<Point, char>();
		start = new Point();
		end = new Point();
		var y = 0;
		foreach (var line in input)
		{
			var x = 0;
			foreach (var elem in line)
			{
				var point = new Point(x, y);
				var e = elem;
				switch (elem)
				{
					case 'S':
						start = point;
						e = 'a';
						break;
					case 'E':
						end = point;
						e = 'z';
						break;
				}

				grid[point] = e;
				
				x++;
			}
			y++;
		}

		return grid;
	}

	private static int ShortestPath(Point start, Dictionary<Point, char> grid, Point end)
	{
		var queue = new Queue<(Point p, int n)>();
		queue.Enqueue((start, 0));
		var visited = new HashSet<Point>();
		
		while (queue.Count != 0)
		{
			var cur = queue.Dequeue();
			if (visited.Contains(cur.p))
				continue;
			
			foreach (var neighbour in GetNeighbours(cur.p, grid))
			{
				if (neighbour == end)
					return cur.n + 1;
				
				queue.Enqueue((neighbour, cur.n + 1));
			}

			visited.Add(cur.p);
		}

		return 100000;
	}

	private static readonly List<(int x, int y)> Diff = new() { (0, 1), (0, -1), (1, 0), (-1, 0) };
	private static IEnumerable<Point> GetNeighbours(Point p, Dictionary<Point, char> grid)
	{
		foreach (var (dx, dy) in Diff)
		{
			var newPoint = new Point(p.X + dx, p.Y + dy);
			if (!grid.ContainsKey(newPoint))
				continue;

			var cur = grid[p];
			
			if (cur - grid[newPoint] == -1 || cur - grid[newPoint] >= 0)
				yield return newPoint;
			
		}
	}
	
	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input, out var start, out var end);

		var maxX = grid.Max(x => x.Key.X);
		var maxY = grid.Max(x => x.Key.Y);
		var points = new List<Point>();
		for (var y = 0; y <= maxY; y++)
		{
			for (var x = 0; x <= maxX; x++)
			{
				var point = new Point(x, y);
				if (grid.TryGetValue(point, out var value) && value == 'a')
					points.Add(point);
			}
		}

		return points.Select(point => ShortestPath(point, grid, end)).Prepend(10000).Min();
	}
}