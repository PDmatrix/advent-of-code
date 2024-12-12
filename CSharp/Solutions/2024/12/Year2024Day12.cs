using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._12;

[UsedImplicitly]
public class Year2024Day12 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);
		var visited = new HashSet<Point>();
		var answer = 0;
		var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
		foreach (var k in grid)
		{
			if (visited.Contains(k.Key))
				continue;
			
			var area = 0;
			var perimeter = 0;
			var queue = new Queue<Point>();
			queue.Enqueue(k.Key);
			while (queue.Count != 0)
			{
				var current = queue.Dequeue();
				if (!visited.Add(current))
					continue;

				area++;
				
				foreach (var (dx, dy) in dirs)
				{
					var newPoint = new Point(current.X + dx, current.Y + dy);
					if (!grid.TryGetValue(newPoint, out var value) || value != k.Value)
					{
						perimeter++;
						continue;
					}
					
					queue.Enqueue(newPoint);
				}
			}

			answer += area * perimeter;
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);
		var visited = new HashSet<Point>();
		var answer = 0;
		var dirs = new[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
		foreach (var k in grid)
		{
			if (visited.Contains(k.Key))
				continue;

			var area = 0;
			var corners = 0;
			var queue = new Queue<Point>();
			queue.Enqueue(k.Key);
			while (queue.Count != 0)
			{
				var current = queue.Dequeue();
				if (!visited.Add(current))
					continue;

				area++;

				foreach (var (dx, dy) in dirs)
				{
					var newPoint = new Point(current.X + dx, current.Y + dy);
					if (!grid.TryGetValue(newPoint, out var value) || value != k.Value)
						continue;

					queue.Enqueue(newPoint);
				}

				// corners == sides
				for (var i = 0; i < dirs.Length; i++)
				{
					var a = dirs[i];
					var b = dirs[(i + 1) % dirs.Length];
					
					var leftPoint = new Point(current.X + a.Item1, current.Y + a.Item2);
					var rightPoint = new Point(current.X + b.Item1, current.Y + b.Item2);
					var midPoint = new Point(current.X + a.Item1 + b.Item1, current.Y + a.Item2 + b.Item2);

					if ((grid.GetValueOrDefault(leftPoint, "") != grid[current] &&
					     grid.GetValueOrDefault(rightPoint, "") != grid[current]) ||
					    (grid.GetValueOrDefault(leftPoint, "") == grid[current] &&
					     grid.GetValueOrDefault(rightPoint, "") == grid[current] &&
					     grid.GetValueOrDefault(midPoint, "") != grid[current]))
						corners++;
				}
			}

			answer += area * corners;
		}

		return answer;
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
}