using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._3;

[UsedImplicitly]
public class Year2023Day03 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		var lastNumber = new List<Point>();
		var answer = 0;
		foreach (var (k, v) in grid)
		{
			if (char.IsDigit(v))
			{
				lastNumber.Add(k);
				continue;
			}

			if (IsPartNumber(lastNumber, grid))
				answer += int.Parse(string.Join("", lastNumber.Select(x => grid[x])));

			lastNumber = new List<Point>();
		}

		if (IsPartNumber(lastNumber, grid))
			answer += int.Parse(string.Join("", lastNumber.Select(x => grid[x])));

		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		var lastNumber = new List<Point>();
		var gears = new Dictionary<Point, List<List<Point>>>();
		foreach (var (k, v) in grid)
		{
			if (char.IsDigit(v))
			{
				lastNumber.Add(k);
				continue;
			}

			var gear = NextToGear(lastNumber, grid);
			if (gear != Point.Empty)
			{
				if (!gears.ContainsKey(gear))
					gears[gear] = new List<List<Point>>();

				gears[gear].Add(lastNumber);
			}

			lastNumber = new List<Point>();
		}

		var gearO = NextToGear(lastNumber, grid);
		if (gearO != Point.Empty)
		{
			if (!gears.ContainsKey(gearO))
				gears[gearO] = new List<List<Point>>();

			gears[gearO].Add(lastNumber);
		}

		var answer = 0;
		foreach (var (k, v) in gears)
		{
			if (v.Count != 2)
				continue;

			answer += v.Aggregate(1,
				(current, nums) => current * int.Parse(string.Join("", nums.Select(x => grid[x]))));
		}

		return answer;
	}

	private static bool IsPartNumber(List<Point> numbers, Dictionary<Point, char> grid)
	{
		var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0), (-1, 1), (-1, -1), (1, -1), (1, 1) };
		foreach (var n in numbers)
		{
			foreach (var (dx, dy) in dirs)
			{
				var newPoint = new Point(n.X + dx, n.Y + dy);
				var val = grid.GetValueOrDefault(newPoint, '.');
				if (val != '.' && !char.IsDigit(val))
					return true;
			}
		}

		return false;
	}

	private static Point NextToGear(List<Point> numbers, Dictionary<Point, char> grid)
	{
		var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0), (-1, 1), (-1, -1), (1, -1), (1, 1) };
		foreach (var n in numbers)
		{
			foreach (var (dx, dy) in dirs)
			{
				var newPoint = new Point(n.X + dx, n.Y + dy);
				var val = grid.GetValueOrDefault(newPoint, '.');
				if (val == '*')
					return newPoint;
			}
		}

		return Point.Empty;
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