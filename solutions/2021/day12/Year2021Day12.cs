using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._12;

[UsedImplicitly]
public class Year2021Day12 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var graph = ParseInput(input);

		var queue = new Queue<(string node, HashSet<string> visited)>();
		queue.Enqueue(("start", new HashSet<string> {"start"}));

		var paths = 0;
		while (queue.Count != 0)
		{
			var current = queue.Dequeue();

			foreach (var neighbor in graph[current.node])
			{
				if (neighbor == "end")
				{
					paths++;
					continue;
				}
				
				if (IsLowerCase(neighbor) && current.visited.Contains(neighbor))
					continue;

				var newVisited = new HashSet<string>(current.visited);
				if (IsLowerCase(neighbor))
					newVisited.Add(neighbor);
				
				queue.Enqueue((neighbor, newVisited));
			}
		}
		
		return paths;
	}

	public object Part2(IEnumerable<string> input)
	{
		var graph = ParseInput(input);

		var queue = new Queue<(string node, HashSet<string> visited, bool twiceVisited)>();
		queue.Enqueue(("start", new HashSet<string> {"start"}, false));

		var paths = 0;
		while (queue.Count != 0)
		{
			var current = queue.Dequeue();

			foreach (var neighbor in graph[current.node])
			{
				switch (neighbor)
				{
					case "end":
						paths++;
						continue;
					case "start":
						continue;
				}

				if (IsLowerCase(neighbor) && current.visited.Contains(neighbor) && current.twiceVisited)
					continue;

				var newVisited = new HashSet<string>(current.visited);
				if (IsLowerCase(neighbor))
					newVisited.Add(neighbor);

				var isTwiceVisited = current.twiceVisited || IsLowerCase(neighbor) && current.visited.Contains(neighbor);

				queue.Enqueue((neighbor, newVisited, isTwiceVisited));
			}
		}
		
		return paths;
	}

	private static Dictionary<string, List<string>> ParseInput(IEnumerable<string> input)
	{
		var graph = new Dictionary<string, List<string>>();
		foreach (var line in input)
		{
			var splitted = line.Split("-");
			if (graph.ContainsKey(splitted[0]))
				graph[splitted[0]].Add(splitted[1]);
			else
				graph[splitted[0]] = new List<string> { splitted[1] };
			
			if (graph.ContainsKey(splitted[1]))
				graph[splitted[1]].Add(splitted[0]);
			else
				graph[splitted[1]] = new List<string> { splitted[0] };
		}

		return graph;
	}

	private static bool IsLowerCase(string input)
	{
		return input.ToLower() == input;
	}
}