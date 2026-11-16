using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2021._5;

[UsedImplicitly]
public class Year2021Day05 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var lines = ParseInput(input);

		lines = lines.Where(x => x.to.X == x.from.X || x.to.Y == x.from.Y).ToList();
		var grid = new Dictionary<Point, int>();
		foreach (var line in lines)
		{
			if (line.from.X == line.to.X)
			{
				var min = Math.Min(line.from.Y, line.to.Y);
				var max = Math.Max(line.from.Y, line.to.Y);
				for (var i = min; i <= max; i++)
				{
					var newPoint = line.from with { Y = i };
					if (grid.ContainsKey(newPoint))
						grid[newPoint]++;
					else
						grid[newPoint] = 1;
				}
			}
			else
			{
				var min = Math.Min(line.from.X, line.to.X);
				var max = Math.Max(line.from.X, line.to.X);
				for (var i = min; i <= max; i++)
				{
					var newPoint = line.from with { X = i };
					if (grid.ContainsKey(newPoint))
						grid[newPoint]++;
					else
						grid[newPoint] = 1;
				}
			}
		}

		return grid.Count(x => x.Value >= 2);
	}

	public object Part2(IEnumerable<string> input)
	{
		var lines = ParseInput(input);

		var grid = new Dictionary<Point, int>();
		foreach (var line in lines)
		{
			if (line.from.X == line.to.X)
			{
				var min = Math.Min(line.from.Y, line.to.Y);
				var max = Math.Max(line.from.Y, line.to.Y);
				for (var i = min; i <= max; i++)
				{
					var newPoint = line.from with { Y = i };
					if (grid.ContainsKey(newPoint))
						grid[newPoint]++;
					else
						grid[newPoint] = 1;
				}
			}
			else if (line.from.Y == line.to.Y)
			{
				var min = Math.Min(line.from.X, line.to.X);
				var max = Math.Max(line.from.X, line.to.X);
				for (var i = min; i <= max; i++)
				{
					var newPoint = line.from with { X = i };
					if (grid.ContainsKey(newPoint))
						grid[newPoint]++;
					else
						grid[newPoint] = 1;
				}
			}
			else
			{
				var diff = Math.Abs(line.from.X - line.to.X);
				for (var i = 0; i <= diff; i++)
				{
					var newX = line.from.X + (line.from.X > line.to.X ? i * -1 : i);
					var newY = line.from.Y + (line.from.Y > line.to.Y ? i * -1 : i);
					var newPoint = new Point(newX, newY);
					if (grid.ContainsKey(newPoint))
						grid[newPoint]++;
					else
						grid[newPoint] = 1;
				}
			}
		}

		return grid.Count(x => x.Value >= 2);
	}

	private static List<(Point from, Point to)> ParseInput(IEnumerable<string> input)
	{
		var lines = new List<(Point from, Point to)>();
		var regex = new Regex(@"(?<fx>\d+),(?<fy>\d+) -> (?<sx>\d+),(?<sy>\d+)", RegexOptions.Compiled);
		foreach (var line in input)
		{
			var match = regex.Match(line);
			lines.Add((new Point(int.Parse(match.Groups["fx"].Value), int.Parse(match.Groups["fy"].Value)),
				new Point(int.Parse(match.Groups["sx"].Value), int.Parse(match.Groups["sy"].Value))));
		}

		return lines;
	}
}