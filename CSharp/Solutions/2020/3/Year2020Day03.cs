using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2020._3;

[UsedImplicitly]
public class Year2020Day03 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);
		
		return GetTreeCount(grid, 3, 1);
	}

	private static int GetTreeCount(Dictionary<Position, bool> grid, int right, int down)
	{
		var maxY = grid.Keys.Max(x => x.Y);
		var maxX = grid.Keys.Max(x => x.X);
		var x = 0;
		var y = 0;
		var treeCount = 0;
		while (y < maxY)
		{
			x = (x + right) % (maxX + 1);
			y += down;
			var newPos = new Position(x, y);
			if (grid.GetValueOrDefault(newPos, false))
				treeCount++;
		}

		return treeCount;
	}


	private static Dictionary<Position, bool> ParseGrid(IEnumerable<string> input)
	{
		var grid = new Dictionary<Position, bool>();
		var enumerable = input as string[] ?? input.ToArray();
		for (int y = 0; y < enumerable.Length; y++)
		{
			for (int x = 0; x < enumerable[y].Length; x++)
			{
				grid.Add(new Position(x, y), enumerable[y][x] == '#');
			}
		}

		return grid;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);
		var list = new List<(int right, int down)>
		{
			(1, 1),
			(3, 1),
			(5, 1),
			(7, 1),
			(1, 2),
		};

		long answer = 1;
		foreach (var (right, down) in list)
			answer *= GetTreeCount(grid, right, down);

		return answer;
	}
}