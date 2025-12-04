using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._4;

[UsedImplicitly]
public class Year2025Day04 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		var answer = 0;
		foreach (var point in grid.Keys)
		{
			if (grid[point] != "@")
				continue;
			
			var adj = CountAdjacent(point, grid);
			if (adj < 4)
				answer++;
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		int removed;
		var answer = 0;
		do
		{
			removed = 0;
			foreach (var point in grid.Keys)
			{
				if (grid[point] != "@")
					continue;
			
				var adj = CountAdjacent(point, grid);
				if (adj < 4)
				{
					grid[point] = ".";
					removed++;
					answer++;
				}
			}
		} while (removed != 0);

		
		return answer;
	}
	
	private static int CountAdjacent(Point point, Dictionary<Point, string> grid)
	{
		var adjacentPoints = new[]
		{
			new Point(point.X - 1, point.Y - 1),
			point with { Y = point.Y - 1 },
			new Point(point.X + 1, point.Y - 1),
			point with { X = point.X - 1 },
			point with { X = point.X + 1 },
			new Point(point.X - 1, point.Y + 1),
			point with { Y = point.Y + 1 },
			new Point(point.X + 1, point.Y + 1),
		};
		
		return adjacentPoints.Count(p => grid.ContainsKey(p) && grid[p] == "@");
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