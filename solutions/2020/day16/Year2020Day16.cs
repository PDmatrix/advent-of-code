using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._16;

[UsedImplicitly]
public class Year2020Day16 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (rules, _, tickets) = ParseInput(input);
		
		return tickets.Sum(ticket => ticket.Where(t => !rules.Any(x => x.Value.Contains(t))).Sum());
	}

	public object Part2(IEnumerable<string> input)
	{
		var (rules, myTicket, tickets) = ParseInput(input);

		var validTickets = GetValidTickets(tickets, rules);
		var possibilities = GetPossibilities(rules, validTickets);
		var positions = GetPositions(possibilities);

		return myTicket.Where((_, i) => positions[i].StartsWith("departure")).Aggregate<int, long>(1, (current, t) => current * t);
	}

	private static (Dictionary<string, HashSet<int>>, List<int>, List<List<int>>) ParseInput(IEnumerable<string> input)
	{
		var enumerable = input as string[] ?? input.ToArray();
		var rules = ParseRules(enumerable.TakeWhile(x => !string.IsNullOrWhiteSpace(x)));
		var myTicket = ParseMyTicket(enumerable.SkipWhile(x => !string.IsNullOrWhiteSpace(x)).Skip(2)
			.TakeWhile(x => !string.IsNullOrWhiteSpace(x)));
		var tickets = ParseNearbyTickets(enumerable.SkipWhile(x => !string.IsNullOrWhiteSpace(x)).Skip(2)
			.SkipWhile(x => !string.IsNullOrWhiteSpace(x)).Skip(2));

		return (rules, myTicket, tickets);
	}


	private static Dictionary<string, HashSet<int>> ParseRules(IEnumerable<string> input)
	{
		var regex = new Regex(
			@"(?<name>\w+\s?\w+?): (?<firstFrom>\d+)-(?<firstTo>\d+) or (?<secondFrom>\d+)-(?<secondTo>\d+)",
			RegexOptions.Compiled);
		var rules = new Dictionary<string, HashSet<int>>();

		foreach (var line in input)
		{
			var match = regex.Match(line);
			var name = match.Groups["name"].Value;
			var firstFrom = int.Parse(match.Groups["firstFrom"].Value);
			var firstTo = int.Parse(match.Groups["firstTo"].Value);
			var secondFrom = int.Parse(match.Groups["secondFrom"].Value);
			var secondTo = int.Parse(match.Groups["secondTo"].Value);

			var rule = new HashSet<int>();
			for (var i = firstFrom; i <= firstTo; i++)
				rule.Add(i);

			for (var i = secondFrom; i <= secondTo; i++)
				rule.Add(i);

			rules[name] = rule;
		}

		return rules;
	}
	
	private static List<int> ParseMyTicket(IEnumerable<string> input)
	{
		return input.First().Split(',').Select(int.Parse).ToList();
	}
	
	private static List<List<int>> ParseNearbyTickets(IEnumerable<string> input)
	{
		return input.Select(line => line.Split(',').Select(int.Parse).ToList()).ToList();
	}

	private static Dictionary<int, string> GetPositions(Dictionary<string, HashSet<int>> possibilities)
	{
		var positions = new Dictionary<int, string>();
		while (positions.Count != possibilities.Count)
		{
			var kv = possibilities.Single(x => x.Value.Count == 1);
			var index = kv.Value.First();
			foreach (var possibility in possibilities)
				possibility.Value.Remove(index);
			positions[index] = kv.Key;
		}

		return positions;
	}

	private static Dictionary<string, HashSet<int>> GetPossibilities(Dictionary<string, HashSet<int>> rulesWithName,
		List<List<int>> validTickets)
	{
		var possibilities = new Dictionary<string, HashSet<int>>();
		foreach (var kv in rulesWithName)
		{
			for (var i = 0; i < validTickets.First().Count; i++)
			{
				var isValid = validTickets.All(ticket => kv.Value.Contains(ticket[i]));

				if (!isValid) 
					continue;
				
				if (possibilities.ContainsKey(kv.Key))
					possibilities[kv.Key].Add(i);
				else
					possibilities[kv.Key] = new HashSet<int> { i };
			}
		}

		return possibilities;
	}

	private static List<List<int>> GetValidTickets(List<List<int>> tickets,
		Dictionary<string, HashSet<int>> rulesWithName)
	{
		var validTickets = new List<List<int>>();
		foreach (var ticket in tickets)
		{
			var isValid = ticket.All(t => rulesWithName.Any(x => x.Value.Contains(t)));

			if (isValid)
				validTickets.Add(ticket);
		}

		return validTickets;
	}
}