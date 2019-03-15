using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._12
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day12 : ISolution
	{
		// ReSharper disable once UnusedMember.Global
		public static int Year = 2015;
		// ReSharper disable once UnusedMember.Global
		public static int Day = 12;

        private static string GetInput(IEnumerable<string> lines)
        {
            var input = lines.FirstOrDefault();
            if (input == null)
                throw new Exception("Invalid input");

            return input;
        }

        public string Part1(IEnumerable<string> lines)
        {
            var input = GetInput(lines);
            var matches = Regex.Matches(input, @"-?\d+");
            var sum = 0;
            foreach (Match match in matches)
            {
                if (int.TryParse(match.Value, out var result))
                    sum += result;
                else
                    throw new Exception("Bad string");
            }
            return sum.ToString();
        }

        public string Part2(IEnumerable<string> lines)
		{
            var input = GetInput(lines);
            return "1";
        }
	}
}