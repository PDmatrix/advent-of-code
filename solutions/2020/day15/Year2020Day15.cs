using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._15;

[UsedImplicitly]
public class Year2020Day15 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var list = input.First().Split(',').Select(int.Parse).ToList();
		var spokenIndex = 0;
		var spoken = list.ToDictionary(x => x, _ =>
		{
			spokenIndex++;
			return new SortedList<int, int> { { spokenIndex, spokenIndex } };
		});
		
		var lastSpoken = list.Last();
		for (int i = spokenIndex + 1; i <= 2020; i++)
		{
			if (!spoken.ContainsKey(lastSpoken) || spoken[lastSpoken].Count <= 1)
			{
				lastSpoken = 0;
				if (spoken.ContainsKey(lastSpoken))
					spoken[lastSpoken].Add(i, i);
				else
					spoken[lastSpoken] = new SortedList<int, int> { { i, i } };
			}
			else
			{
				var lastTwo = spoken[lastSpoken].TakeLast(2).ToList();
				var first = lastTwo.First().Key;
				var second = lastTwo.Last().Key;
				lastSpoken = second - first;
				if (spoken.ContainsKey(lastSpoken))
					spoken[lastSpoken].Add(i, i);
				else
					spoken[lastSpoken] = new SortedList<int, int> { { i, i } };
			}
		}
		
		return lastSpoken;
	}

	public object Part2(IEnumerable<string> input)
	{
		var list = input.First().Split(',').Select(int.Parse).ToList();
		var spokenIndex = 0;
		var spoken = list.ToDictionary(x => x, _ =>
		{
			spokenIndex++;
			return new Pos(spokenIndex, -1);
		});
		
		var lastSpoken = list.Last();
		for (int i = spokenIndex + 1; i <= 30000000; i++)
		{
			if (!spoken.ContainsKey(lastSpoken) || spoken[lastSpoken].Second == -1)
			{
				lastSpoken = 0;
			}
			else
			{
				var first = spoken[lastSpoken].First;
				var second = spoken[lastSpoken].Second;
				lastSpoken = second - first;
			}

			if (spoken.ContainsKey(lastSpoken) && spoken[lastSpoken].Second != -1)
				spoken[lastSpoken] = new Pos(spoken[lastSpoken].Second, i);
			else if (!spoken.ContainsKey(lastSpoken))
				spoken[lastSpoken] = new Pos(i, -1);
			else
				spoken[lastSpoken] = new Pos(spoken[lastSpoken].First, i);
		}

		return lastSpoken;
	}

	private readonly record struct Pos(int First, int Second);
}