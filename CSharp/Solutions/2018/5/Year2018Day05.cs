using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._5
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day05 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var polymers = new StringBuilder(input.First());

			return React(polymers).Length.ToString();
		}

		private static string React(StringBuilder polymers)
		{
			for (var i = 0; i < polymers.Length - 1; i++)
			{
				if (Math.Abs(polymers[i] - polymers[i + 1]) != 32)
					continue;

				polymers.Remove(i, 2);
				return React(polymers);
			}

			return polymers.ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var loverCase = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (char) i).ToArray();
			var upperCase = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char) i).ToArray();
			var polymer = input.First();
			var polymerLengths = new List<int>();
			for (var i = 0; i < loverCase.Length; i++)
			{
				var strippedPolymer = polymer.Replace(loverCase[i].ToString(), string.Empty)
					.Replace(upperCase[i].ToString(), string.Empty);
				polymerLengths.Add(React(new StringBuilder(strippedPolymer)).Length);
			}

			return polymerLengths.Min().ToString();
		}
	}
}