using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._13
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day13 : ISolution
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
			var guests = GetGuests(enumerable).ToArray();
			var units = GetUnits(enumerable);
			var permutations = GetPer(guests);
			return permutations.Select(r => Compute(r, units)).Max().ToString();
		}

		private static int Compute(IEnumerable<string> list, 
			IReadOnlyDictionary<string, Dictionary<string, int>> dictionary)
		{
			var sum = 0;
			var enumerable = list as string[] ?? list.ToArray();
			for (var i = 0; i < enumerable.Length; i++)
			{
				if (i == 0)
					sum += dictionary[enumerable[i]][enumerable.Last()];
				else
					sum += dictionary[enumerable[i]][enumerable[i - 1]];

				if (i == enumerable.Length - 1)
					sum += dictionary[enumerable[i]][enumerable.First()];
				else
					sum += dictionary[enumerable[i]][enumerable[i + 1]];
			}

			return sum;
		}

		private static IEnumerable<string> GetGuests(IEnumerable<string> lines)
		{
			var res = new List<string>();
			foreach (var line in lines)
			{
				var splitted = line.Split(" ");
				res.Add(splitted.First());
				res.Add(splitted.Last().Replace(".", string.Empty));
			}

			return res.Distinct();
		}
		
		private static Dictionary<string, Dictionary<string, int>> GetUnits(IEnumerable<string> lines, bool withMe = false)
		{
			var res = new Dictionary<string, Dictionary<string, int>>();
			foreach (var line in lines)
			{
				var splitted = line.Split(" ");
				var firstGuest = splitted.First();
				if (!res.ContainsKey(firstGuest))
				{
					res[firstGuest] = new Dictionary<string, int>();
				}

				var secondGuest = splitted.Last().Replace(".", string.Empty);
				if (!res.ContainsKey(secondGuest))
				{
					res[secondGuest] = new Dictionary<string, int>();
				}

				res[firstGuest][secondGuest] =
					splitted[2] == "lose"
						? int.Parse(splitted[3]) * -1
						: int.Parse(splitted[3]);
				if (withMe)
					res[firstGuest]["Me"] = 0;
			}

			return res;
		}

		public string Part2(IEnumerable<string> lines)
		{
			var enumerable = lines as string[] ?? lines.ToArray();
			var guests = GetGuests(enumerable).ToList();
			var units = GetUnits(enumerable, true);
			units["Me"] = new Dictionary<string, int>();
			foreach (var guest in guests)
			{
				units["Me"][guest] = 0;
			}
			guests.Add("Me");
			var permutations = GetPer(guests.ToArray());
			return permutations.Select(r => Compute(r, units)).Max().ToString();
		}
	}
}