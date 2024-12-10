using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._6;

[UsedImplicitly]
public class Year2024Day06 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);
		var guard = grid.Single(x => new[]{'v', '^', '>', '<'}.Contains(x.Value)).Key;
		var (visited, _) = Walk(grid, guard);

		return visited.Count;
	}

	private static (HashSet<(Point, char)>, bool) Walk(Dictionary<Point, char> grid, Point guard)
	{
		var maxX = grid.Keys.Max(x => x.X);
		var maxY = grid.Keys.Max(x => x.Y);

		var posToDir = new Dictionary<char, (int, int)>
		{
			{ '>', (1, 0) },
			{ 'v', (0, 1) },
			{ '^', (0, -1) },
			{ '<', (-1, 0) },
		};
		var newDirs = new Dictionary<char, char>
		{
			{ '>', 'v' },
			{ 'v', '<' },
			{ '^', '>' },
			{ '<', '^' },
		};

		var visited = new HashSet<(Point, char)> { (guard, grid[guard]) };
		while (guard.X > 0 && guard.X < maxX && guard.Y > 0 && guard.Y < maxY)
		{
			var dir = posToDir[grid[guard]];
			var newGuard = new Point(guard.X + dir.Item1, guard.Y + dir.Item2);
			if (!grid.ContainsKey(newGuard))
				break;

			if (grid[newGuard] == '#')
			{
				grid[guard] = newDirs[grid[guard]];
				continue;
			}
			
			if (visited.Contains((newGuard, grid[guard])))
			{
				return (visited, true);
			}
			
			grid[newGuard] = grid[guard];
			grid[guard] = '.';
			guard = newGuard;
			
			visited.Add((newGuard, grid[newGuard]));
		}

		return (visited, false);
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);
		var guard = grid.Single(x => new[]{'v', '^', '>', '<'}.Contains(x.Value)).Key;
		var (visitedBefore, _) = Walk(new Dictionary<Point, char>(grid), guard);
		
		var looped = new HashSet<Point>();
		foreach (var c in visitedBefore)
		{
			var newGrid = new Dictionary<Point, char>(grid);
			if (newGrid[c.Item1] != '.')
				continue;

			newGrid[c.Item1] = '#';

			if (Walk(newGrid, guard).Item2)
				looped.Add(c.Item1);
		}

		return looped.Count;
	}
	
	private static Dictionary<Point, char> ParseGrid(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, char>();
		var enumerable = input as string[] ?? input.ToArray();
		for (var y = 0; y < enumerable.Length; y++)
		{
			for (var x = 0; x < enumerable[y].Length; x++)
			{
				grid.Add(new Point(x, y), enumerable[y][x]);
			}
		}

		return grid;
	}
}