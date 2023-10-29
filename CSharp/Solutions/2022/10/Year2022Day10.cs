using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._10;

[UsedImplicitly]
public class Year2022Day10 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var cycles = ParseInput(input);

		var neededCycles = new List<int> { 20, 60, 100, 140, 180, 220 };

		return neededCycles.Sum(neededCycle => cycles[neededCycle - 1] * neededCycle);
	}

	public object Part2(IEnumerable<string> input)
	{
		var cycles = ParseInput(input);
		var sb = new StringBuilder();
		sb.AppendLine();
		for (var i = 0; i < cycles.Count; i++)
		{
			if (i % 40 == 0)
				sb.AppendLine();

			var register = cycles[i];
			var pos = i % 40;
			if (pos == register || pos == register - 1 || pos == register + 1)
				sb.Append('#');
			else
				sb.Append('.');
		}

		return sb.ToString();
	}

	private static List<int> ParseInput(IEnumerable<string> input)
	{
		var cycles = new List<int>();
		var register = 1;
		foreach (var line in input)
		{
			if (line == "noop")
			{
				cycles.Add(register);
				continue;
			}

			var amount = int.Parse(line.Split()[1]);
			cycles.Add(register);
			cycles.Add(register);
			register += amount;
		}

		return cycles;
	}
}