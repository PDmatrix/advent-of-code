using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2018._1;

[UsedImplicitly]
public class Year2018Day01 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		return input.Select(int.Parse).Sum().ToString();
	}

	public object Part2(IEnumerable<string> input)
	{
		var frequencyList = input.Select(int.Parse).ToArray();
		var seenFrequencies = new HashSet<int> {0};
		var currentFrequency = 0;
		foreach (var frequency in Cycle(frequencyList))
		{
			currentFrequency += frequency;

			if (seenFrequencies.Contains(currentFrequency))
				return currentFrequency.ToString();

			seenFrequencies.Add(currentFrequency);
		}
			
		throw new Exception();
	}

	private static IEnumerable<T> Cycle<T>(IEnumerable<T> iterable)
	{
		var enumerable = iterable as T[] ?? iterable.ToArray();
		while (true)
		{
			foreach (var t in enumerable)
			{
				yield return t;
			}
		}
	}
}