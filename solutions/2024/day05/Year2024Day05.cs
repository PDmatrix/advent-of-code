using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._5;

[UsedImplicitly]
public class Year2024Day05 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (rules, updates) = ParseInput(input);

		return updates.Where(update => IsInCorrectOrder(update, rules)).Sum(update => update[update.Count / 2]);
	}

	private static (Dictionary<int, List<int>>, List<List<int>>) ParseInput(IEnumerable<string> input)
	{
		var rules = new Dictionary<int, List<int>>();
		var updates = new List<List<int>>();

		var processingUpdates = false;
		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				processingUpdates = true;
				continue;
			}

			if (processingUpdates)
			{
				updates.Add(line.Split(",").Select(int.Parse).ToList());
				continue;
			}

			var parts = line.Split("|");
			if (!rules.ContainsKey(int.Parse(parts[0])))
				rules[int.Parse(parts[0])] = new();
			
			rules[int.Parse(parts[0])].Add(int.Parse(parts[1]));
		}

		return (rules, updates);
	}

	private static bool IsInCorrectOrder(List<int> update, Dictionary<int, List<int>> rules)
	{
		var printed = new HashSet<int>();
		foreach (var page in update)
		{
			if (rules.GetValueOrDefault(page, new List<int>()).Any(pageAfter => printed.Contains(pageAfter)))
				return false;

			printed.Add(page);
		}

		return true;
	}

	public object Part2(IEnumerable<string> input)
	{
		var (rules, updates) = ParseInput(input);

		var answer = 0;
		foreach (var update in updates)
		{
			if (IsInCorrectOrder(update, rules))
				continue;

			var hs = update.ToHashSet();
			var requirements = new Dictionary<int, List<int>>();
			foreach (var i in update)
			{
				if (!requirements.ContainsKey(i))
					requirements[i] = new List<int>();

				foreach (var j in rules.GetValueOrDefault(i, new List<int>()).Where(j => hs.Contains(j)))
					requirements[i].Add(j);
			}

			answer += requirements.Single(x => x.Value.Count == requirements.Count / 2).Key;
		}

		return answer;
	}
}