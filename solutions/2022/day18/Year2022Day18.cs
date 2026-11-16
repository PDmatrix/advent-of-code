using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._18;

[UsedImplicitly]
public partial class Year2022Day18 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var cubes = ParseInput(input);
		return CountTheSurfaceArea(cubes);
	}

	private static object CountTheSurfaceArea(HashSet<(int X, int Y, int Z)> cubes)
	{
		return cubes.Sum(cube => GetNeighbours(cube).Count(neighbour => !cubes.Contains(neighbour)));
	}

	public object Part2(IEnumerable<string> input)
	{
		var cubes = ParseInput(input);
		
		var minX = cubes.Min(x => x.X) - 1;
		var minY = cubes.Min(x => x.Y) - 1;
		var minZ = cubes.Min(x => x.Z) - 1;
		var maxX = cubes.Max(x => x.X) + 1;
		var maxY = cubes.Max(x => x.Y) + 1;
		var maxZ = cubes.Max(x => x.Z) + 1;
		
		var waterPoints = new HashSet<(int X, int Y, int Z)>();
		var q = new Queue<(int X, int Y, int Z)>();
		q.Enqueue((minX, minY, minZ));
		while (q.Count != 0)
		{
			var (x, y, z) = q.Dequeue();
			if (!waterPoints.Add((x, y, z)))
				continue;

			foreach (var neighbour in GetNeighbours((x, y, z)))
			{
				if (minX > neighbour.X || neighbour.X > maxX || minY > neighbour.Y || neighbour.Y > maxY ||
				    minZ > neighbour.Z || neighbour.Z > maxZ) continue;
				
				if (!cubes.Contains(neighbour))
					q.Enqueue(neighbour);
			}
		}
		
		var lavaPoints = new HashSet<(int X, int Y, int Z)>();
		for (var x = minX; x <= maxX; x++)
		{
			for (var y = minY; y <= maxY; y++)
			{
				for (var z = minZ; z <= maxZ; z++)
				{
					if (!waterPoints.Contains((x, y, z)))
						lavaPoints.Add((x, y, z));
				}
			}
		}

		return CountTheSurfaceArea(lavaPoints);
	}

	private static HashSet<(int X, int Y, int Z)> GetNeighbours((int X, int Y, int Z) cube)
	{
		var (x, y, z) = cube;
		return new HashSet<(int X, int Y, int Z)>
		{
			(x - 1, y, z),
			(x + 1, y, z),
			(x, y - 1, z),
			(x, y + 1, z),
			(x, y, z - 1),
			(x, y, z + 1),
		};
	}

	private static HashSet<(int X, int Y, int Z)> ParseInput(IEnumerable<string> input)
	{
		return input.Select(line => line.Split(",")).Select(split => (int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]))).ToHashSet();
	}
}