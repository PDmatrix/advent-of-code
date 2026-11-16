using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._2;

[UsedImplicitly]
public class Year2020Day02 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var regex = new Regex(@"(?<from>\d+)-(?<to>\d+) (?<letter>\w): (?<pass>\w+)");
		var list = new List<(int from, int to, char letter, string password)>();
		foreach (var line in input)
		{
			var match = regex.Match(line);
			list.Add((int.Parse(match.Groups["from"].Value), int.Parse(match.Groups["to"].Value),
				match.Groups["letter"].Value[0], match.Groups["pass"].Value));
		}

		var valid = 0;
		foreach (var (from, to, letter, password) in list)
		{
			var count = password.Count(x => x == letter);
			if (count >= from && count <= to)
				valid++;
		}
		
		return valid;
	}

	public object Part2(IEnumerable<string> input)
	{
		var regex = new Regex(@"(?<from>\d+)-(?<to>\d+) (?<letter>\w): (?<pass>\w+)");
		var list = new List<(int from, int to, char letter, string password)>();
		foreach (var line in input)
		{
			var match = regex.Match(line);
			list.Add((int.Parse(match.Groups["from"].Value), int.Parse(match.Groups["to"].Value),
				match.Groups["letter"].Value[0], match.Groups["pass"].Value));
		}

		var valid = 0;
		foreach (var (from, to, letter, password) in list)
		{
			if ((password[from - 1] == letter && password[to - 1] != letter) ||
			    (password[from - 1] != letter && password[to - 1] == letter))
				valid++;
		}
		
		return valid;
	}
}