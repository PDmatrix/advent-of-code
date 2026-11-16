using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._21;

[UsedImplicitly]
public class Year2020Day21 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (allergens, ingredientsSet) = ParseInput(input);

		return ingredientsSet.Where(ing => !allergens.Any(x => x.Value.Contains(ing.Key))).Sum(ing => ing.Value);
	}

	public object Part2(IEnumerable<string> input)
	{
		var (allergens, _) = ParseInput(input);

		var canonical = new SortedDictionary<string, string>();
		while (canonical.Count != allergens.Count)
		{
			var known = allergens.First(x => x.Value.Count == 1);
			var value = known.Value.Single();
			canonical[known.Key] = value;
			foreach (var allergen in allergens)
			{
				allergen.Value.Remove(value);
			}
		}
		
		return string.Join(',', canonical.Values);
	}
	
	private static (Dictionary<string, HashSet<string>> allergens, Dictionary<string, int> ingredients) ParseInput(IEnumerable<string> input)
	{
		var allergens = new Dictionary<string, HashSet<string>>();
		var ingredientsSet = new Dictionary<string, int>();
		foreach (var line in input)
		{
			var split = line.Split(" (");
			var ingredients = split[0].Split().ToHashSet();
			foreach (var ingredient in ingredients)
			{
				if (ingredientsSet.ContainsKey(ingredient))
					ingredientsSet[ingredient]++;
				else
					ingredientsSet[ingredient] = 1;
			}

			var contains = split[1].Replace(")", string.Empty).Replace("contains ", string.Empty).Split(", ")
				.ToArray();
			foreach (var c in contains)
			{
				if (allergens.ContainsKey(c))
				{
					var newIng = new HashSet<string>(ingredients);
					newIng.IntersectWith(allergens[c]);
					allergens[c] = newIng;
				}
				else
				{
					allergens[c] = ingredients;
				}
			}
		}

		return (allergens, ingredientsSet);
	}
}