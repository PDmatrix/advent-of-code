using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._12;

[UsedImplicitly]
public class Year2025Day12 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (shapes, list) = ParseInput(input);

		var answer = 0;
		foreach (var (w, h, nums) in list)
		{
			var area = w * h;
			var total = nums.Select((t, i) => shapes[i] * t).Sum();
			if (total <= area)
				answer++;
		}

		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		return "Congratulations!";
	}

	private static (List<int>, List<(int, int, List<int>)>) ParseInput(IEnumerable<string> input)
	{
		var l = new List<(int, int, List<int>)>();
		var a = new List<int>();
		var regex = new Regex(@"(?<w>\d+)x(?<h>\d+): (?<nums>.*)");
		var c = 0;
		foreach (var line in input)
		{
			if (line.Contains("#"))
			{
				c += line.Count(x => x == '#');
				continue;
			}

			if (string.IsNullOrEmpty(line))
			{
				a.Add(c);
				c = 0;
				continue;
			}

			var match = regex.Match(line);
			if (!match.Success)
				continue;

			var w = int.Parse(match.Groups["w"].Value);
			var h = int.Parse(match.Groups["h"].Value);
			var nums = match.Groups["nums"].Value.Split(' ').Select(int.Parse).ToList();
			l.Add((w, h, nums));
		}

		return (a, l);
	}
}