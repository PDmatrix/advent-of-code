using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using AdventOfCode.Solutions._2020._19;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._14;

[UsedImplicitly]
public class Year2022Day14 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		var pov = new Point(500, 0);
		var oldPov = pov;
		grid[pov] = GridType.Sand;
		var atRest = false;
		while (!atRest)
		{
			pov = Step(grid, pov, grid.Max(x => x.Key.Y));
			if (pov == new Point(500, -1))
				atRest = true;
			
			if (oldPov == pov)
			{
				pov = new Point(500, 0);
				grid[pov] = GridType.Sand;
			}

			oldPov = pov;
		}

		return grid.Count(x => x.Value == GridType.Sand) - 1;
	}

	private static Point Step(DefaultDictionary<Point, GridType> grid, Point pov, int maxY, bool partTwo = false)
	{
		if (!partTwo && pov.Y >= maxY)
			return new Point(500, -1);
		
		var below = pov with { Y = pov.Y + 1 };
		var belowLeft = pov with { X = pov.X - 1,  Y = pov.Y + 1 };
		var belowRight = pov with { X = pov.X + 1,  Y = pov.Y + 1 };
		
		if (partTwo && pov.Y == maxY + 1)
		{
			grid[below] = GridType.Wall;
			grid[belowLeft] = GridType.Wall;
			grid[belowRight] = GridType.Wall;
		}
		
		if (grid[below] != GridType.Empty && grid[belowLeft] != GridType.Empty && grid[belowRight] != GridType.Empty)
		{
			return pov;
		}
		
		
		if (grid[below] == GridType.Empty)
		{
			grid[pov] = GridType.Empty;
			grid[below] = GridType.Sand;
			return below;
		}
		
		if (grid[below] != GridType.Empty && grid[belowLeft] == GridType.Empty)
		{
			grid[pov] = GridType.Empty;
			grid[belowLeft] = GridType.Sand;
			return belowLeft;
		}
		
		if (grid[below] != GridType.Empty && grid[belowLeft] != GridType.Empty && grid[belowRight] == GridType.Empty)
		{
			grid[pov] = GridType.Empty;
			grid[belowRight] = GridType.Sand;
			return belowRight;
		}

		return pov;
	}
	
	private class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : new() where TKey : notnull
	{
		public new TValue this[TKey key]
		{
			get
			{
				if (TryGetValue(key, out var val)) 
					return val;
				val = new TValue();
				return val;
			}
			set => base[key] = value;
		}
	}

	private static DefaultDictionary<Point, GridType> ParseInput(IEnumerable<string> input)
	{
		var grid = new DefaultDictionary<Point, GridType>();
		foreach (var line in input)
		{
			var splitted = line.Split(" -> ");
			for (var i = 0; i < splitted.Length - 1; i++)
			{
				var (fromX, fromY) = splitted[i].Split(",");
				var (toX, toY) = splitted[i + 1].Split(",");
				if (fromX == toX)
				{
					var minY = Math.Min(int.Parse(fromY), int.Parse(toY));
					var maxY = Math.Max(int.Parse(fromY), int.Parse(toY));
					for (var j = minY; j <= maxY; j++)
						grid[new Point(int.Parse(fromX), j)] = GridType.Wall;
				}
				
				if (fromY == toY)
				{
					var minX = Math.Min(int.Parse(fromX), int.Parse(toX));
					var maxX = Math.Max(int.Parse(fromX), int.Parse(toX));
					for (var j = minX; j <= maxX; j++)
						grid[new Point(j, int.Parse(fromY))] = GridType.Wall;
				}

			}
		}

		return grid;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		var pov = new Point(500, 0);
		var oldPov = pov;
		grid[pov] = GridType.Sand;
		var atRest = false;
		var infinity = grid.Max(x => x.Key.Y);
		while (!atRest)
		{
			pov = Step(grid, pov, infinity, true);
			if (pov == new Point(500, 0))
				atRest = true;
			
			if (oldPov == pov)
			{
				pov = new Point(500, 0);
				grid[pov] = GridType.Sand;
			}
		
			oldPov = pov;
		}
		
		return grid.Count(x => x.Value == GridType.Sand);
	}
	
	private enum GridType
	{
		Empty,
		Wall,
		Sand
	}
}
