using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._1;

[UsedImplicitly]
public class Year2020Day01 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var set = new HashSet<int>();
		var list = input.Select(int.Parse).ToList();

		return TwoSum(2020, list);
	}

	private static int TwoSum(int required, List<int> numbers)
	{
		var set = new HashSet<int>();
		foreach (var number in numbers)
			set.Add(number);

		foreach (var number in numbers)
		{
			var desired = required - number;
			if (set.Contains(desired))
				return number * desired;
		}

		return -1;
	}

	public object Part2(IEnumerable<string> input)
	{
		var list = input.Select(int.Parse).ToList();

		const int required = 2020;
		foreach (var number in list)
		{
			var desired = required - number;
			var s = TwoSum(desired, list.Where(x => x != number).ToList());
			if (s != -1)
				return number * s;
		}

		throw new Exception("Answer is not found");
	}
}