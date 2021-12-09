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
		var etallonAunt = new Dictionary<string, int>
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
		foreach (var aunt in aunts)
		{
			var match = true;
			foreach (var (key, value) in aunt)
			{
				if(key == "id")
					continue;

				if (value == etallonAunt[key]) 
					continue;
					
				match = false;
				break;
			}

			if (match)
				return aunt["id"].ToString();
		}
		return "error";
	}

	private static IEnumerable<Dictionary<string, int>> GetAunts(IEnumerable<string> lines)
	{
		var res = new List<Dictionary<string, int>>();
		foreach (var line in lines)
		{
			var splitted = line.Split().Select(r =>
				r.Replace(",", string.Empty)
					.Replace(":", string.Empty)).ToArray();
			res.Add(new Dictionary<string, int>
			{
				{"id", int.Parse(splitted[1])},
				{splitted[2], int.Parse(splitted[3])},
				{splitted[4], int.Parse(splitted[5])},
				{splitted[6], int.Parse(splitted[7])}
			});
				
		}

		return res;
	}

	public object Part2(IEnumerable<string> lines)
	{
		var aunts = GetAunts(lines);
		var etallonAunt = new Dictionary<string, int>
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
		foreach (var aunt in aunts)
		{
			var match = true;
			foreach (var (key, value) in aunt)
			{
				if(key == "id")
					continue;
					
				if (key == "trees" || key == "cats")
				{
					if (value <= etallonAunt[key])
					{
						match = false;
						break;
					}
					continue;
				}
					
				if (key == "pomeranians" || key == "goldfish")
				{
					if (value >= etallonAunt[key])
					{
						match = false;
						break;
					}

					continue;
				}
					
				if (value == etallonAunt[key]) 
					continue;
					
				match = false;
				break;
			}

			if (match)
				return aunt["id"].ToString();
		}
		return "error";
	}
}