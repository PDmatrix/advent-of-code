using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._7;

[UsedImplicitly]
public class Year2025Day07 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		
		var start = grid.First(x => x.Value == "S").Key;
		var queue = new Queue<Point>();
		queue.Enqueue(start);
		var visited = new HashSet<Point>();
		
		var answer = 0;
		while (queue.Any())
		{
			var current = queue.Dequeue();
			if (!visited.Add(current))
				continue;
			
			var newPoint = current with { Y = current.Y + 1 };
			if (!grid.TryGetValue(newPoint, out var value))
				continue;

			if (value == ".")
			{
				queue.Enqueue(newPoint);
				continue;
			}
			
			answer++;
			queue.Enqueue(newPoint with { X = newPoint.X - 1 });
			queue.Enqueue(newPoint with { X = newPoint.X + 1 });
		}
		
		return answer;
	}
	
	private static readonly Dictionary<Point, long> Memo = new();
	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		
		var start = grid.First(x => x.Value == "S").Key;
		var answer = CountBeam(start, grid);
		return answer;
	}

	private static long CountBeam(Point start, Dictionary<Point, string> grid)
	{
		if (!grid.ContainsKey(start))
			return 1;
		
		if (Memo.TryGetValue(start, out var cached))
			return cached;
		
		var bottom = start with { Y = start.Y + 1 };
		if (grid[start] == ".")
		{
			Memo[start] = CountBeam(bottom, grid);
			return Memo[start];
		}
		
		var leftBottom = start with { X = start.X - 1, Y = start.Y + 1 };
		var rightBottom = start with { X = start.X + 1, Y = start.Y + 1 };
		
		Memo[start] = CountBeam(leftBottom, grid) + CountBeam(rightBottom, grid);
		return Memo[start];
	}
	
	private static Dictionary<Point, string> ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, string>();
		var y = 0;
		foreach (var line in input)
		{
			if (!line.Contains('^') && !line.Contains('S'))
				continue;
			
			for (var x = 0; x < line.Length; x++)
			{
				grid[new Point(x, y)] = line[x].ToString();
			}

			y++;
		}

		return grid;
	}
}