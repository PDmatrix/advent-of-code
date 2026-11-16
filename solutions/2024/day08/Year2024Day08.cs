using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._8;

[UsedImplicitly]
public class Year2024Day08 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (grid, antennas) = ParseGrid(input);
		var hs = new HashSet<Point>();
		foreach (var kv in antennas)
		{
			foreach (var point in kv.Value)
			{
				foreach (var neighbour in kv.Value)
				{
					if (point == neighbour)
						continue;

					var pointToCheck = GetNextPoint(neighbour, point);

					if (!grid.ContainsKey(GetNextPoint(neighbour, point)))
						continue;
					
					hs.Add(pointToCheck);
				}
			}
		}

		return hs.Count;
	}

	private static Point GetNextPoint(Point neighbour, Point point)
	{
		var upY = neighbour.Y - Math.Abs(point.Y - neighbour.Y);
		var downY = neighbour.Y + Math.Abs(point.Y - neighbour.Y);
		var leftX = neighbour.X - Math.Abs(point.X - neighbour.X);
		var rightX = neighbour.X + Math.Abs(point.X - neighbour.X);

		if (neighbour.Y > point.Y)
			return neighbour.X > point.X ? new Point(rightX, downY) : new Point(leftX, downY);
		
		return neighbour.X > point.X ? new Point(rightX, upY) : new Point(leftX, upY);
	}

	public object Part2(IEnumerable<string> input)
	{
		var (grid, antennas) = ParseGrid(input);
		var hs = new HashSet<Point>();
		foreach (var kv in antennas)
		{
			foreach (var point in kv.Value)
			{
				foreach (var neighbour in kv.Value)
				{
					if (point == neighbour)
						continue;

					hs.Add(neighbour);
					var pointToCheck = GetNextPoint(neighbour, point);

					var prevNeighbour = neighbour;
					while (grid.ContainsKey(pointToCheck))
					{
						hs.Add(pointToCheck);
						
						var prevPoint = pointToCheck;
						pointToCheck = GetNextPoint(pointToCheck, prevNeighbour);
						prevNeighbour = prevPoint;
					}
				}
			}
		}

		return hs.Count;
	}

	private static (Dictionary<Point, char>, Dictionary<char, List<Point>>) ParseGrid(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, char>();
		var antennas = new Dictionary<char, List<Point>>();
		var enumerable = input as string[] ?? input.ToArray();
		for (var y = 0; y < enumerable.Length; y++)
		{
			for (var x = 0; x < enumerable[y].Length; x++)
			{
				var c = enumerable[y][x];
				var p = new Point(x, y);
				grid.Add(p, c);
				if (c == '.')
					continue;

				if (!antennas.ContainsKey(c))
					antennas[c] = new List<Point>();

				antennas[c].Add(p);
			}
		}

		return (grid, antennas);
	}
}