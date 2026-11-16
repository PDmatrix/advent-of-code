using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._1;

[UsedImplicitly]
public class Year2024Day01 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var left = new List<int>();
		var right = new List<int>();
		foreach (var line in input)
		{
			var splitted = line.Split("   ");
			left.Add(int.Parse(splitted[0]));
			right.Add(int.Parse(splitted[1]));
		}
		left.Sort();
		right.Sort();
		
		return left.Select((t, i) => Math.Abs(t - right[i])).Sum();
	}

	public object Part2(IEnumerable<string> input)
	{
		var left = new List<int>();
		var right = new Dictionary<int, int>();
		foreach (var line in input)
		{
			var splitted = line.Split("   ");
			left.Add(int.Parse(splitted[0]));

			var second = int.Parse(splitted[1]);
			right.TryAdd(second, 0);
			right[second]++;
		}
		left.Sort();

		return left.Sum(i => i * right.GetValueOrDefault(i, 0));
	}
}