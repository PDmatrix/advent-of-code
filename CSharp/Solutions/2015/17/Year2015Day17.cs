using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._17
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day17 : ISolution
	{
		// ReSharper disable once UnusedMember.Global
		public static int Year = 2015;
		// ReSharper disable once UnusedMember.Global
		public static int Day = 17;
				
		public string Part1(IEnumerable<string> lines)
		{
			var numbers = lines.Select(int.Parse).ToArray();
			var res = SumUp(numbers, 150);
			return res.Count.ToString();
		}

		public string Part2(IEnumerable<string> lines)
		{
			var numbers = lines.Select(int.Parse).ToArray();
			var res = SumUp(numbers, 150);
			var min = res.Select(r => r.Count).Min();
			return res.Count(r => r.Count == min).ToString();
		}
		
		private static List<List<int>> SumUp(IReadOnlyList<int> numbers, int target)
		{
			return SumUpRecursive(numbers, target, new List<int>());
		}

		private static List<List<int>> SumUpRecursive(IReadOnlyList<int> numbers, int target, List<int> partial)
		{
			var res = new List<List<int>>();
			var s = 0;
			foreach (var x in partial) 
				s += x;

			if (s == target)
			{
				res.Add(partial);
			}

			if (s >= target)
				return res;

			for (var i = 0; i < numbers.Count; i++)
			{
				var remaining = new List<int>();
				var n = numbers[i];
				for (var j = i + 1; j < numbers.Count; j++) 
					remaining.Add(numbers[j]);

				var partialRec = new List<int>(partial) {n};
				var rs = SumUpRecursive(remaining, target, partialRec);
				res = res.Union(rs).ToList();
			}

			return res;
		}
	}
}