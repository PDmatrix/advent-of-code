using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._20;

[UsedImplicitly]
public class Year2020Day20 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var tiles = ParseInput(input);

		return tiles.Where(x => x.Value.Neighbors.Count == 2).Aggregate<KeyValuePair<int, Tile>, long>(1, (acc, tile) => tile.Key * acc);
	}
	
	public object Part2(IEnumerable<string> input)
	{
		var tiles = ParseInput(input);

		var corners = tiles.Where(x => x.Value.Neighbors.Count == 2).ToList();

		var (r, c) = (0, 0);
		var gridMap = new Dictionary<(int r, int c), int>();
		var idn = 0;
		while (tiles.Count != gridMap.Count)
		{
			if (r == 0 && c == 0)
			{
				idn = corners.First().Key;
				var tile = tiles[idn].Grid;
				var neighbours = tiles[idn].Neighbors.Values.ToList();
				neighbours.AddRange(neighbours.Select(Reverse).ToList());

				while (true)
				{
					var (bottom, right) = (GetSide(tile, "bottom"), GetSide(tile, "right"));
					if (neighbours.Contains(bottom) && neighbours.Contains(right))
						break;
					tile = Rotate(tile);
				}

				gridMap[(r, c)] = idn;
				tiles[idn] = tiles[idn] with { Grid = tile };
				c++;
			} else if (c == 0)
			{
				var pIdn = gridMap[(r - 1, c)];
				var pTile = tiles[pIdn].Grid;
				var sideOptions = new List<string>
				{
					GetSide(pTile, "bottom"),
					Reverse(GetSide(pTile, "bottom"))
				};
				idn = tiles[pIdn].Neighbors.First(x => sideOptions.Contains(x.Value)).Key;
				var tile = tiles[idn].Grid;
				var nSide = GetSide(tiles[pIdn].Grid, "bottom");
				for (var i = 0; i < 8; i++)
				{
					if (i == 4)
						tile = Flip(tile);
					if (GetSide(tile, "top") == Reverse(nSide))
						break;
					tile = Rotate(tile);
				}

				gridMap[(r, c)] = idn;
				tiles[idn] = tiles[idn] with { Grid = tile };
				c++;
			}
			else
			{
				var pIdn = idn;
				var pTile = tiles[pIdn].Grid;
				var sideOptions = new List<string>
				{
					GetSide(pTile, "right"),
					Reverse(GetSide(pTile, "right"))
				};
				var idns = tiles[pIdn].Neighbors.Where(x => sideOptions.Contains(x.Value)).Select(x => x.Key).ToList();
				if (idns.Count == 1)
				{
					idn = idns.First();
					var tile = tiles[idn].Grid;
					var nSide = GetSide(tiles[pIdn].Grid, "right");
					for (var i = 0; i < 8; i++)
					{
						if (i == 4)
							tile = Flip(tile);
						if (GetSide(tile, "left") == Reverse(nSide))
							break;
						tile = Rotate(tile);
					}

					gridMap[(r, c)] = idn;
					tiles[idn] = tiles[idn] with { Grid = tile };
					c++;
				}
				else
				{
					r++;
					c = 0;
				}
			}
		}

		var fullMap = gridMap.Select(x => tiles[x.Value].Grid).ToList();

		var mapWithoutBorders = fullMap.Select(RemoveBorders).ToList();

		var width = mapWithoutBorders.First().Length;
		var squareRoot = Convert.ToInt32(Math.Sqrt(mapWithoutBorders.Count));
		var temp = new List<string>();
		for (var i = 0; i < squareRoot; i++)
		{
			for (var j = 0; j < width; j++)
			{
				var sbt = new StringBuilder();
				for (var k = i * squareRoot; k < i * squareRoot + squareRoot; k++)
					sbt.Append(mapWithoutBorders[k][j]);
				temp.Add(sbt.ToString());
			}
		}

		var finalGrid = temp.ToArray();
		
		var monster = new List<(int dx, int dy)>
		{
			(0, 1), (1, 2), (4, 2), (5, 1), (6, 1), (7, 2), (10, 2), (11, 1),
			(12, 1), (13, 2), (16, 2), (17, 1), (18, 0), (18, 1), (19, 1)
		};
		var mon = new HashSet<(int dx, int dy)>();
		for (var i = 0; i < 8; i++)
		{
			if (i == 4)
				finalGrid = Flip(finalGrid);

			for (var j = 0; j < finalGrid.Length - 2; j++)
			{
				for (var k = 0; k < finalGrid[j].Length - 20; k++)
				{
					if (!monster.TrueForAll(x => finalGrid[j + x.dy][k + x.dx] == '#')) continue;
					foreach (var (dx, dy) in monster)
						mon.Add((j + dy, k + dx));
				}
			}

			finalGrid = Rotate(finalGrid);
		}


		return finalGrid.Sum(x => x.Count(c => c == '#')) - mon.Count;
	}

	private static Dictionary<int, Tile> ParseInput(IEnumerable<string> input)
	{
		var sb = new StringBuilder();
		var currentTile = 0;
		var tiles = new Dictionary<int, Tile>();
		foreach (var line in input)
		{
			if (string.IsNullOrEmpty(line))
			{
				var grid = sb.ToString().Split("\n")[..^1];
				var sides = new List<string>
				{
					grid[0],
					grid[^1],
					string.Join(string.Empty, grid.Select(x => x[0])),
					string.Join(string.Empty, grid.Select(x => x[^1])),
				};

				sides.AddRange(sides.Select(Reverse).ToList());

				tiles[currentTile] = new Tile(grid, sides, new Dictionary<int, string>());

				foreach (var (i, tile) in tiles)
				{
					if (i == currentTile)
						continue;

					var shared = tile.Sides.Where(x => sides.Contains(x)).ToList();
					foreach (var s in shared)
					{
						tiles[currentTile].Neighbors[i] = s;
						tiles[i].Neighbors[currentTile] = s;
					}
				}

				sb.Clear();
				continue;
			}

			if (line.StartsWith("Tile"))
			{
				currentTile = int.Parse(string.Join(string.Empty, line.Split().Last().Take(4)));
				continue;
			}

			sb.AppendLine(line);
		}

		return tiles;
	}

	private record struct Tile(string[] Grid, List<string> Sides, Dictionary<int, string> Neighbors);
	
	private static string Reverse(string s)
	{
		var charArray = s.ToCharArray();
		Array.Reverse(charArray);
		return new string(charArray);
	}

	private static string[] Rotate(string[] grid)
	{
		var rowLenght = grid.First().Length;
		var colLenght = grid.Length;

		var newGrid = new List<List<char>>();
		for (var r = 0; r < rowLenght; r++)
		{
			var list = new List<char>();
			for (var c = 0; c < colLenght; c++)
				list.Add('x');
			
			newGrid.Add(list);
		}

		
		for (var r = 0; r < rowLenght; r++)
		{
			for (var c = 0; c < colLenght; c++)
			{
				newGrid[r][c] = grid[colLenght - c - 1][r];
			}
		}

		return newGrid.Select(x => string.Join(string.Empty, x)).ToArray();
	}
	
	private static string[] Flip(string[] grid)
	{
		return grid.Reverse().ToArray();
	}

	private static string GetSide(string[] grid, string side)
	{
		return side switch
		{
			"top" => grid[0],
			"bottom" => Reverse(grid[^1]),
			"left" => string.Join(string.Empty, grid.Select(x => x[0])),
			"right" => Reverse(string.Join(string.Empty, grid.Select(x => x[^1]))),
		};
	}
	
	private static string[] RemoveBorders(string[] grid)
	{
		var len = grid[0].Length;
		var newGrid = new List<string>();
		for (var i = 1; i < grid.Length - 1; i++)
		{
			newGrid.Add(grid[i][1..(len - 1)]);
		}

		return newGrid.ToArray();
	}
}