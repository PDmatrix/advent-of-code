using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2021._6;

[UsedImplicitly]
public class Year2021Day06 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var lanternFishes = input.First().Split(',').Select(int.Parse).ToList();
		for (var day = 0; day < 80; day++)
		{
			var currentCount = lanternFishes.Count;
			for (var i = 0; i < currentCount; i++)
			{
				if (lanternFishes[i] == 0)
				{
					lanternFishes[i] = 6;
					lanternFishes.Add(8);
					continue;
				}

				lanternFishes[i]--;
			}
		}
		
		return lanternFishes.Count;
	}

	public object Part2(IEnumerable<string> input)
	{
		var lanternFishes = input.First().Split(',').Select(int.Parse).ToList();
		var dict = new Dictionary<int, long>();
		foreach (var lanternFish in lanternFishes)
		{
			if (dict.ContainsKey(lanternFish))
				dict[lanternFish]++;
			else
				dict[lanternFish] = 1;
		}
		for (var day = 0; day < 256; day++)
		{
			var newDict = new Dictionary<int, long>();
			foreach (var kv in dict)
				newDict[kv.Key - 1] = kv.Value;

			if (newDict.ContainsKey(-1))
			{
				if (newDict.ContainsKey(6))
					newDict[6] += newDict[-1];
				else
					newDict[6] = newDict[-1];
				
				if (newDict.ContainsKey(8))
					newDict[8] += newDict[-1];
				else
					newDict[8] = newDict[-1];

				newDict.Remove(-1);
			}

			dict = newDict;
		}

		return dict.Sum(x => x.Value);
	}
}