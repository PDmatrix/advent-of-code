using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2021._15;

[UsedImplicitly]
public class Year2021Day15 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var graphList = input.Select(line => line.Select(num => int.Parse(num.ToString())).ToList()).ToList();

		var graph = new int[graphList.Count, graphList.First().Count];
		for (var y = 0; y < graphList.Count; y++)
		for (var x = 0; x < graphList.First().Count; x++)
			graph[y, x] = graphList[y][x];

		return GetPathCostV2(graph);
	}

	public object Part2(IEnumerable<string> input)
	{
		var graph = input.Select(line => line.Select(num => int.Parse(num.ToString())).ToList()).ToList();

		var extendedGraph = ExtendGraph(graph);

		return GetPathCostV2(extendedGraph);
	}

	private static int GetPathCostV2(int[,] graph)
	{
		var priorityQueue = new PriorityQueue<(int x, int y), int>();
		
		var costs = new int[graph.GetLength(1), graph.GetLength(1)];
		for (var y = 0; y < graph.GetLength(1); y++)
		for (var x = 0; x < graph.GetLength(1); x++)
			costs[y, x] = int.MaxValue;

		costs[0, 0] = 0;
		priorityQueue.Enqueue((0, 0), 0);
		var visited = new HashSet<(int x, int y)>();
		while (priorityQueue.Count != 0)
		{
			var current = priorityQueue.Dequeue();
			var adjacents = GetAdjacent(graph, current);
			foreach (var adjacent in adjacents)
			{
				if (visited.Contains(adjacent))
					continue;

				var newCost = costs[current.y, current.x] + graph[adjacent.y, adjacent.x];
				var oldCost = costs[adjacent.y, adjacent.x];

				if (newCost < oldCost)
				{
					costs[adjacent.y, adjacent.x] = newCost;
					priorityQueue.Enqueue((adjacent.x, adjacent.y), newCost);
				}
			}

			visited.Add(current);
			if (current == (graph.GetLength(1) - 1, graph.GetLength(1) - 1))
				break;
		}

		return costs[graph.GetLength(1) - 1, graph.GetLength(1) - 1];
	}

	private static IEnumerable<(int x, int y)> GetAdjacent(int[,] graph, (int x, int y) node)
	{
		var diff = new List<(int x, int y)>
		{
			(0, 1),
			(1, 0),
			(-1, 0),
			(0, -1)
		};
		foreach (var (dx, dy) in diff)
		{
			var nx = node.x + dx;
			var ny = node.y + dy;
			
			if (nx < 0 || nx >= graph.GetLength(1))
				continue;
			
			if (ny < 0 || ny >= graph.GetLength(1))
				continue;

			yield return (nx, ny);
		}
	}
	
	private static int GetPathCost(int[,] graph)
	{
		var grid = new Grid(graph.GetLength(1), graph.GetLength(1));

		for (var y = 0; y < graph.GetLength(1); y++)
		for (var x = 0; x < graph.GetLength(1); x++)
			grid.SetCellCost(new Position(x, y), graph[y, x]);

		var path = grid.GetPath(new Position(0, 0), new Position(graph.GetLength(1) - 1, graph.GetLength(1) - 1),
			MovementPatterns.LateralOnly);

		return path.Aggregate(0, (i, position) => i + graph[position.Y, position.X]) - graph[0, 0];
	}

	private static int[,] ExtendGraph(List<List<int>> graph)
	{
		var (rows, cols) = (graph.Count, graph.First().Count);
		var fullGraph = new int[rows * 5, cols * 5];

		for (var y = 0; y < rows; y++)
		{
			for (var i = 0; i < 5; i++)
			{
				for (var x = 0; x < cols; x++)
				{
					for (var j = 0; j < 5; j++)
					{
						fullGraph[y + i * rows, x + j * cols] = graph[y][x] + i + j;

						if (fullGraph[y + i * rows, x + j * cols] > 9)
							fullGraph[y + i * rows, x + j * cols] =
								(fullGraph[y + i * rows, x + j * cols] % 10) + 1;
					}
				}
			}
		}

		return fullGraph;
	}
}