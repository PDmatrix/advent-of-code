using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._6;

[UsedImplicitly]
public class Year2022Day06 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		return Solve(input, 4);
	}

	public object Part2(IEnumerable<string> input)
	{
		return Solve(input, 14);
	}

	private static int Solve(IEnumerable<string> input, int distinct)
	{
		var signal = input.First();
		var queue = new Queue<char>();
		for (var i = 0; i < signal.Length; i++)
		{
			queue.Enqueue(signal[i]);
			if (queue.Count != distinct)
				continue;
			
			if (IsUnique(queue.ToList()))
				return i + 1;

			queue.Dequeue();
		}

		throw new Exception("Solution is not found");
	}

	private static bool IsUnique(List<char> l)
	{
		var hs = new HashSet<char>();
		foreach (var c in l)
		{
			if (hs.Contains(c))
				return false;

			hs.Add(c);
		}

		return true;
	}
}