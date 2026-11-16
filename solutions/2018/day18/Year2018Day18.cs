using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2018._18;

[UsedImplicitly]
public class Year2018Day18 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = input.Select(line => line.ToCharArray()).ToArray();
			
		for (var minute = 1; minute <= 10; minute++)
		{
			grid = TakeStep(grid);
		}

		var flatList = grid.SelectMany(x => x).Where(x => x != '.').ToList();
		return (flatList.Count(x => x == '#') * flatList.Count(x => x == '|')).ToString();
	}

	private char[][] TakeStep(char[][] grid)
	{
		var clonedGrid = grid.Select(x => x.ToArray()).ToArray();
		for (var y = 0; y < clonedGrid.Length; y++)
		{
			for (var x = 0; x < clonedGrid[0].Length; x++)
			{
				var adjacentAcres = GetAdjacentAcres(x, y, grid);
				clonedGrid[y][x] = grid[y][x] switch
				{
					'.' => adjacentAcres.Count(acr => acr == '|') >= 3 ? '|' : '.',
					'#' => adjacentAcres.Count(acr => acr == '#') >= 1 &&
					       adjacentAcres.Count(acr => acr == '|') >= 1
						? '#'
						: '.',
					'|' => adjacentAcres.Count(acr => acr == '#') >= 3 ? '#' : '|',
					_ => throw new Exception()
				};
			}
		}

		return clonedGrid.ToArray();
	}

	private char[] GetAdjacentAcres(int x, int y, char[][] grid)
	{
		var acres = new char[8];
		var idx = 0;
		for (var i = y - 1; i <= y + 1; i++)
		{
			for (var j = x - 1; j <= x + 1; j++)
			{
				if (i == y && j == x || i < 0 || j < 0)
					continue;
					
				if (i >= grid.Length || j >= grid[0].Length)
					continue;

				acres[idx] = grid[i][j];
				idx++;
			}
		}

		return acres;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = input.Select(line => line.ToCharArray()).ToArray();
		var states = new Dictionary<string, int>();
		var periodOffset = 0;
		var periodLength = 0;
		for (var minute = 1; minute <= 1000000000; minute++)
		{
			grid = TakeStep(grid);
			var state = string.Join(string.Empty, grid.Select(x => string.Join(string.Empty, x)));
			if (states.ContainsKey(state))
			{
				periodOffset = states[state];
				periodLength = minute - states[state];
				break;
			}

			states[state] = minute;
		}

		var obm = periodOffset + (1000000000 - periodOffset) % periodLength;
		var (key, _) = states.First(x => x.Value == obm);
			
		return (key.Count(x => x == '#') * key.Count(x => x == '|')).ToString();
	}
}