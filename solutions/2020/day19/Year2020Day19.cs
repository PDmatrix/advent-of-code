using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._19;

public static class Utils
{
	public static void Deconstruct(this string[] s, out string first, out string second)
	{
		first = s.First();
		second = s.Last();
	}
}

[UsedImplicitly]
public class Year2020Day19 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var rules = ParseInput(input);

		var regex = new Regex("^" + GenerateRegex(rules) + "$", RegexOptions.Compiled);
		return input.SkipWhile(x => !string.IsNullOrEmpty(x)).Skip(1).Count(line => regex.IsMatch(line));
	}
	
	public object Part2(IEnumerable<string> input)
	{
		var rules = ParseInput(input);

		rules["8"] = new List<List<string>>
		{
			new() { "42" },
			new() { "42", "8" },
		};
		rules["11"] = new List<List<string>>
		{
			new() { "42", "31" },
			new() { "42", "11", "31" }
		};

		var regex = new Regex("^" + GenerateRegex(rules) + "$", RegexOptions.Compiled);
		return input.SkipWhile(x => !string.IsNullOrEmpty(x)).Skip(1).Count(line => regex.IsMatch(line));
	}

	private static Dictionary<string, List<List<string>>> ParseInput(IEnumerable<string> input)
	{
		var rules = new Dictionary<string, List<List<string>>>();
		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
				break;

			var (num, r) = line.Split(": ");
			rules[num] = r.Split(" | ").Select(s => s.Split().ToList()).ToList();
		}

		return rules;
	}

	private static string GenerateRegex(Dictionary<string, List<List<string>>> rules, string r = "0", int depth = 25)
	{
		if (depth == 0)
			return "";
		if (rules[r][0][0].StartsWith('"'))
			return rules[r][0][0].Trim('"');

		return "(" + string.Join('|', rules[r].Select(x => string.Join(string.Empty, x.Select(c => GenerateRegex(rules, c, depth - 1))))) + ")";
	}
}