using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2015._25;

[UsedImplicitly]
public class Year2015Day25 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (row, column) = GetRowAndColumn(input);
		var rowColumnMax = Math.Max(row, column) * 2;
		var grid = new long[rowColumnMax, rowColumnMax];
		long code = -1;
		for (var i = 0; i < rowColumnMax; i++)
		{
			for (int x = 0, y = i ; y >= 0; x++, y--)
			{
				code = GetNextCode(code);
				grid[x, y] =  code;
			}
		}
		return grid[column - 1, row - 1].ToString();
	}

	private static (int, int) GetRowAndColumn(IEnumerable<string> input)
	{
		var splitted = input.First().Split(" ").ToArray();
		return (int.Parse(splitted[16].Trim(',')), int.Parse(splitted[18].Trim('.')));
	}

	private static long GetNextCode(long code)
	{
		if (code == -1)
			return 20151125;
			
		return code * 252533 % 33554393;
	}

	public object Part2(IEnumerable<string> input)
	{
		return "Congratulations!";
	}
}