using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._8
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day08 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var numbers = input.First().Split(' ').Select(int.Parse).ToList();
			var acc = 2;
			var stack = new Stack<(int children, int meta)>();
			stack.Push((numbers[0], numbers[1]));

			var total = 0;
			while (acc < numbers.Count)
			{
				var (children, meta) = stack.Pop();

				if (children == 0)
				{
					for (var i = 0; i < meta; i++)
					{
						total += numbers[acc];
						acc += 1;
					}

					continue;
				}

				stack.Push((children - 1, meta));
				stack.Push((numbers[acc], numbers[acc + 1]));
				acc += 2;
			}

			return total.ToString();
		}

		public object Part2(IEnumerable<string> input)
		{
			var numbers = input.First().Split(' ').Select(int.Parse).ToList();
			var acc = 2;
			var stack = new Stack<(int node, int children, int meta)>();
			stack.Push((1, numbers[0], numbers[1]));

			var graph = new Dictionary<int, List<List<int>>>
			{
				{1, new List<List<int>> {new List<int>(), new List<int>()}}
			};
			var nodeCount = 1;

			while (acc < numbers.Count)
			{
				var (node, children, meta) = stack.Pop();

				if (children == 0)
				{
					for (var i = 0; i < meta; i++)
					{
						graph[node][1].Add(numbers[acc]);
						acc++;
					}

					continue;
				}

				children--;
				nodeCount++;
				graph[nodeCount] = new List<List<int>> {new List<int>(), new List<int>()};
				graph[node][0].Add(nodeCount);
				stack.Push((node, children, meta));
				stack.Push((nodeCount, numbers[acc], numbers[acc + 1]));
				acc += 2;
			}

			return CountNodeValue(graph, 1).ToString();
		}

		private static int CountNodeValue(IReadOnlyDictionary<int, List<List<int>>> graph, int node)
		{
			var children = graph[node][0];
			var meta = graph[node][1];

			return children.Count == 0
				? meta.Sum()
				: meta.Sum(x => x > children.Count ? 0 : CountNodeValue(graph, children[x - 1]));
		}
	}
}