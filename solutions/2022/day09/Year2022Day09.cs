using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._9;

[UsedImplicitly]
public class Year2022Day09 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, bool>();
		var head = new Point();
		var tail = new Point();
		foreach (var line in input)
		{
			var splt = line.Split();
			var count = int.Parse(splt[1]);
			var direction = splt[0];
			for (var i = 0; i < count; i++)
			{
				head = direction switch
				{
					"U" => head with { Y = head.Y + 1 },
					"D" => head with { Y = head.Y - 1 },
					"R" => head with { X = head.X + 1 },
					"L" => head with { X = head.X - 1 },
					_ => head
				};
				var diff = FindDifference(head, tail);
				tail = new Point(tail.X + diff.X, tail.Y + diff.Y);
				grid[tail] = true;
			}
		}

		return grid.Values.Count;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, bool>();
		var head = new Point();
		var tails = new List<Point>();
		for (var i = 0; i < 9; i++)
			tails.Add(new Point());
		
		foreach (var line in input)
		{
			var splt = line.Split();
			var count = int.Parse(splt[1]);
			var direction = splt[0];
			for (var i = 0; i < count; i++)
			{
				head = direction switch
				{
					"U" => head with { Y = head.Y + 1 },
					"D" => head with { Y = head.Y - 1 },
					"R" => head with { X = head.X + 1 },
					"L" => head with { X = head.X - 1 },
					_ => head
				};
				var diff = FindDifference(head, tails.First());
				tails[0] = new Point(tails[0].X + diff.X, tails[0].Y + diff.Y);
				for (var j = 1; j < tails.Count; j++)
				{
					var prevDiff = FindDifference(tails[j - 1], tails[j]);
					tails[j] = new Point(tails[j].X + prevDiff.X, tails[j].Y + prevDiff.Y);
				}
				
				grid[tails[^1]] = true;
			}
		}

		return grid.Values.Count;
	}

	private static Point FindDifference(Point head, Point tail)
	{
		var d = new Point(head.X - tail.X, head.Y - tail.Y);
		if (Math.Abs(d.X) < 2 && Math.Abs(d.Y) < 2)
			return new Point(0, 0);
			
		if (Math.Abs(d.X) == 2)
			d.X = d.X == -2 ? -1 : 1;
		if (Math.Abs(d.Y) == 2)
			d.Y = d.Y == -2 ? -1 : 1;

		return d;
	}
}