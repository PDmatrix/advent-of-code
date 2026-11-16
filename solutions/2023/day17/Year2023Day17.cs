using System.Collections.Generic;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._17;

[UsedImplicitly]
public class Year2023Day17 : ISolution
{
	// cudos to https://observablehq.com/@jwolondon/advent-of-code-2023-day-17
	public object Part1(IEnumerable<string> input)
	{
		return MinHeatLoss(ParseInput(input));
	}

	public object Part2(IEnumerable<string> input)
	{
		return MinHeatLoss(ParseInput(input), 10, 4);
	}
	
	private static int MinHeatLoss(int[][] grid, int maxSteps = 3, int minSteps = 1)
	{
		var (nRows, nCols) = (grid.Length, grid[0].Length);
		var visited = new HashSet<string>();
		var queue = new PriorityQueue<(int high, int row, int col, string dir), int>();

		queue.Enqueue((0, 0, 0, "EW"), 0);
		queue.Enqueue((0, 0, 0, "NS"), 0);

		while (queue.Count > 0)
		{
			var (heatLoss, row, col, dir) = queue.Dequeue();
			if (row == nRows - 1 && col == nCols - 1)
				return heatLoss;
			
			var state = Hash(row, col, dir);
			if (visited.Add(state))
				AddMoves(heatLoss, row, col, dir);
		}
		
		return 0;

		void AddMoves(int high, int row, int col, string dir)
		{
			foreach (var (dRow, dCol) in DirectionOffsets(dir))
			{
				var hl2 = high;
				for (var i = 1; i <= maxSteps; i++)
				{
					var (r, c) = (row + i * dRow, col + i * dCol);
					if (r >= 0 && r < nRows && c >= 0 && c < nCols)
					{
						hl2 += grid[r][c];
						if (i >= minSteps)
						{
							queue.Enqueue((hl2, r, c, dir == "EW" ? "NS" : "EW"), hl2);
						}
					}
				}
			}
		}

		string Hash(int row, int col, string dir) => $"{row},{col},{dir}";
	}
	
	private static List<(int row, int col)> DirectionOffsets(string dir)
	{
		switch (dir)
		{
			case "EW":
				return
				[
					(0, 1),
					(0, -1)
				];
			case "NS":
				return
				[
					(1, 0),
					(-1, 0)
				];
			default:
				return [];
		}
	}
	
	private static int[][] ParseInput(IEnumerable<string> input)
	{
		var lines = new List<int[]>();
		foreach (var line in input)
		{
			var row = new int[line.Length];
			for (var i = 0; i < line.Length; i++)
			{
				row[i] = int.Parse(line[i].ToString());
			}
			lines.Add(row);
		}

		return lines.ToArray();
	}
}