using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._4;

[UsedImplicitly]
public class Year2022Day04 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var result = 0;
		foreach (var line in input)
		{
			var (firstElf, secondElf) = ParseElves(line);

			if (firstElf.l >= secondElf.l && firstElf.r <= secondElf.r)
				result++;
			else if (secondElf.l >= firstElf.l && secondElf.r <= firstElf.r)
				result++;
		}

		return result;
	}

	public object Part2(IEnumerable<string> input)
	{
		var result = 0;
		foreach (var line in input)
		{
			var (firstElf, secondElf) = ParseElves(line);

			if (firstElf.l <= secondElf.r && firstElf.r >= secondElf.l)
				result++;
			else if (secondElf.l <= firstElf.r && secondElf.r >= firstElf.l)
				result++;
		}

		return result;
	}

	private static ((int l, int r), (int l, int r)) ParseElves(string line)
	{
		var pairs = line.Split(',');
		var firstElfSegment = pairs[0].Split('-');
		var secondElfSegment = pairs[1].Split('-');
		var firstElf = (l: int.Parse(firstElfSegment[0]), r: int.Parse(firstElfSegment[1]));
		var secondElf = (l: int.Parse(secondElfSegment[0]), r: int.Parse(secondElfSegment[1]));

		return (firstElf, secondElf);
	}
}