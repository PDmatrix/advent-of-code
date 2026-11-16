using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._9;

[UsedImplicitly]
public class Year2021Day09 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		var answer = 0;
		foreach (var kv in grid)
		{
			var adjacent = GetAdjacent(grid, kv.Key);
			answer += adjacent.All(x => x > kv.Value) ? kv.Value + 1 : 0;
		}

		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		var basins = new List<List<int>>();
		var visited = new HashSet<Point>();
		foreach (var kv in grid)
		{
			if (kv.Value == 9)
				continue;
			if (visited.Contains(kv.Key))
				continue;

			var queue = new Queue<Point>();
			var list = new List<int>();
			queue.Enqueue(kv.Key);
			
			while (queue.Count != 0)
			{
				var current = queue.Dequeue();
				if (visited.Contains(current))
					continue;

				visited.Add(current);
				list.Add(grid[current]);

				var adjacents = GetAdjacentV2(grid, current);
				foreach (var adjacent in adjacents)
				{
					if (grid[adjacent] == 9)
						continue;
					
					if (visited.Contains(adjacent))
						continue;

					queue.Enqueue(adjacent);
				}
			}
			basins.Add(list);
		}

		return basins.OrderByDescending(x => x.Count).Take(3).Aggregate(1, (acc, el) => acc * el.Count);
	}

	private static Dictionary<Point, int> ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, int>();
		var y = 0;
		foreach (var line in input)
		{
			for (var i = 0; i < line.Length; i++)
				grid[new Point(i, y)] = int.Parse(line[i].ToString());

			y++;
		}

		return grid;
	}

	private static IEnumerable<int> GetAdjacent(Dictionary<Point, int> grid, Point point)
	{
		var diff = new[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
		foreach (var (dx, dy) in diff)
		{
			var newPoint = new Point(point.X + dx, point.Y + dy);
			if (grid.TryGetValue(newPoint, out var value))
				yield return value;
		}
	}
	
	private static IEnumerable<Point> GetAdjacentV2(Dictionary<Point, int> grid, Point point)
	{
		var diff = new[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
		foreach (var (dx, dy) in diff)
		{
			var newPoint = new Point(point.X + dx, point.Y + dy);
			if (grid.ContainsKey(newPoint))
				yield return newPoint;
		}
	}

}