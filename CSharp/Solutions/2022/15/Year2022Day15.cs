using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using AdventOfCode.Solutions._2020._19;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._15;

[UsedImplicitly]
public class Year2022Day15 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		input = new[]
		{
			"Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
			"Sensor at x=9, y=16: closest beacon is at x=10, y=16",
			"Sensor at x=13, y=2: closest beacon is at x=15, y=3",
			"Sensor at x=12, y=14: closest beacon is at x=10, y=16",
			"Sensor at x=10, y=20: closest beacon is at x=10, y=16",
			"Sensor at x=14, y=17: closest beacon is at x=10, y=16",
			"Sensor at x=8, y=7: closest beacon is at x=2, y=10",
			"Sensor at x=2, y=0: closest beacon is at x=2, y=10",
			"Sensor at x=0, y=11: closest beacon is at x=2, y=10",
			"Sensor at x=20, y=14: closest beacon is at x=25, y=17",
			"Sensor at x=17, y=20: closest beacon is at x=21, y=22",
			"Sensor at x=16, y=7: closest beacon is at x=15, y=3",
			"Sensor at x=14, y=3: closest beacon is at x=15, y=3",
			"Sensor at x=20, y=1: closest beacon is at x=15, y=3",
		};
		
		var grid = ParseInput(input);
		ShowScreen(grid);
		return 1;
	}
	
	// Sensor at x=2, y=18: closest beacon is at x=-2, y=15

	private static DefaultDictionary<Point, GridType> ParseInput(IEnumerable<string> input)
	{
		var grid = new DefaultDictionary<Point, GridType>();
		foreach (var line in input)
		{
			var splitted = line.Split(" ");
			var x = int.Parse(splitted[2].Replace("x=", string.Empty).Replace(",", string.Empty));
			var y = int.Parse(splitted[3].Replace("y=", string.Empty).Replace(":", string.Empty));
			var x2 = int.Parse(splitted[8].Replace("x=", string.Empty).Replace(",", string.Empty));
			var y2 = int.Parse(splitted[9].Replace("y=", string.Empty));
			grid.TryAdd(new Point(x, y), GridType.Sensor);
			grid.TryAdd(new Point(x2, y2), GridType.Beacon);
		}

		return grid;
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
	
	private enum GridType
	{
		Empty,
		Beacon,
		Sensor
	}
	

	public object Part2(IEnumerable<string> input)
	{
		return 2;
	}
	
	private static void ShowScreen(Dictionary<Point, GridType> grid)
	{
		var minX = grid.Min(x => x.Key.X);
		var maxX = grid.Max(x => x.Key.X);
		var minY = grid.Min(x => x.Key.Y);
		var maxY = grid.Max(x => x.Key.Y);
		var sb = new StringBuilder();
		sb.AppendLine();
		for (int y = minY; y <= maxY; y++)
		{
			for (int x = minX; x <= maxX; x++)
			{
				var tile = grid.GetValueOrDefault(new Point(x, y), GridType.Empty);
				sb.Append(tile switch
				{
					GridType.Empty => '.',
					GridType.Beacon => 'B',
					GridType.Sensor => 'S',
					_ => throw new ArgumentOutOfRangeException()
				});
			}

			sb.AppendLine();
		}

		Console.Write(sb.ToString());
	}

}
