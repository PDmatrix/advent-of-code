using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._10;

[UsedImplicitly]
public class Year2020Day10 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var adapters = input.Select(int.Parse).ToList();
		adapters.Add(adapters.Max() + 3);
		
		adapters.Sort();

		var currentAdapter = 0;
		var oneDiff = 0;
		var threeDiff = 0;
		foreach (var adapter in adapters)
		{
			if (adapter - currentAdapter == 3)
				threeDiff++;
			else
				oneDiff++;
			
			currentAdapter = adapter;
		}
		
		return oneDiff * threeDiff;
	}

	public object Part2(IEnumerable<string> input)
	{
		var adapters = input.Select(long.Parse).ToList();
		adapters.Add(adapters.Max() + 3);
		
		adapters.Sort();

		var dp = new Dictionary<long, long>
		{
			[0] = 1
		};
		
		foreach (var adapter in adapters)
		{
			dp[adapter] = dp.GetValueOrDefault(adapter - 1, 0) + dp.GetValueOrDefault(adapter - 2, 0) +
			              dp.GetValueOrDefault(adapter - 3, 0);
		}

		return dp[adapters.Last()];
	}
}