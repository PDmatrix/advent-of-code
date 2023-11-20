using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2015._16;

[UsedImplicitly]
public class Year2015Day16 : ISolution
{
	public object Part1(IEnumerable<string> lines)
	{
		var aunts = GetAunts(lines);
		var etalonAunt = GetEtalonAunt();
		return FindRightAunt(aunts, etalonAunt);
	}

	private static IEnumerable<Dictionary<string, int>> GetAunts(IEnumerable<string> lines)
	{
		foreach (var line in lines)
		{
			var splitted = line.Split().Select(r =>
				r.Replace(",", string.Empty)
					.Replace(":", string.Empty)).ToArray();
			yield return new Dictionary<string, int>
			{
				{ "id", int.Parse(splitted[1]) },
				{ splitted[2], int.Parse(splitted[3]) },
				{ splitted[4], int.Parse(splitted[5]) },
				{ splitted[6], int.Parse(splitted[7]) }
			};
		}
	}

	private static Dictionary<string, int> GetEtalonAunt()
	{
		return new Dictionary<string, int>
		{
			{"children", 3},
			{"cats", 7},
			{"samoyeds", 2},
			{"pomeranians", 3},
			{"akitas", 0},
			{"vizslas", 0},
			{"goldfish", 5},
			{"trees", 3},
			{"cars", 2},
			{"perfumes", 1}
		};
	}

	public object Part2(IEnumerable<string> lines)
	{
		var aunts = GetAunts(lines);
		var etalonAunt = GetEtalonAunt();
		return FindRightAunt(aunts, etalonAunt, true);
	}

	private static object FindRightAunt(IEnumerable<Dictionary<string, int>> aunts, IReadOnlyDictionary<string, int> etalonAunt, bool withFixedInstructions = false)
	{
		foreach (var aunt in aunts)
		{
			var match = MatchAunts(etalonAunt, withFixedInstructions, aunt);

			if (match)
				return aunt["id"].ToString();
		}

		return "error";
	}

	private static bool MatchAunts(IReadOnlyDictionary<string, int> etalonAunt, bool withFixedInstructions, Dictionary<string, int> aunts)
	{
		var match = true;
		foreach (var (key, value) in aunts)
		{
			if (key == "id")
				continue;

			if (withFixedInstructions && (key == "trees" || key == "cats"))
			{
				if (value <= etalonAunt[key])
				{
					match = false;
					break;
				}

				continue;
			}

			if (withFixedInstructions && (key == "pomeranians" || key == "goldfish"))
			{
				if (value >= etalonAunt[key])
				{
					match = false;
					break;
				}

				continue;
			}

			if (value == etalonAunt[key])
				continue;

			match = false;
			break;
		}

		return match;
	}
}