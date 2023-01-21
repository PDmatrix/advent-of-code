using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._9;

[UsedImplicitly]
public class Year2020Day09 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var nums = input.Select(long.Parse).ToList();
		var preamble = nums.Take(25).ToList();

		return GetInvalidNumber(nums.Skip(25), preamble);
	}

	public object Part2(IEnumerable<string> input)
	{
		var nums = input.Select(long.Parse).ToList();
		var preamble = nums.Take(25).ToList();

		var invalidNumber = GetInvalidNumber(nums.Skip(25), preamble);
		
		for (int j = 2; j < nums.Count; j++)
		{
			for (int i = 0; i < nums.Count; i++)
			{
				var contiguousList = nums.Skip(i).Take(j).ToList();
				if (contiguousList.Sum() == invalidNumber)
					return contiguousList.Min() + contiguousList.Max();
			}
		}

		throw new Exception("No answer found");
	}

	private static long GetInvalidNumber(IEnumerable<long> enumerable, List<long> preamble)
	{
		foreach (var num in enumerable)
		{
			if (!IsValid(num, preamble))
				return num;

			preamble.RemoveAt(0);
			preamble.Add(num);
		}

		return -1;
	}

	private static bool IsValid(long required, List<long> numbers)
	{
		foreach (var number in numbers)
		{
			var desired = required - number;
			if (numbers.Contains(desired))
				return true;
		}

		return false;
	}
}