using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._19;

[UsedImplicitly]
public class Year2024Day19 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (patterns, desired) = ParseInput(input);
		return desired.Sum(d => IsPossible(new Dictionary<string, bool>(), patterns, d) ? 1 : 0);
	}

	public object Part2(IEnumerable<string> input)
	{
		var (patterns, desired) = ParseInput(input);
		return desired.Sum(d => GetPossibleWays(new Dictionary<string, long>(), patterns, d));
	}
	
	private static bool IsPossible(Dictionary<string, bool> memo, HashSet<string> patters, string desired)
	{
		if (memo.TryGetValue(desired, out var possible))
			return possible;

		if (patters.Contains(desired))
		{
			memo[desired] = true;
			return true;
		}

		for (var i = 0; i < desired.Length; i++)
		{
			if (!patters.Contains(desired[..(i+1)]))
				continue;

			if (!IsPossible(memo, patters, desired[(i + 1)..]))
				continue;
			
			memo[desired[(i + 1)..]] = true;
			return true;
		}

		memo[desired] = false;
		return false;
	}
	
	private static long GetPossibleWays(Dictionary<string, long> memo, HashSet<string> patterns, string desired)
	{
		if (memo.TryGetValue(desired, out var possible))
			return possible;
		
		if (desired == "")
			return 1;
		
		var ways = patterns.Where(desired.StartsWith).Sum(pattern => GetPossibleWays(memo, patterns, desired[pattern.Length..]));

		memo[desired] = ways;
		return ways;
	}

	private static (HashSet<string>, List<string>) ParseInput(IEnumerable<string> input)
	{
		var patterns = new HashSet<string>();
		var desired = new List<string>();
		
		var isPatterns = true;
		foreach (var line in input)
		{
			if (string.IsNullOrEmpty(line))
			{
				isPatterns = false;
				continue;
			}

			if (isPatterns)
			{
				patterns = new HashSet<string>(line.Split(", "));
				continue;
			}
			
			desired.Add(line);
		}

		return (patterns, desired);
	}
}