using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._6
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day06 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var banks = input.First().Split('\t').Select(int.Parse).ToArray();
			var states = new List<string>();
			var idx = 1;
			while (true)
			{
				var nextState = GetNextState(banks);
				if (states.Contains(nextState))
					return idx.ToString();
				states.Insert(0, nextState);
				idx++;
			}
		}

		public string Part2(IEnumerable<string> input)
		{
			var banks = input.First().Split('\t').Select(int.Parse).ToArray();
			var states = new List<string>();
			while (true)
			{
				var nextState = GetNextState(banks);
				if (states.Contains(nextState))
					break;
				states.Insert(0, nextState);
			}

			var next = "";
			var idx = 0;
			var old = string.Join(',', banks);
			while (next != old)
			{
				next = GetNextState(banks);
				idx++;
			}

			return idx.ToString();
		}

		private static string GetNextState(IList<int> banks)
		{
			var maxValue = banks.Max();
			var maxIndex = banks.IndexOf(maxValue);
			banks[maxIndex] = 0;
			while (maxValue > 0)
			{
				maxIndex++;
				banks[maxIndex % banks.Count] += 1;
				maxValue--;
			}

			return string.Join(',', banks);
		}
	}
}