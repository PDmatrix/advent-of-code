using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._25;

[UsedImplicitly]
public class Year2023Day25 : ISolution
{
	// this puzzle was pretty hard for me so code is AI generated (shame!)
	public object Part1(IEnumerable<string> input)
	{
		var connections = ParseInput(input);
		if (connections.Count < 2)
			throw new InvalidOperationException("At least two components are required.");

		// Each undirected wire starts with capacity 1 in both directions.
		var capacities = new Dictionary<(string From, string To), int>();
		foreach (var (from, neighbors) in connections)
			foreach (var to in neighbors)
				capacities[(from, to)] = 1;

		// Fix one component. Eventually a target will be on the other side of the cut.
		var source = connections.Keys.First();
		foreach (var target in connections.Keys.Where(node => node != source))
		{
			var residual = new Dictionary<(string From, string To), int>(capacities);
			for (var flow = 0; flow < 4; flow++)
			{
				var parents = FindPath(connections, residual, source, target);
				if (!parents.ContainsKey(target))
				{
					// No more flow can reach the target. BFS has visited exactly
					// the source side of a minimum cut in the residual graph.
					if (flow != 3)
						throw new InvalidOperationException("Expected a minimum cut of three wires.");

					var groupSize = parents.Count;
					return groupSize * (connections.Count - groupSize);
				}

				// Send one unit along the path. Reverse capacity lets a later
				// path undo earlier choices instead of getting stuck with them.
				for (var node = target; node != source; node = parents[node])
				{
					var previous = parents[node];
					residual[(previous, node)]--;
					residual[(node, previous)]++;
				}
			}
			// Four units made it through: three wires cannot separate this pair.
		}

		throw new InvalidOperationException("No three-wire cut was found.");
	}

	public object Part2(IEnumerable<string> input)
	{
		return "Congratulations!";
	}

	private static Dictionary<string, string> FindPath(
		Dictionary<string, HashSet<string>> connections,
		Dictionary<(string From, string To), int> residual,
		string source, string target)
	{
		var parents = new Dictionary<string, string> { [source] = source };
		var queue = new Queue<string>();
		queue.Enqueue(source);

		while (queue.TryDequeue(out var node))
		{
			foreach (var neighbor in connections[node])
			{
				if (residual[(node, neighbor)] == 0 || parents.ContainsKey(neighbor))
					continue;

				parents[neighbor] = node;
				if (neighbor == target)
					return parents;

				queue.Enqueue(neighbor);
			}
		}

		return parents;
	}

	private static Dictionary<string, HashSet<string>> ParseInput(IEnumerable<string> input)
	{
		var connections = new Dictionary<string, HashSet<string>>();

		foreach (var line in input.Where(line => !string.IsNullOrWhiteSpace(line)))
		{
			var parts = line.Split(':', 2);
			if (parts.Length != 2)
				throw new FormatException($"Invalid wiring diagram line: {line}");

			var root = parts[0].Trim();
			connections.TryAdd(root, new HashSet<string>());
			var components = parts[1].Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
			foreach (var component in components)
			{
				connections.TryAdd(component, new HashSet<string>());
				connections[root].Add(component);
				connections[component].Add(root);
			}
		}

		return connections;
	}
}