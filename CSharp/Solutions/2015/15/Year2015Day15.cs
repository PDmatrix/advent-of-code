using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2015._15;

[UsedImplicitly]
public class Year2015Day15 : ISolution
{
	private class Ingredient
	{
		public int Capacity { get; set; }
		public int Durability { get; set; }
		public int Flavor { get; set; }
		public int Texture { get; set; }
		public int Calories { get; set; }
	}

	private static Dictionary<string, Ingredient> GetIngredients(IEnumerable<string> lines)
	{
		var ingredients = new Dictionary<string, Ingredient>();
		foreach (var line in lines)
		{
			var splitted = line.Split(":");
			var name = splitted.First();
			var matches = Regex.Matches(splitted.Last(), @"-?\d");
			ingredients[name] = new Ingredient
			{
				Capacity = int.Parse(matches[0].Value),
				Durability = int.Parse(matches[1].Value),
				Flavor = int.Parse(matches[2].Value),
				Texture = int.Parse(matches[3].Value),
				Calories = int.Parse(matches[4].Value)
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
					res.Add(MakeCookie(sprinkles, butterscotch, chocolate, candy, ingredients, withCalories));
				}
			}
		}

		return res.Max();
	}

	private static long MakeCookie(int sprinkles, int butterscotch, int chocolate, int candy, IReadOnlyDictionary<string, Ingredient> ingredients, bool withCalories = false)
	{
		var sprinklesIngredient = ingredients["Sprinkles"];
		var butterscotchIngredient = ingredients["Butterscotch"];
		var chocolateIngredient = ingredients["Chocolate"];
		var candyIngredient = ingredients["Candy"];
		
		long calory = sprinkles * sprinklesIngredient.Calories
		              + butterscotch * butterscotchIngredient.Calories
		              + chocolate * chocolateIngredient.Calories
		              + candy * candyIngredient.Calories;
			
		if (withCalories && calory != 500)
			return 0;
		
		long flavor = sprinkles * sprinklesIngredient.Flavor
		              + butterscotch * butterscotchIngredient.Flavor
		              + chocolate * chocolateIngredient.Flavor
		              + candy * candyIngredient.Flavor;
		flavor = flavor > 0 ? flavor : 0;
			
		long capacity = sprinkles * sprinklesIngredient.Capacity
		                + butterscotch * butterscotchIngredient.Capacity
		                + chocolate * chocolateIngredient.Capacity
		                + candy * candyIngredient.Capacity;
		capacity = capacity > 0 ? capacity : 0;
			
		long durability = sprinkles * sprinklesIngredient.Durability
		                  + butterscotch * butterscotchIngredient.Durability
		                  + chocolate * chocolateIngredient.Durability
		                  + candy * candyIngredient.Durability;
		durability = durability > 0 ? durability : 0;
				
		long texture = sprinkles * sprinklesIngredient.Texture
		               + butterscotch * butterscotchIngredient.Texture
		               + chocolate * chocolateIngredient.Texture
		               + candy * candyIngredient.Texture;
		texture = texture > 0 ? texture : 0;
			
		var res = flavor * durability * texture * capacity;
		return res <= 0 ? 0 : res;
	}

	public object Part2(IEnumerable<string> lines)
	{
		return GetMaxCookieScore(lines, true);
	}
}