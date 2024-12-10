using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._2;

[UsedImplicitly]
public class Year2024Day02 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		return input.Count(CheckIfSafe);
	}

	public object Part2(IEnumerable<string> input)
	{
		return input.Count(CheckIfSafeWithToleration);
	}

	private static bool CheckIfSafe(string line)
	{
		var numbers = line.Split().Select(int.Parse).ToList();

		return Check(numbers, 0);
	}

	private static bool CheckIfSafeWithToleration(string line )
	{
		var numbers = line.Split().Select(int.Parse).ToList();
		
		for (var i = 0; i < numbers.Count; i++)
		{
			var clone = new List<int>(numbers);
			clone.RemoveAt(i);
			if (Check(clone, 0))
				return true;
		}

		return false;
	}

	private static bool Check(List<int> list, int n)
	{
		if (n == list.Count - 1)
			return true;

		if (Math.Abs(list[n] - list[n + 1]) > 3 || list[n] == list[n + 1])
			return false;
		
		if (n == 0)
			return Check(list, 1);
		
		// 7 5 6
		if (list[n + 1] > list[n] && list[n] < list[n - 1])
			return false;
		
		// 3 5 4
		if (list[n + 1] < list[n] && list[n] > list[n - 1])
			return false;
		
		return Check(list, n + 1);
	}
}