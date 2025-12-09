using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._9;

[UsedImplicitly]
public class Year2025Day09 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var points = ParseInput(input);
		var orderedPoints = points.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();

		long maxArea = 0;
		for (var i = 0; i < orderedPoints.Count; i++)
		{
			for (var j = i + 1; j < orderedPoints.Count; j++)
			{
				var a = orderedPoints[i];
				var b = orderedPoints[j];
				var area = GetArea(a, b);
				if (area > maxArea)
					maxArea = area;
			}
		}
		
		return maxArea;
	}

	public object Part2(IEnumerable<string> input)
	{
		var points = ParseInput(input);

		long maxArea = 0;
		for (var i = 0; i < points.Count; i++)
		{
			for (var j = i + 1; j < points.Count; j++)
			{
				var minX = Math.Min(points[i].X, points[j].X);
				var maxX = Math.Max(points[i].X, points[j].X);
				var minY = Math.Min(points[i].Y, points[j].Y);
				var maxY = Math.Max(points[i].Y, points[j].Y);

				var valid = true;
				for (var k = 0; k < points.Count; k++)
				{
					var currX = points[k].X;
					var currY = points[k].Y;
					var nextX = points[(k + 1) % points.Count].X;
					var nextY = points[(k + 1) % points.Count].Y;
					
					if (!(minX >= Math.Max(currX, nextX) ||
					      maxX <= Math.Min(currX, nextX) ||
					      minY >= Math.Max(currY, nextY) ||
					      maxY <= Math.Min(currY, nextY)))
					{
						valid = false;
						break;
					}
				}
				
				if (!valid)
					continue;
				
				maxArea = Math.Max(maxArea, GetArea(points[i], points[j]));
			}
		}

		return maxArea;
	}

	private static long GetArea(Point a, Point b)
	{
		var width = (long)Math.Abs(b.X - a.X) + 1;
		var height = (long)Math.Abs(b.Y - a.Y) + 1;
		return width * height;
	}
	
	private static List<Point> ParseInput(IEnumerable<string> input)
	{
		var points = new List<Point>();
		foreach (var line in input)
		{
			var parts = line.Split(',');
			var point = new Point(int.Parse(parts[0]), int.Parse(parts[1]));
			points.Add(point);
		}

		return points;
	}
}