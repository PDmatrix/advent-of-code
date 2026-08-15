using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._23;

[UsedImplicitly]
public class Year2023Day23 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);

		var start = new Point(1, 0);
		var end = new Point(grid.Keys.Max(p => p.X) - 1, grid.Keys.Max(p => p.Y));

		var visited = new HashSet<Point>();
		return LongestPath(grid, start, end, visited);
	}

	public object Part2(IEnumerable<string> input)
	{
		// input =
        // [
		// 	"#.#####################",
		// 	"#.......#########...###",
		// 	"#######.#########.#.###",
		// 	"###.....#.>.>.###.#.###",
		// 	"###v#####.#v#.###.#.###",
		// 	"###.>...#.#.#.....#...#",
		// 	"###v###.#.#.#########.#",
		// 	"###...#.#.#.......#...#",
		// 	"#####.#.#.#######.#.###",
		// 	"#.....#.#.#.......#...#",
		// 	"#.#####.#.#.#########v#",
		// 	"#.#...#...#...###...>.#",
		// 	"#.#.#v#######v###.###v#",
		// 	"#...#.>.#...>.>.#.###.#",
		// 	"#####v#.#.###v#.#.###.#",
		// 	"#.....#...#...#.#.#...#",
		// 	"#.#########.###.#.#.###",
		// 	"#...###...#...#...#.###",
		// 	"###.###.#.###v#####v###",
		// 	"#...#...#.#.>.>.#.>.###",
		// 	"#.###.###.#.###.#.#v###",
		// 	"#.....###...###...#...#",
		// 	"#####################.#",
		// ];

		var grid = ParseInput(input);

		var start = new Point(1, 0);
		var end = new Point(grid.Keys.Max(p => p.X) - 1, grid.Keys.Max(p => p.Y));

		var graph = BuildGraph(grid);
		var visited = new HashSet<Point>();
		return LongestPathStraight(graph, start, end, visited);
	}

	private static int LongestPath(Dictionary<Point, string> grid, Point start, Point end, HashSet<Point> visited)
	{
		var (p, t) = PathToIntersection(grid, start, end, visited);

		// dead end
		if (t == -1)
			return 0;

		if (p == end)
			return t;

		var adj = GetAdjacentNotVisited(grid, p, visited);

		var gridDir = new Dictionary<string, Point>
		{
			[">"] = new Point(1, 0),
			["v"] = new Point(0, 1),
			["^"] = new Point(0, -1),
			["<"] = new Point(-1, 0),
		};

		var l = adj.First();
		var ldir = gridDir[grid[l]];
		l = l with { X = l.X + ldir.X, Y = l.Y + ldir.Y};

		var r = adj.Last();
		var rdir = gridDir[grid[r]];
		r = r with { X = r.X + rdir.X, Y = r.Y + rdir.Y};

		return t + 1 + Math.Max(LongestPath(grid, l, end, [adj.First(), ..visited]), LongestPath(grid, r, end, [adj.Last(), ..visited]));
	}

	private static Dictionary<Point, List<(Point Destination, int Distance)>> BuildGraph(Dictionary<Point, string> grid)
	{
		var nodes = new HashSet<Point>();

		foreach (var (point, value) in grid)
		{
			if (value == "#")
				continue;

			var adjacent = GetAdjacent(grid, point);

			if (adjacent.Count != 2)
				nodes.Add(point);
		}

		var graph = new Dictionary<Point, List<(Point Destination, int Distance)>>();

		foreach (var node in nodes)
			graph[node] = [];

		foreach (var node in nodes)
		{
			foreach (var neighbour in GetAdjacent(grid, node))
			{
				var previous = node;
				var current = neighbour;
				var distance = 1;

				while (!nodes.Contains(current))
				{
					var adjacent = GetAdjacent(grid, current);

					var next = adjacent[0] == previous
						? adjacent[1]
						: adjacent[0];

					previous = current;
					current = next;
					distance++;
				}

				graph[node].Add((current, distance));
			}
		}

		return graph;
	}

	private static int LongestPathStraight(
		Dictionary<Point, List<(Point Destination, int Distance)>> graph,
		Point current,
		Point end,
		HashSet<Point> visited)
	{
		if (current == end)
			return 0;

		visited.Add(current);

		var max = -1;

		foreach (var (destination, distance) in graph[current])
		{
			if (visited.Contains(destination))
				continue;

			var remaining = LongestPathStraight(
				graph,
				destination,
				end,
				visited);

			// This branch doesn't reach the end.
			if (remaining == -1)
				continue;

			max = Math.Max(max, distance + remaining);
		}

		visited.Remove(current);

		return max;
	}

	private static (Point, int) PathToIntersection(Dictionary<Point, string> grid, Point start, Point end, HashSet<Point> visited)
	{
		var time = 0;
		var current = start;
		var adj = GetAdjacentNotVisited(grid, current, visited);

		while (adj.Count() == 1)
		{
			visited.Add(current);
			current = adj.First();
			time++;
			adj = GetAdjacentNotVisited(grid, current, visited);
		}

		if (current == end)
			return (current, time);

		// dead end
		if (adj.Count() == 0)
			return (new Point(), -1);
		
		return (current, time);
	}

	private static List<Point> GetAdjacent(Dictionary<Point, string> m, Point p)
    {
        var adjacent = new List<Point>();
        var diff = new[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
        foreach (var (dx, dy) in diff)
        {
            var newPos = new Point(p.X + dx, p.Y + dy);

            if (m.TryGetValue(newPos, out string value) && value != "#")
                adjacent.Add(newPos);
        }

        return adjacent;
    }


	private static List<Point> GetAdjacentNotVisited(Dictionary<Point, string> m, Point p, HashSet<Point> visited)
    {
        var adjacent = new List<Point>();
        var diff = new[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
        foreach (var (dx, dy) in diff)
        {
            var newPos = new Point(p.X + dx, p.Y + dy);
			if (visited.Contains(newPos))
				continue;

            if (m.TryGetValue(newPos, out string value) && value != "#")
                adjacent.Add(newPos);
        }

        return adjacent;
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
