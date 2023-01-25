using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._17;

[UsedImplicitly]
public class Year2020Day17 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);

		for(var cycle = 0; cycle < 6; cycle++)
		{
			var newGrid = grid.ToDictionary(x => x.Key, x => x.Value);
			foreach (var cube in grid)
			{
				var neighbors = GetNeighbors(cube.Key, grid);

				foreach (var neighbor in neighbors)
				{
					if (grid.ContainsKey(neighbor.Key))
						continue;

					var outerNeighbors = GetNeighbors(neighbor.Key, grid)
						.Where(x => grid.ContainsKey(x.Key));

					if (outerNeighbors.Count(x => x.Value) == 3)
						newGrid[neighbor.Key] = true;

				}

				if (cube.Value && (neighbors.Count(x => x.Value) == 2 || neighbors.Count(x => x.Value) == 3))
					newGrid[cube.Key] = cube.Value;
				else if (cube.Value == false && neighbors.Count(x => x.Value) == 3)
					newGrid[cube.Key] = true;
				else
					newGrid[cube.Key] = false;
				
			}

			grid = newGrid.ToDictionary(x => x.Key, x => x.Value);
		}

		return grid.Count(x => x.Value);
	}
	
	private static Dictionary<(int x, int y, int z), bool> GetNeighbors((int x, int y, int z) target, Dictionary<(int x, int y, int z), bool> grid)
	{
		var possibleNeighbors = new List<(int x, int y, int z)>();

		for (var x = -1 + target.x; x <= 1 + target.x; x++)
		for (var y = -1 + target.y; y <= 1 + target.y; y++)
		for (var z = -1 + target.z; z <= 1 + target.z; z++)
			if (x == target.x && y == target.y && z == target.z)
				continue;
			else
				possibleNeighbors.Add((x, y, z));


		var neighbors = new Dictionary<(int x, int y, int z), bool>();
		foreach (var neighbor in possibleNeighbors)
		{
			grid.TryGetValue(neighbor, out var state);
			neighbors[neighbor] = state;
		}
		return neighbors;
	}
	
	private static Dictionary<(int x, int y, int z, int w), bool> GetNeighbors((int x, int y, int z, int w) target, Dictionary<(int x, int y, int z, int w), bool> grid)
	{
		var possibleNeighbors = new List<(int x, int y, int z, int w)>();

		for (var x = -1 + target.x; x <= 1 + target.x; x++)
		for (var y = -1 + target.y; y <= 1 + target.y; y++)
		for (var z = -1 + target.z; z <= 1 + target.z; z++)
		for (var w = -1 + target.w; w <= 1 + target.w; w++)
			if (x == target.x && y == target.y && z == target.z && w == target.w)
				continue;
			else
				possibleNeighbors.Add((x, y, z, w));


		var neighbors = new Dictionary<(int x, int y, int z, int w), bool>();
		foreach (var neighbor in possibleNeighbors)
		{
			grid.TryGetValue(neighbor, out var state);
			neighbors[neighbor] = state;
		}
		return neighbors;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseGridPart2(input);
		for(var cycle = 0; cycle < 6; cycle++)
		{
			var newGrid = grid.ToDictionary(x => x.Key, x => x.Value);
			foreach (var cube in grid)
			{
				var neighbors = GetNeighbors(cube.Key, grid);

				foreach (var neighbor in neighbors)
				{
					if (grid.ContainsKey(neighbor.Key))
						continue;

					var outerNeighbors = GetNeighbors(neighbor.Key, grid)
						.Where(x => grid.ContainsKey(x.Key));

					if (outerNeighbors.Count(x => x.Value) == 3)
						newGrid[neighbor.Key] = true;

				}

				if (cube.Value && (neighbors.Count(x => x.Value) == 2 || neighbors.Count(x => x.Value) == 3))
					newGrid[cube.Key] = cube.Value;
				else if (cube.Value == false && neighbors.Count(x => x.Value) == 3)
					newGrid[cube.Key] = true;
				else
					newGrid[cube.Key] = false;
				
			}

			grid = newGrid.ToDictionary(x => x.Key, x => x.Value);
		}

		return grid.Count(x => x.Value);
	}
	
	private static Dictionary<(int x, int y, int z), bool> ParseGrid(IEnumerable<string> input)
	{
		var grid = new Dictionary<(int x, int y, int z), bool>();
		var enumerable = input as string[] ?? input.ToArray();
		for (var y = 0; y < enumerable.Length; y++)
		{
			for (var x = 0; x < enumerable[y].Length; x++)
			{
				grid.Add((x, y, 0), enumerable[y][x] == '#');
			}
		}

		return grid;
	}
	
	private static Dictionary<(int x, int y, int z, int w), bool> ParseGridPart2(IEnumerable<string> input)
	{
		var grid = new Dictionary<(int x, int y, int z, int w), bool>();
		var enumerable = input as string[] ?? input.ToArray();
		for (var y = 0; y < enumerable.Length; y++)
		{
			for (var x = 0; x < enumerable[y].Length; x++)
			{
				grid.Add((x, y, 0, 0), enumerable[y][x] == '#');
			}
		}

		return grid;
	}
}