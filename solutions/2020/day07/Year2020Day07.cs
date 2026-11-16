using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._7;

[UsedImplicitly]
public class Year2020Day07 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var rules = new Dictionary<string, List<string>>();
		foreach (var line in input)
		{
			var bags = new List<string>();
			var split = line.Split(" contain ");
			var bagType = split[0].TrimEnd('s');
			var required = split[1].Split(", ");
			if (required[0] != "no other bags.")
				bags = required.Select(req => string.Join(string.Empty, req.Skip(2)).TrimEnd('.').TrimEnd('s')).ToList();

			rules.Add(bagType, bags);
		}

		const string myBag = "shiny gold bag";
		var visited = new HashSet<string>();
		var answer = 0;
		foreach (var rule in rules.Where(x => x.Value.Count != 0))
		{
			if (visited.Contains(rule.Key))
			{
				answer++;
				continue;
			}

			var queue = new Queue<string>();
			foreach (var bag in rule.Value)
				queue.Enqueue(bag);

			var found = false;
			while (queue.Count > 0)
			{
				var current = queue.Dequeue();
				if (current == myBag)
				{
					found = true;
					answer++;
					break;
				}

				foreach (var bag in rules[current])
					queue.Enqueue(bag);
			}
			
			if (found)
				visited.Add(rule.Key);
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var rules = new Dictionary<string, List<(int numberOfBugs, string bug)>>();
		foreach (var line in input)
		{
			var bags = new List<(int numberOfBugs, string bug)>();
			var split = line.Split(" contain ");
			var bagType = split[0].TrimEnd('s');
			var required = split[1].Split(", ");
			if (required[0] != "no other bags.")
			{
				foreach (var req in required)
				{
					var num = int.Parse(req[0].ToString());
					var bag = string.Join(string.Empty, req.Skip(2)).TrimEnd('.').TrimEnd('s');
					bags.Add((num, bag));
				}
			}

			rules.Add(bagType, bags);
		}

		const string myBag = "shiny gold bag";
		var answer = 0;
		var queue = new Queue<(int num, string bag)>();
		foreach (var bag in rules[myBag])
			queue.Enqueue(bag);

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			answer += current.num;
			foreach (var next in rules[current.bag])
				queue.Enqueue((next.numberOfBugs * current.num, next.bug));
		}

		return answer;
	}
}