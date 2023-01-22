using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2020._11;

[UsedImplicitly]
public class Year2020Day11 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);
		var layouts = new HashSet<int>();
		while (true)
		{
			var newGrid = grid.ToDictionary(x => x.Key, x => x.Value);
			foreach (var kv in grid)
			{
				if (kv.Value == State.Floor)
					continue;
				
				var adjacentCount = GetAdjacentCount(grid, kv.Key);
				if (kv.Value == State.Occupied && adjacentCount >= 4)
					newGrid[kv.Key] = State.Empty;
				else if (kv.Value == State.Empty && adjacentCount == 0)
					newGrid[kv.Key] = State.Occupied;
			}

			grid = newGrid.ToDictionary(x => x.Key, x => x.Value);
			
			var hash = GetHash(grid);
			if (layouts.Contains(hash))
				break;

			layouts.Add(hash);
		}
		
		
		return grid.Count(x => x.Value == State.Occupied);
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);
		var layouts = new HashSet<int>();
		while (true)
		{
			var newGrid = grid.ToDictionary(x => x.Key, x => x.Value);
			foreach (var kv in grid)
			{
				if (kv.Value == State.Floor)
					continue;
				
				var adjacentCount = GetAdjacentCountPart2(grid, kv.Key);
				if (kv.Value == State.Occupied && adjacentCount >= 5)
					newGrid[kv.Key] = State.Empty;
				else if (kv.Value == State.Empty && adjacentCount == 0)
					newGrid[kv.Key] = State.Occupied;
			}

			grid = newGrid.ToDictionary(x => x.Key, x => x.Value);
			
			var hash = GetHash(grid);
			if (layouts.Contains(hash))
				break;

			layouts.Add(hash);
		}
		
		
		return grid.Count(x => x.Value == State.Occupied);
	}
	
	private static int GetAdjacentCountPart2(Dictionary<Position, State> grid, Position position)
	{
		var adjacentCount = 0;

		var diff = new List<(int x, int y)>
		{
			(0, 1), (0, -1), (1, 0), (-1, 0),
			(1, 1), (1, -1), (-1, 1), (-1, -1)
		};

		foreach (var (dx, dy) in diff)
		{
			var newPos = new Position(position.X + dx, position.Y + dy);
			while (true)
			{
				if (!grid.ContainsKey(newPos))
					break;

				if (grid[newPos] == State.Empty)
					break;
				
				if (grid[newPos] == State.Occupied)
				{
					adjacentCount++;
					break;
				}

				newPos = new Position(newPos.X + dx, newPos.Y + dy);
			}
		}

		return adjacentCount;
	}

	private static int GetAdjacentCount(Dictionary<Position, State> grid, Position position)
	{
		var adjacentCount = 0;

		var diff = new List<(int x, int y)>
		{
			(0, 1), (0, -1), (1, 0), (-1, 0),
			(1, 1), (1, -1), (-1, 1), (-1, -1)
		};

		foreach (var (dx, dy) in diff)
		{
			var newPos = new Position(position.X + dx, position.Y + dy);
			if (!grid.ContainsKey(newPos))
				continue;

			if (grid[newPos] == State.Occupied)
				adjacentCount++;
		}

		return adjacentCount;
	}
	
	private static int GetHash(Dictionary<Position, State> grid)
	{
		unchecked
		{
			var hash = 19;
			foreach (var kv in grid)
				hash = hash * 31 + kv.Key.GetHashCode() + (int)kv.Value;

			return hash;
		}
	}


	private Dictionary<Position, State> ParseGrid(IEnumerable<string> input)
	{
		var grid = new Dictionary<Position, State>();
		var enumerable = input as string[] ?? input.ToArray();
		for (var y = 0; y < enumerable.Length; y++)
		{
			for (var x = 0; x < enumerable[y].Length; x++)
			{
				var state = enumerable[y][x] switch
				{
					'.' => State.Floor,
					'#' => State.Occupied,
					'L' => State.Empty,
					_ => throw new Exception("Invalid input")
				};
				
				grid.Add(new Position(x, y), state);
			}
		}

		return grid;
	}
	
	private enum State
	{
		Floor,
		Empty,
		Occupied
	}
}