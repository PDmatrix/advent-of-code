using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._6;

[UsedImplicitly]
public class Year2020Day06 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var list = new List<HashSet<char>>();
		var tempSet = new HashSet<char>();
		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				list.Add(new HashSet<char>(tempSet));
				tempSet.Clear();
				continue;
			}

			foreach (var ch in line)
				tempSet.Add(ch);
		}
		
		list.Add(tempSet);
		
		return list.Sum(set => set.Count);
	}

	public object Part2(IEnumerable<string> input)
	{
		var list = new List<(int persons, Dictionary<char, int> map)>();
		var tempMap = new Dictionary<char, int>();
		var persons = 0;
		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				list.Add((persons, tempMap.ToDictionary(x => x.Key, x => x.Value)));
				persons = 0;
				tempMap.Clear();
				continue;
			}

			persons += 1;

			foreach (var ch in line)
			{
				if (!tempMap.ContainsKey(ch))
					tempMap.Add(ch, 1);
				else
					tempMap[ch] += 1;
			}
		}
		
		list.Add((persons, tempMap));

		return list.Sum(element => element.map.Count(x => x.Value == element.persons));
	}
}