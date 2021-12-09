using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2018._11;

[UsedImplicitly]
public class Year2018Day11 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var serialNumber = int.Parse(input.First());
		var grid = new int[301, 301];

		for (var x = 1; x <= 300; x++)
		for (var y = 1; y <= 300; y++)
			grid[x, y] = GetPowerLevel(serialNumber, x, y);

		var max = (maxX: 0, maxY: 0, level: 0);
		for (var x = 1; x <= 300 - 3; x++)
		for (var y = 1; y <= 300 - 3; y++)
		{
			var gridLevel = GetGridLevel(grid, x, y, 3);
			if (gridLevel > max.level)
				max = (x, y, gridLevel);
		}

		var (maxX, maxY, _) = max;

		return $"{maxX},{maxY}";
	}

	private static int GetGridLevel(int[,] grid, int x, int y, int size)
	{
		var res = 0;
		for (var i = x; i < x + size; i++)
		for (var j = y; j < y + size; j++)
			res += grid[i, j];

		return res;
	}

	private static int GetPowerLevel(int serialNumber, int x, int y)
	{
		var rackId = x + 10;
		var startingPowerLevel = rackId * y;
		var withSerialNumber = startingPowerLevel + serialNumber;
		var multipliedWithRackId = withSerialNumber * rackId;
		var hundredsDigit = multipliedWithRackId / 100 % 10;

		return hundredsDigit - 5;
	}

	public object Part2(IEnumerable<string> input)
	{
		var serialNumber = int.Parse(input.First());
		var grid = new int[301, 301];

		for (var x = 1; x <= 300; x++)
		for (var y = 1; y <= 300; y++)
			grid[x, y] = GetPowerLevel(serialNumber, x, y);

		var max = (maxX: 0, maxY: 0, level: 0, size: 0);
		for (var x = 1; x <= 300; x++)
		for (var y = 1; y <= 300; y++)
		{
			var score = grid[x, y];
			for (var size = 2; size <= 300; size++)
			{
				if (x + size > 300 || y + size > 300)
					break;

				for (var dx = 0; dx < size; dx++)
					score += grid[x + dx, y + size - 1];
				for (var dy = 0; dy < size; dy++)
					score += grid[x + size - 1, y + dy];

				score -= grid[x + size - 1, y + size - 1];
					
				if (score > max.level)
					max = (x, y, score, size);
			}
		}

		var (maxX, maxY, _, maxSize) = max;

		return $"{maxX},{maxY},{maxSize}";
	}
}