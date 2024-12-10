using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._3;

[UsedImplicitly]
public class Year2024Day03 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var sb = new StringBuilder();
		foreach (var line in input)
			sb.Append(line);

		return Mul(sb.ToString());
	}

	private static int Mul(string line)
	{
		var answer = 0;
		var matches = Regex.Matches(line, @"(mul\(\d+,\d+\))");
		foreach (Match match in matches)
		{
			var values = match.Value.Split(',');
			answer += int.Parse(values[0].TrimStart("mul(".ToCharArray())) *
			          int.Parse(values[1].TrimEnd(")".ToCharArray()));
		}

		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var sb = new StringBuilder();
		foreach (var line in input)
			sb.Append(line);

		var answer = Mul(sb.ToString());

		// we remove the mul from the final answer if it's inside a don't ... do block
		var matches = Regex.Matches(sb.ToString(), @"(don't\(\).*?(do\(\)))");
		foreach (Match match in matches)
			answer -= Mul(match.Value);

		return answer;
	}
}