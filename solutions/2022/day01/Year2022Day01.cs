using System.Collections.Generic;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._1;

[UsedImplicitly]
public class Year2022Day01 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var pq = ParseInput(input);

		return TopKSum(pq, 1);;
	}

	public object Part2(IEnumerable<string> input)
	{
		var pq = ParseInput(input);

		return TopKSum(pq, 3);
	}

	private static int TopKSum(PriorityQueue<int, int> pq, int k)
	{
		var finalSum = 0;
		for (var i = 0; i < k; i++)
			finalSum += pq.Dequeue();
		
		return finalSum;
	}

	private static PriorityQueue<int, int> ParseInput(IEnumerable<string> input)
	{
		var pq = new PriorityQueue<int, int>();
		var localSum = 0;
		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				pq.Enqueue(localSum, -1 * localSum);
				localSum = 0;
				continue;
			}

			localSum += int.Parse(line);
		}

		pq.Enqueue(localSum, -1 * localSum);
		return pq;
	}
}