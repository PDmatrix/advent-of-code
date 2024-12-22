using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._20;

[UsedImplicitly]
public class Year2024Day20 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);

		var start = grid.Single(x => x.Value == "S").Key;
		var end = grid.Single(x => x.Value == "E").Key;

		var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
		var dirs2 = new[] { (0, 2), (0, -2), (2, 0), (-2, 0) };

		var steps = new Dictionary<Point, int>();
		var curLoc = start;
		steps[start] = 0;
		var stepCount = 0;
		var savingsCount = 0;

		do
		{
			foreach (var (dx, dy) in dirs)
			{
				var newPoint = new Point(curLoc.X + dx, curLoc.Y + dy);
				if (grid[newPoint] != "#" && !steps.ContainsKey(newPoint))
				{
					curLoc = newPoint;
					break;
				}
			}

			steps[curLoc] = ++stepCount;
		} while (curLoc != end);
		

		foreach( var (loc, s) in steps)
		{
			foreach (var (dx, dy) in dirs2)
			{
				var newPoint = new Point(loc.X + dx, loc.Y + dy);
				if (!steps.ContainsKey(newPoint))
					continue;
				
				var saved = steps[newPoint] - steps[loc] - 2;
				if (saved >= 100)
					savingsCount++;
			}
		}

		return savingsCount;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);

		var start = grid.Single(x => x.Value == "S").Key;
		var end = grid.Single(x => x.Value == "E").Key;

		var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

		var steps = new Dictionary<Point, int>();
		var curLoc = start;
		steps[start] = 0;
		var stepCount = 0;
		var savingsCount = 0;

		do
		{
			foreach (var (dx, dy) in dirs)
			{
				var newPoint = new Point(curLoc.X + dx, curLoc.Y + dy);
				if (grid[newPoint] != "#" && !steps.ContainsKey(newPoint))
				{
					curLoc = newPoint;
					break;
				}
			}

			steps[curLoc] = ++stepCount;
		} while (curLoc != end);
		
		foreach( var (loc, s) in steps)
		{
			foreach (var newPoint in steps.Where(x => ManhattanDistance(x.Key, loc) <= 20))
			{
				if (!steps.ContainsKey(newPoint.Key))
					continue;
				
				var saved = newPoint.Value - steps[loc] - ManhattanDistance(newPoint.Key, loc);
				if (saved >= 100)
					savingsCount++;
			}
		}

		return savingsCount;
	}
	
	private static int ManhattanDistance(Point a, Point b)
	{
		return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
	}

	private static Dictionary<Point, string> ParseGrid(IEnumerable<string> input)
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