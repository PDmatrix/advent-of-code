using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._6;

[UsedImplicitly]
public class Year2023Day06 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var times = GetNumbers(input.First());
		var distances = GetNumbers(input.Last());

		return times.Select((t, i) => GetWaysToWin(t, distances[i])).Aggregate(1, (t, i) => t * i);
	}

	public object Part2(IEnumerable<string> input)
	{
		var times = GetNumbers(input.First(), true);
		var distances = GetNumbers(input.Last(), true);

		return times.Select((t, i) => GetWaysToWin(t, distances[i])).Aggregate(1, (t, i) => t * i);
	}

	private static List<long> GetNumbers(string input, bool withoutSpaces = false)
	{
		var digits = new List<long>();
		var currentNumber = string.Empty;
		foreach (var c in input)
		{
			if (char.IsDigit(c))
			{
				currentNumber += c;
				continue;
			}
			
			if (withoutSpaces)
				continue;

			if (currentNumber != string.Empty)
				digits.Add(long.Parse(currentNumber));
			
			currentNumber = string.Empty;
		}
		
		if (currentNumber != string.Empty)
			digits.Add(long.Parse(currentNumber));
			
		return digits;
	}

	private static int GetWaysToWin(long time, long distance)
	{
		var wins = 0;
		for (var i = 1; i < time; i++)
		{
			var currentDistance = i * (time - i);
			if (currentDistance <= distance)
				continue;
			
			wins++;
		}
		
		return wins;
	}
}