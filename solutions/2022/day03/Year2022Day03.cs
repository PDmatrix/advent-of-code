using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._3;

[UsedImplicitly]
public class Year2022Day03 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var duplicate = new List<char>();
		foreach (var line in input)
		{
			
			var set = new HashSet<char>();
			foreach (var c in line[..(line.Length / 2)])
				set.Add(c);

			foreach (var c in line[(line.Length / 2)..])
			{
				if (!set.Contains(c)) continue;
				
				duplicate.Add(c);
				break;
			}
		}

		return duplicate.Select(x =>
		{
			var n = (int)x;
			if (n >= 97)
				return n - 96;

			return n - 38;
		}).Sum();
	}

	public object Part2(IEnumerable<string> input)
	{
		var duplicate = new List<char>();
		var inputList = input.ToList();
		for (var i = 0; i < inputList.Count; i += 3)
		{
			var hs = inputList[i].ToCharArray().ToHashSet();
			for (var j = i; j < i + 3; j++)
				hs.IntersectWith(inputList[j].ToCharArray().ToHashSet());
			
			duplicate.Add(hs.First());
		}

		return duplicate.Select(x =>
		{
			var n = (int)x;
			if (n >= 97)
				return n - 96;

			return n - 38;
		}).Sum();
	}
}