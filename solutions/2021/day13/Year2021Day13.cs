using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._13;

[UsedImplicitly]
public class Year2021Day13 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (grid, folds) = ParseInput(input);

		grid = Fold(folds, grid, true);

		return grid.Count(x => x.Value);
	}

	public object Part2(IEnumerable<string> input)
	{
		var (grid, folds) = ParseInput(input);

		grid = Fold(folds, grid);
		
		return GetScreen(grid);
	}

	private static Dictionary<Point, bool> Fold(List<(string along, int number)> folds, Dictionary<Point, bool> grid, bool oneTime = false)
	{
		foreach (var (along, number) in folds)
		{
			var minX = grid.Min(x => x.Key.X);
			var maxX = grid.Max(x => x.Key.X);
			var minY = grid.Min(x => x.Key.Y);
			var maxY = grid.Max(x => x.Key.Y);

			var newGrid = new Dictionary<Point, bool>();
			if (along == "x")
			{
				for (var y = minY; y <= maxY; y++)
				{
					for (var x = minX; x <= maxX; x++)
					{
						if (x == number)
							continue;

						if (x > number)
							newGrid[new Point(maxX - x, y)] = grid.GetValueOrDefault(new Point(x, y), false) ||
							                                  grid.GetValueOrDefault(new Point(maxX - x, y), false);
						else
							newGrid[new Point(x, y)] = grid.GetValueOrDefault(new Point(x, y), false);
					}
				}
			}
			else
			{
				for (var y = minY; y <= maxY; y++)
				{
					if (y == number)
						continue;

					for (var x = minX; x <= maxX; x++)
					{
						if (y > number)
							newGrid[new Point(x, maxY - y)] = grid.GetValueOrDefault(new Point(x, y), false) ||
							                                  grid.GetValueOrDefault(new Point(x, maxY - y), false);
						else
							newGrid[new Point(x, y)] = grid.GetValueOrDefault(new Point(x, y), false);
					}
				}
			}

			grid = newGrid;
			if (oneTime)
				break;
		}

		return grid;
	}

	private static (Dictionary<Point, bool>, List<(string along, int number)>) ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, bool>();
		var folds = new List<(string along, int number)>();
		var state = 0;
		foreach (var line in input)
		{
			if (string.IsNullOrEmpty(line))
			{
				state++;
				continue;
			}

			if (state == 0)
			{
				var splitted = line.Split(',');
				grid[new Point(int.Parse(splitted[0]), int.Parse(splitted[1]))] = true;
				continue;
			}

			var splittedFold = line.Split("=");
			folds.Add((splittedFold[0].Last().ToString(), int.Parse(splittedFold[1])));
		}

		return (grid, folds);
	}
	
	private static string GetScreen(Dictionary<Point, bool> grid)
	{
		var minX = grid.Min(x => x.Key.X);
		var maxX = grid.Max(x => x.Key.X);
		var minY = grid.Min(x => x.Key.Y);
		var maxY = grid.Max(x => x.Key.Y);
		var sb = new StringBuilder();
		sb.AppendLine();
		for (int y = minY; y <= maxY; y++)
		{
			for (int x = minX; x <= maxX; x++)
			{
				var tile = grid.GetValueOrDefault(new Point(x, y), false);
				sb.Append(tile ? '#' : '.');
			}

			sb.AppendLine();
		}

		return sb.ToString();
	}
}