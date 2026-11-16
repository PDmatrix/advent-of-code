using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._24;

[UsedImplicitly]
public class Year2020Day24 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);

		return grid.Values.Count(x => x);
	}
	
	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);
		var blackTiles = grid.Where(x => x.Value).Select(x => x.Key).ToHashSet();
		
		for (var day = 0; day < 100; day++)
		{
			var newTiles = new HashSet<Hex>();
			var tilesToCheck = new HashSet<Hex>();

			foreach (var blackTile in blackTiles)
			{
				tilesToCheck.Add(blackTile);
				foreach (var (dx, dy, dz) in HexDiff)
					tilesToCheck.Add(new Hex(blackTile.X + dx, blackTile.Y + dy, blackTile.Z + dz));
			}

			foreach (var hex in tilesToCheck)
			{
				var adjacentCount = 0;
				foreach (var (dx, dy, dz) in HexDiff)
				{
					var newHex = new Hex(hex.X + dx, hex.Y + dy, hex.Z + dz);
					if (blackTiles.Contains(newHex))
						adjacentCount++;
				}

				if ((blackTiles.Contains(hex) && adjacentCount is > 0 and <= 2) ||
				    (!blackTiles.Contains(hex) && adjacentCount == 2))
					newTiles.Add(hex);
			}

			blackTiles = newTiles.ToHashSet();
		}

		return blackTiles.Count;
	}

	private record struct Hex(int X, int Y, int Z);
	
	private static Dictionary<Hex, bool> ParseGrid(IEnumerable<string> input)
	{
		var grid = new Dictionary<Hex, bool>();
		foreach (var line in input)
		{
			var position = new Hex(0, 0, 0);
			var index = 0;
			while (index < line.Length)
			{
				if (line[index] == 'e')
				{
					position = position with { X = position.X + 1, Y = position.Y - 1 };
					index++;
					continue;
				}

				if (line[index] == 'w')
				{
					position = position with { X = position.X - 1, Y = position.Y + 1 };
					index++;
					continue;
				}

				if (line[index] == 's' && line[index + 1] == 'e')
				{
					position = position with { Y = position.Y - 1, Z = position.Z + 1 };
					index += 2;
					continue;
				}

				if (line[index] == 's' && line[index + 1] == 'w')
				{
					position = position with { X = position.X - 1, Z = position.Z + 1 };
					index += 2;
					continue;
				}

				if (line[index] == 'n' && line[index + 1] == 'w')
				{
					position = position with { Y = position.Y + 1, Z = position.Z - 1 };
					index += 2;
					continue;
				}

				if (line[index] == 'n' && line[index + 1] == 'e')
				{
					position = position with { X = position.X + 1, Z = position.Z - 1 };
					index += 2;
					continue;
				}
			}

			grid[position] = !grid.GetValueOrDefault(position, false);
		}

		return grid;
	}

	private static readonly List<(int x, int y, int z)> HexDiff = new()
	{
		(0, 1, -1),
		(0, -1, 1),
		(-1, 0, 1),
		(1, 0, -1),
		(1, -1, 0),
		(-1, 1, 0),
	};
}