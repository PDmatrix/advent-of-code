using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._23;

[UsedImplicitly]
public class Year2023Day23 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		input =
        [
            "#.#####################",
			"#.......#########...###",
			"#######.#########.#.###",
			"###.....#.>.>.###.#.###",
			"###v#####.#v#.###.#.###",
			"###.>...#.#.#.....#...#",
			"###v###.#.#.#########.#",
			"###...#.#.#.......#...#",
			"#####.#.#.#######.#.###",
			"#.....#.#.#.......#...#",
			"#.#####.#.#.#########v#",
			"#.#...#...#...###...>.#",
			"#.#.#v#######v###.###v#",
			"#...#.>.#...>.>.#.###.#",
			"#####v#.#.###v#.#.###.#",
			"#.....#...#...#.#.#...#",
			"#.#########.###.#.#.###",
			"#...###...#...#...#.###",
			"###.###.#.###v#####v###",
			"#...#...#.#.>.>.#.>.###",
			"#.###.###.#.###.#.#v###",
			"#.....###...###...#...#",
			"#####################.#",
		];

		var grid = ParseInput(input);

		var start = new Point(1, 0);
		var end = new Point(grid.Keys.Max(p => p.X) - 1, grid.Keys.Max(p => p.Y));
		var queue = new Queue<(Point Position, int Time)>();

		PrintGrid(grid);
		
		return 1;
	}

	public object Part2(IEnumerable<string> input)
	{
		return 2;
	}

	private static void PrintGrid(Dictionary<Point, string> grid)
	{
		var maxX = grid.Keys.Max(p => p.X);
		var maxY = grid.Keys.Max(p => p.Y);

		for (var y = 0; y <= maxY; y++)
		{
			for (var x = 0; x <= maxX; x++)
			{
				Console.Write(grid[new Point(x, y)]);
			}

			Console.WriteLine();
		}
	}

	private static Dictionary<Point, string> ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, string>();
		var lines = input.ToList();
		for (var y = 0; y < lines.Count; y++)
		{
			var line = lines[y];
			for (var x = 0; x < line.Length; x++)
			{
				grid[new Point(x, y)] = line[x].ToString();
			}
		}

		return grid;
	}
}
