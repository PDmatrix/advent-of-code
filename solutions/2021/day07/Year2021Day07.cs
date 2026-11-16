using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2021._7;

[UsedImplicitly]
public class Year2021Day07 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var crabs = input.First().Split(',').Select(int.Parse).ToList();
		var min = crabs.Min();
		var max = crabs.Max();

		var minFuel = int.MaxValue;
		for (var i = min; i <= max; i++)
		{
			var currentFuel = crabs.Sum(crab => Math.Abs(crab - i));
			minFuel = Math.Min(currentFuel, minFuel);
		}
		
		return minFuel;
	}

	public object Part2(IEnumerable<string> input)
	{
		var crabs = input.First().Split(',').Select(int.Parse).ToList();
		var min = crabs.Min();
		var max = crabs.Max();

		var minFuel = int.MaxValue;
		for (var i = min; i <= max; i++)
		{
			var currentFuel = crabs.Sum(crab =>
				FactorialSum(Math.Abs(crab - i)));
			minFuel = Math.Min(currentFuel, minFuel);
		}

		return minFuel;
	}

	private static readonly Dictionary<int, int> Cache = new()
	{
		[0] = 0, [1] = 1
	};
	private static int FactorialSum(int n)
	{
		if (Cache.TryGetValue(n, out var value))
			return value;

		var sum = n + FactorialSum(n - 1);
		Cache[n] = sum;
		
		return Cache[n];
	}
}