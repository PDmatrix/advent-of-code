using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._18;

[UsedImplicitly]
public class Year2020Day18 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		/*
		input = new[]
		{
			"1 + (2 * 3) + (4 * (5 + 6))",
		};
		*/
		var regex = new Regex(@"\(([^()]+)\)");

		long answer = 0;
		foreach (var line in input)
		{
			var temp = line;
			var match = regex.Match(temp);
			while (match.Success)
			{
				temp = temp.Replace(match.Groups[0].Value, Evaluate(match.Groups[1].Value).ToString());
				match = regex.Match(temp);
			}

			answer += Evaluate(temp);
		}

		return answer;
	}
	
	public object Part2(IEnumerable<string> input)
	{
		var regex = new Regex(@"\(([^()]+)\)");

		long answer = 0;
		foreach (var line in input)
		{
			var temp = line;
			var match = regex.Match(temp);
			while (match.Success)
			{
				temp = temp.Replace(match.Groups[0].Value, EvaluateAdvanced(match.Groups[1].Value).ToString());
				match = regex.Match(temp);
			}

			answer += EvaluateAdvanced(temp);
		}

		return answer;
	}
	
	private static long Evaluate(string expression)
	{
		var splt = expression.Split(' ');
		long result;
		
		var first = long.Parse(splt[0]);
		var second = long.Parse(splt[2]);
		if (splt[1][0] == '+')
			result = first + second;
		else
			result = first * second;
		
		for (int i = 3; i < splt.Length; i += 2)
		{
			var val = long.Parse(splt[i + 1]);
			if (splt[i][0] == '+')
				result += val;
			else
				result *= val;
		}

		return result;
	}
	
	private static long EvaluateAdvanced(string expression)
	{
		var temp = expression;
		var regex = new Regex(@"(\d+ \+ \d+)");
		var match = regex.Match(temp);
		while (match.Success)
		{
			temp = temp[..match.Index] + Evaluate(match.Groups[1].Value) +
			       temp.Substring(match.Index + match.Length, temp.Length - (match.Index + match.Length));
			match = regex.Match(temp);
		}

		return long.TryParse(temp, out var val) ? val : Evaluate(temp);
	}
}