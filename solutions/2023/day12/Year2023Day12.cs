using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._12;

[UsedImplicitly]
public class Year2023Day12 : ISolution
{
	private readonly Dictionary<string, long> _cache = new();
	public object Part1(IEnumerable<string> input)
	{
		var rows = ParseInput(input);
		long result = 0;
		foreach (var (pattern, counts) in rows)
			result += Calculate(pattern, counts);

		return result;
	}

	public object Part2(IEnumerable<string> input)
	{
		var rows = ParseInput(input);
		long result = 0;
		foreach (var (pattern, counts) in rows)
		{
			var unfoldedPattern = string.Join('?', Enumerable.Repeat(pattern, 5));
			var unfoldedCounts = Enumerable.Repeat(counts, 5).SelectMany(g => g).ToList();
			result += Calculate(unfoldedPattern, unfoldedCounts);
		}

		return result;
	}

	// cudos to u/yfilipov
	// https://www.reddit.com/r/adventofcode/comments/18ge41g/comment/kd0u7ej
	private long Calculate(string springs, List<int> groups)
	{
		var key = $"{springs},{string.Join(',', groups)}";

		if (_cache.TryGetValue(key, out var value))
		{
			return value;
		}

		value = GetCount(springs, groups);
		_cache[key] = value;

		return value;
	}

	private long GetCount(string springs, List<int> groups)
	{
		while (true)
		{
			if (groups.Count == 0)
			{
				return
					springs.Contains('#')
						? 0
						: 1; // No more groups to match: if there are no springs left, we have a match
			}

			if (string.IsNullOrEmpty(springs))
			{
				return 0; // No more springs to match, although we still have groups to match
			}

			if (springs.StartsWith('.'))
			{
				springs = springs.Trim('.'); // Remove all dots from the beginning
				continue;
			}

			if (springs.StartsWith('?'))
			{
				return Calculate("." + springs[1..], groups) +
				       Calculate("#" + springs[1..], groups); // Try both options recursively
			}

			if (springs.StartsWith('#')) // Start of a group
			{
				if (groups.Count == 0)
				{
					return 0; // No more groups to match, although we still have a spring in the input
				}

				if (springs.Length < groups[0])
				{
					return 0; // Not enough characters to match the group
				}

				if (springs[..groups[0]].Contains('.'))
				{
					return 0; // Group cannot contain dots for the given length
				}

				if (groups.Count > 1)
				{
					if (springs.Length < groups[0] + 1 || springs[groups[0]] == '#')
					{
						return 0; // Group cannot be followed by a spring, and there must be enough characters left
					}

					springs = springs[
						(groups[0] + 1)..]; // Skip the character after the group - it's either a dot or a question mark
					groups = groups.Skip(1).ToList();
					continue;
				}

				springs = springs[groups[0]..]; // Last group, no need to check the character after the group
				groups = groups.Skip(1).ToList();
				continue;
			}

			throw new Exception("Invalid input");
		}
	}

	private static List<(string, List<int>)> ParseInput(IEnumerable<string> input)
	{
		var result = new List<(string, List<int>)>();
		foreach (var line in input)
		{
			var parts = line.Split(' ');
			var pattern = parts[0];
			var counts = parts[1].Split(',').Select(int.Parse).ToList();
			result.Add((pattern, counts));
		}

		return result;
	}
}