using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2015._24;

[UsedImplicitly]
public class Year2015Day24 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var numbers = input.Select(int.Parse).ToArray();
		var sum = numbers.Sum() / 3;
		var qe = SumUp(numbers, sum)
			.Select(r => new {QE = r.Aggregate((a, b) => a * b), r.Count})
			.OrderBy(r => r.Count)
			.ThenBy(r => r.QE)
			.First();
		return qe.QE.ToString();
	}

	public object Part2(IEnumerable<string> input)
	{
		_bestValue = int.MaxValue;
		var numbers = input.Select(int.Parse).ToArray();
		var sum = numbers.Sum() / 4;
		var qe = SumUp(numbers, sum)
			.Select(r => new {QE = r.Aggregate((a, b) => a * b), r.Count})
			.OrderBy(r => r.Count)
			.ThenBy(r => r.QE)
			.First();
		return qe.QE.ToString();
	}
		
	private static IEnumerable<List<long>> SumUp(IReadOnlyList<int> numbers, int target)
	{
		return SumUpRecursive(numbers, target, new List<long>());
	}
	private static int _bestValue = int.MaxValue;
	private static IEnumerable<List<long>> SumUpRecursive(IReadOnlyList<int> numbers, int target, List<long> partial)
	{
		var res = new List<List<long>>();
		if (partial.Count >= _bestValue) return res;
		long s = 0;
		foreach (var x in partial) 
			s += x;

		if (s == target)
		{
			res.Add(partial);
			if (partial.Count < _bestValue)
				_bestValue = partial.Count;
		}

		if (s >= target)
			return res;

		for (var i = 0; i < numbers.Count; i++)
		{
			var remaining = new List<int>();
			var n = numbers[i];
			for (var j = i + 1; j < numbers.Count; j++) 
				remaining.Add(numbers[j]);

			var partialRec = new List<long>(partial) {n};
			var rs = SumUpRecursive(remaining, target, partialRec);
			res = res.Union(rs).ToList();
		}

		return res;
	}
}