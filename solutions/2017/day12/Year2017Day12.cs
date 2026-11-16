using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2017._12;

[UsedImplicitly]
public class Year2017Day12 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var programs = ParseInput(input);
		var set = GetGroup(0, programs);
		return set.Count.ToString();
	}

	public object Part2(IEnumerable<string> input)
	{
		var programs = ParseInput(input);
		var groups = 0;
		var set = new HashSet<int>();
		for (var i = 0; i < programs.Count; i++)
		{
			if(set.Contains(i)) 
				continue;

			groups++;
			Visit(set, programs[i], programs);
		}
		return groups.ToString();
	}

	private static void Visit(
		ISet<int> set,
		IEnumerable<int> destinations,
		IReadOnlyDictionary<int, IEnumerable<int>> programs)
	{
		foreach (var destination in destinations)
		{
			if (set.Contains(destination))
				continue;

			set.Add(destination);
			Visit(set, programs[destination], programs);
		}
	}

	private static ISet<int> GetGroup(int start, IReadOnlyDictionary<int, IEnumerable<int>> programs)
	{
		var set = new HashSet<int>();
		Visit(set, programs[start], programs);
		return set;
	}
		
	private static Dictionary<int, IEnumerable<int>> ParseInput(IEnumerable<string> input)
	{
		var regex = new Regex(@"(?<from>\d+) <-> (?<to>.+)", RegexOptions.Compiled);
		var programs = input
			.Select(x => regex.Match(x))
			.Select(x => new
			{
				From = int.Parse(x.Groups["from"].Value),
				To = x.Groups["to"].Value.Split(", ").Select(int.Parse)
			})
			.ToDictionary(key => key.From, value => value.To);
			
		return programs;
	}
}