using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._4;

[UsedImplicitly]
public class Year2024Day04 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0), (-1, 1), (-1, -1), (1, -1), (1, 1) };
		var answer = 0;
		foreach (var key in grid.Keys)
		{
			var current = grid[key];
			if (current != 'X')
				continue;

			foreach (var dir in dirs)
			{
				var newDir = (key.Item1 + dir.Item1, key.Item2 + dir.Item2);
				if (grid.GetValueOrDefault(newDir, '\0') != 'M')
					continue;

				newDir = (newDir.Item1 + dir.Item1, newDir.Item2 + dir.Item2);
				if (grid.GetValueOrDefault(newDir, '\0') != 'A')
					continue;
				
				newDir = (newDir.Item1 + dir.Item1, newDir.Item2 + dir.Item2);
				if (grid.GetValueOrDefault(newDir, '\0') != 'S')
					continue;
				
				answer++;
			}
		}

		return answer;
	}

	private Dictionary<(int, int), char> ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<(int, int), char>();
		var y = 0;
		foreach (var line in input)
		{
			var x = 0;
			foreach (var c in line)
			{
				grid[(x, y)] = c;
				x++;
			}

			y++;
		}

		return grid;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		return grid.Keys.Count(key => IsMas(grid, key) && IsMasBack(grid, (key.Item1 + 2, key.Item2)));
	}

	private bool IsMas(Dictionary<(int, int), char> grid, (int, int) start)
	{
		var current = grid[start];
		if (current == 'M')
		{
			if (grid.GetValueOrDefault((start.Item1 + 1, start.Item2 + 1), '\0') == 'A' &&
			    grid.GetValueOrDefault((start.Item1 + 2, start.Item2 + 2), '\0') == 'S')
				return true;
		}
		
		if (current == 'S')
		{
			if (grid.GetValueOrDefault((start.Item1 + 1, start.Item2 + 1), '\0') == 'A' &&
			    grid.GetValueOrDefault((start.Item1 + 2, start.Item2 + 2), '\0') == 'M')
				return true;
		}

		return false;
	}
	
	private bool IsMasBack(Dictionary<(int, int), char> grid, (int, int) start)
	{
		var current = grid[start];
		if (current == 'M')
		{
			if (grid.GetValueOrDefault((start.Item1 - 1, start.Item2 + 1), '\0') == 'A' &&
			    grid.GetValueOrDefault((start.Item1 - 2, start.Item2 + 2), '\0') == 'S')
				return true;
		}
		
		if (current == 'S')
		{
			if (grid.GetValueOrDefault((start.Item1 - 1, start.Item2 + 1), '\0') == 'A' &&
			    grid.GetValueOrDefault((start.Item1 - 2, start.Item2 + 2), '\0') == 'M')
				return true;
		}

		return false;
	}
}