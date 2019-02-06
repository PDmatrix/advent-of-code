using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._1
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day01 : ISolution
	{
		// ReSharper disable once UnusedMember.Global
		public static int Year = 2015;
		// ReSharper disable once UnusedMember.Global
		public static int Day = 1;

		private static string Parse(IEnumerable<string> lines)
		{
			return lines.FirstOrDefault();
		}

		public string Part1(IEnumerable<string> lines)
		{
			var input = Parse(lines);
			var floor = 0;
			foreach (var direction in input)
				floor += direction == '(' ? 1 : -1;

			return floor.ToString();
		}

		public string Part2(IEnumerable<string> lines)
		{
			var input = Parse(lines);
			var floor = 0;
			for (var i = 0; i < input.Length; i++)
			{
				floor += input[i] == '(' ? 1 : -1;
				if (floor == -1)
					return (i + 1).ToString();
			}
			throw new Exception("Answer not found");
		}
	}
}