using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._9
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day09 : ISolution
	{
		private static void Swap(ref string a, ref string b)
		{
			if (a == b) 
				return;
			
			var temp = a;
			a = b;
			b = temp;
		}

		private static IEnumerable<List<string>> GetPer(string[] list)
		{
			var x = list.Length - 1;
			return GetPer(list, 0, x);
		}

		private static IEnumerable<List<string>> GetPer(string[] list, int k, int m)
		{
			var res = new List<List<string>>();
			if (k == m)
			{
				res.Add(list.ToList());
			}
			else
				for (var i = k; i <= m; i++)
				{
					Swap(ref list[k], ref list[i]);
					var permutations = GetPer(list, k + 1, m);
					res = res.Union(permutations).ToList();
					Swap(ref list[k], ref list[i]);
				}

			return res;
		}
		
		public string Part1(IEnumerable<string> lines)
		{
			var enumerable = lines as string[] ?? lines.ToArray();
			var cities = GetCities(enumerable).ToArray();
			var routes = GetRoutes(enumerable);
			var permutations = GetPer(cities);
			return permutations.Select(r => Compute(r, routes)).Min().ToString();
		}

		private static int Compute(IEnumerable<string> list, 
			IReadOnlyDictionary<string, Dictionary<string, int>> dictionary)
		{
			var sum = 0;
			var enumerable = list as string[] ?? list.ToArray();
			for (var i = 0; i < enumerable.Length - 1; i++)
			{
				sum += dictionary[enumerable[i]][enumerable[i + 1]];
			}

			return sum;
		}

		private static IEnumerable<string> GetCities(IEnumerable<string> lines)
		{
			var res = new List<string>();
			foreach (var line in lines)
			{
				var twoCities = line.Split("to").Select(r => r.Trim());
				var enumerable = twoCities as string[] ?? twoCities.ToArray();
				res.Add(enumerable.First());
				res.Add(enumerable.Last().Split("=").First().Trim());
			}

			return res.Distinct();
		}
		
		private static Dictionary<string, Dictionary<string, int>> GetRoutes(IEnumerable<string> lines)
		{
			var res = new Dictionary<string, Dictionary<string, int>>();
			foreach (var line in lines)
			{
				var twoCities = line.Split("to").Select(r => r.Trim());
				var enumerable = twoCities as string[] ?? twoCities.ToArray();
				var firstCity = enumerable.First();
				var secondCity = enumerable.Last().Split('=')[0].Trim();
				var distance = int.Parse(enumerable.Last().Split('=')[1].Trim());
				if (!res.ContainsKey(firstCity))
				{
					res[firstCity] = new Dictionary<string, int>();
				}
				if (!res.ContainsKey(secondCity))
				{
					res[secondCity] = new Dictionary<string, int>();
				}
	
				res[firstCity][secondCity] = distance;
				res[secondCity][firstCity] = distance;
			}

			return res;
		}

		public string Part2(IEnumerable<string> lines)
		{
			var enumerable = lines as string[] ?? lines.ToArray();
			var cities = GetCities(enumerable).ToArray();
			var routes = GetRoutes(enumerable);
			var permutations = GetPer(cities);
			return permutations.Select(r => Compute(r, routes)).Max().ToString();
		}
	}
}