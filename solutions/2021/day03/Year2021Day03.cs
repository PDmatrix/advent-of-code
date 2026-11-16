using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._3;

[UsedImplicitly]
public class Year2021Day03 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var reports = input.ToList();
		var gammaRateString = "";
		var epsilonRateString = "";
		
		for (var i = 0; i < reports.First().Length; i++)
		{
			gammaRateString += GetMostCommonBit(reports, i);
			epsilonRateString += GetLeastCommonBit(reports, i);
		}
		
		return Convert.ToInt32(gammaRateString, 2) * Convert.ToInt32(epsilonRateString, 2);
	}

	public object Part2(IEnumerable<string> input)
	{
		var reports = input.ToList();

		var oxygenGeneratorRating = new List<string>(reports);
		for (var i = 0; i < reports.First().Length; i++)
		{
			var mostCommonBit = GetMostCommonBit(oxygenGeneratorRating, i);
			oxygenGeneratorRating = oxygenGeneratorRating.Where(x => x[i] == mostCommonBit).ToList();
			if (oxygenGeneratorRating.Count == 1)
				break;
		}

		var co2ScrubberRating = new List<string>(reports);
		for (var i = 0; i < reports.First().Length; i++)
		{
			var leastCommonBit = GetLeastCommonBit(co2ScrubberRating, i);
			co2ScrubberRating = co2ScrubberRating.Where(x => x[i] == leastCommonBit).ToList();
			if (co2ScrubberRating.Count == 1)
				break;
		}


		return Convert.ToInt32(co2ScrubberRating.First(), 2) * Convert.ToInt32(oxygenGeneratorRating.First(), 2);
	}

	private static char GetMostCommonBit(List<string> list, int position)
	{
		var mostCommon = list.Sum(report => report[position] == '1' ? 1 : -1);
		return mostCommon < 0 ? '0' : '1';
	}
	
	private static char GetLeastCommonBit(List<string> list, int position)
	{
		var mostCommon = list.Sum(report => report[position] == '1' ? 1 : -1);
		return mostCommon < 0 ? '1' : '0';
	}
}