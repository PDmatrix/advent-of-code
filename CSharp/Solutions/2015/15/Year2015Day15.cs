using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2015._15;

[UsedImplicitly]
public class Year2015Day15 : ISolution
{
	private static Dictionary<string, Dictionary<string, int>> GetIngredients(IEnumerable<string> lines)
	{
		var ingredients = new Dictionary<string, Dictionary<string, int>>();
		foreach (var line in lines)
		{
			var splitted = line.Split(":");
			var name = splitted.First();
			var matches = Regex.Matches(splitted.Last(), @"-?\d");

			ingredients[name] = new Dictionary<string, int>
			{
				{ "capacity", int.Parse(matches[0].Value) },
				{ "durability", int.Parse(matches[1].Value) },
				{ "flavor", int.Parse(matches[2].Value) },
				{ "texture", int.Parse(matches[3].Value) },
				{ "calories", int.Parse(matches[4].Value) },
			};
		}

		return ingredients;
	}

	public object Part1(IEnumerable<string> lines)
	{
		return GetMaxCookieScore(lines);
	}

	private static long GetMaxCookieScore(IEnumerable<string> lines, bool withCalories = false)
	{
		var ingredients = GetIngredients(lines);
		var res = new List<long>();
		for (var sprinkles = 1; sprinkles < 100; sprinkles++)
		{
			for (var butterscotch = 1; butterscotch < 100 - sprinkles; butterscotch++)
			{
				for (var chocolate = 1; chocolate < 100 - sprinkles - butterscotch; chocolate++)
				{
					var candy = 100 - sprinkles - butterscotch - chocolate;
					var currentIngredients = new Dictionary<string, int>
					{
						{ "Sprinkles", sprinkles },
						{ "Butterscotch", butterscotch },
						{ "Chocolate", chocolate },
						{ "Candy", candy }
					};
					res.Add(MakeCookie(currentIngredients, ingredients, withCalories));
				}
			}
		}

		return res.Max();
	}

	private static long MakeCookie(Dictionary<string, int> currentIngredients,
		IReadOnlyDictionary<string, Dictionary<string, int>> allIngredients, bool withCalories = false)
	{
		var propertiesList = new List<string>
		{
			"calories",
			"capacity",
			"durability",
			"flavor",
			"texture"
		};

		long res = 1;
		foreach (var property in propertiesList)
		{
			var intermediate = currentIngredients.Aggregate<KeyValuePair<string, int>, long>(0,
				(current, kv) => current + allIngredients[kv.Key][property] * kv.Value);

			intermediate = intermediate > 0 ? intermediate : 0;
			if (withCalories && property == "calories" && intermediate != 500)
				return 0;
			if (property == "calories")
				continue;

			res *= intermediate;
		}

		return res <= 0 ? 0 : res;
	}

	public object Part2(IEnumerable<string> lines)
	{
		return GetMaxCookieScore(lines, true);
	}
}