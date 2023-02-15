using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._14;

[UsedImplicitly]
public class Year2021Day14 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var template = input.First();
		var rules = new Dictionary<(char, char), string>();
		foreach (var line in input.Skip(2))
		{
			var splitted = line.Split(" -> ");
			rules[(splitted[0][0], splitted[0][1])] = splitted[1];
		}

		var currentPolymer = template;
		for (int steps = 0; steps < 10; steps++)
		{
			var newPolymer = new StringBuilder();
			for (int i = 0; i < currentPolymer.Length - 1; i++)
			{
				newPolymer.Append(currentPolymer[i]);
				var pair = (currentPolymer[i], currentPolymer[i + 1]);
				newPolymer.Append(rules[pair]);
			}

			newPolymer.Append(currentPolymer[^1]);

			currentPolymer = newPolymer.ToString();
		}

		var count = new Dictionary<string, int>();
		foreach (var polymer in currentPolymer)
		{
			if (!count.ContainsKey(polymer.ToString()))
				count.Add(polymer.ToString(), 1);
			else
				count[polymer.ToString()]++;
		}

		return count.MaxBy(x => x.Value).Value - count.MinBy(x => x.Value).Value;
	}

	public object Part2(IEnumerable<string> input)
	{
		var template = input.First();
		var rules = new Dictionary<(char, char), char>();
		foreach (var line in input.Skip(2))
		{
			var splitted = line.Split(" -> ");
			rules[(splitted[0][0], splitted[0][1])] = splitted[1][0];
		}

		var count = new Dictionary<char, long>();
		var currentPolymer = new Dictionary<(char f, char s), long>();
		for (var i = 0; i < template.Length - 1; i++)
		{
			var pair = (template[i], template[i + 1]);

			if (currentPolymer.ContainsKey(pair))
				currentPolymer[pair]++;
			else
				currentPolymer[pair] = 1;

			if (count.ContainsKey(template[i]))
				count[template[i]]++;
			else
				count[template[i]] = 1;
		}

		if (count.ContainsKey(template[^1]))
			count[template[^1]]++;
		else
			count[template[^1]] = 1;


		for (int steps = 0; steps < 40; steps++)
		{
			var newPolymer = currentPolymer.ToDictionary(x => x.Key, x => x.Value);
			foreach (var polymer in currentPolymer)
			{
				if (count.ContainsKey(rules[polymer.Key]))
					count[rules[polymer.Key]] += polymer.Value;
				else
					count[rules[polymer.Key]] = polymer.Value;

				new List<(char f, char s)>
				{
					(polymer.Key.f, rules[polymer.Key]),
					(rules[polymer.Key], polymer.Key.s)
				}.ForEach(x =>
				{
					if (newPolymer.ContainsKey(x))
						newPolymer[x] += polymer.Value;
					else
						newPolymer[x] = polymer.Value;
				});

				newPolymer[polymer.Key] -= polymer.Value;
				if (newPolymer[polymer.Key] <= 0)
					newPolymer.Remove(polymer.Key);
			}

			currentPolymer = newPolymer.ToDictionary(x => x.Key, x => x.Value);
		}

		return count.MaxBy(x => x.Value).Value - count.MinBy(x => x.Value).Value;
	}
}