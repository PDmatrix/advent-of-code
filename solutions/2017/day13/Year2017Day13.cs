using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2017._13;

[UsedImplicitly]
public class Year2017Day13 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var firewall = ParseInput(input);

		var res = Trip(firewall, 0);
			
		return res.ToString();
	}

	public object Part2(IEnumerable<string> input)
	{
		var firewall = ParseInput(input).ToList();

		int delay;
		for (delay = 0;; delay++)
		{
			var res = Trip(firewall, delay);
			if(res == -1) break;
		}

		return delay.ToString();
	}

	private static int Trip(IEnumerable<(int depth, int range)> firewall, int delay)
	{
		var res = -1;
		foreach (var (depth, range) in firewall)
		{
			if ((depth + delay) % (2 * (range - 1)) != 0) 
				continue;
				
			if (res == -1) res = 0;
			res += depth * range;
		}

		return res;
	}

	private static IEnumerable<(int depth, int range)> ParseInput(IEnumerable<string> input)
	{
		var regex = new Regex(@"(?<depth>\d+): (?<range>\d+)", RegexOptions.Compiled);
		foreach (var inp in input)
		{
			var groups = regex.Match(inp).Groups;
			var depth = int.Parse(groups["depth"].Value);
			var range = int.Parse(groups["range"].Value);

			yield return (depth, range);
		}
	}
}