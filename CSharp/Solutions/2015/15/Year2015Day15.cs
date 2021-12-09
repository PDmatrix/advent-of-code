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
		public string Name { get; set; } = null!;
		public int Capacity { get; set; }
		public int Durability { get; set; }
		public int Flavor { get; set; }
		public int Texture { get; set; }
		public int Calories { get; set; }
	}

	private IEnumerable<Ingredient> GetIngredients(IEnumerable<string> lines)
	{
		// ReSharper disable once LoopCanBeConvertedToQuery
		foreach (var line in lines)
		{
			var splitted = line.Split(":");
			var name = splitted.First();
			var matches = Regex.Matches(splitted.Last(), @"-?\d");
			yield return new Ingredient
			{
				Name = name,
				Capacity = int.Parse(matches[0].Value),
				Durability = int.Parse(matches[1].Value),
				Flavor = int.Parse(matches[2].Value),
				Texture = int.Parse(matches[3].Value),
				Calories = int.Parse(matches[4].Value)
			};
		}
	}
		
	public object Part1(IEnumerable<string> lines)
	{
		var ingredients = GetIngredients(lines);
		var res = new List<long>();
		var enumerable = ingredients as Ingredient[] ?? ingredients.ToArray();
		for (var sprinkles = 1; sprinkles < 100; sprinkles++)
		{
			for (var butterscotch = 1; butterscotch < 100 - sprinkles; butterscotch++)
			{
				for (var chocolate = 1; chocolate < 100 - sprinkles - butterscotch; chocolate++)
				{
					var candy = 100 - sprinkles - butterscotch - chocolate;
					res.Add(MakeCookie(sprinkles, butterscotch, chocolate, candy, enumerable));
				}
			}
		}
		return res.Max().ToString();
	}

	private static long MakeCookie(int sprinkles, int butterscotch, int chocolate, int candy, IEnumerable<Ingredient> ingredients)
	{
		var enumerable = ingredients as Ingredient[] ?? ingredients.ToArray();
		var sprinklesIngredient = enumerable.First(r => r.Name == "Sprinkles");
		var butterscotchIngredient = enumerable.First(r => r.Name == "Butterscotch");
		var chocolateIngredient = enumerable.First(r => r.Name == "Chocolate");
		var candyIngredient = enumerable.First(r => r.Name == "Candy");
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
		var ingredients = GetIngredients(lines);
		var res = new List<long>();
		var enumerable = ingredients as Ingredient[] ?? ingredients.ToArray();
		for (var sprinkles = 1; sprinkles < 100; sprinkles++)
		{
			for (var butterscotch = 1; butterscotch < 100 - sprinkles; butterscotch++)
			{
				for (var chocolate = 1; chocolate < 100 - sprinkles - butterscotch; chocolate++)
				{
					var candy = 100 - sprinkles - butterscotch - chocolate;
					res.Add(MakeCookieWithCalories(sprinkles, butterscotch, chocolate, candy, enumerable));
				}
			}
		}
		return res.Max().ToString();
	}
		
	private static long MakeCookieWithCalories(int sprinkles, int butterscotch, int chocolate, int candy, IEnumerable<Ingredient> ingredients)
	{
		var enumerable = ingredients as Ingredient[] ?? ingredients.ToArray();
		var sprinklesIngredient = enumerable.First(r => r.Name == "Sprinkles");
		var butterscotchIngredient = enumerable.First(r => r.Name == "Butterscotch");
		var chocolateIngredient = enumerable.First(r => r.Name == "Chocolate");
		var candyIngredient = enumerable.First(r => r.Name == "Candy");
			
		long calory = sprinkles * sprinklesIngredient.Calories
		              + butterscotch * butterscotchIngredient.Calories
		              + chocolate * chocolateIngredient.Calories
		              + candy * candyIngredient.Calories;
			
		if (calory != 500)
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
}