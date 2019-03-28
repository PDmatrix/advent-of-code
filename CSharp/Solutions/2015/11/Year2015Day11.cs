using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._11
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day11 : ISolution
	{
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
            while (!IsValid(input))
                input = Increment(input);

            return input;
        }

        private static bool IsValid(string input)
        {
            return Increasing(input) && NotContaining(input) && HasAtLeastTwoDifferentPairs(input);
        }

        private static bool Increasing(string input)
        {
            for (var i = 0; i < input.Length - 2; i++)
            {
                var firstAndSecond = input[i] - input[i + 1] == -1;
                var secondAndThird = input[i + 1] - input[i + 2] == -1;
                if (firstAndSecond && secondAndThird)
                    return true;
            }

            return false;
        }

        private static bool NotContaining(string input)
        {
            var badChars = new[] {'l', 'o', 'i'};
            return !badChars.Any(input.Contains);
        }

        private static bool HasAtLeastTwoDifferentPairs(string input)
        {
            var pairs = new List<string>();
            for (var i = 0; i < input.Length - 1; i++)
            {
                pairs.Add( string.Concat(input[i], input[i + 1]));
            }

            return pairs.Distinct().Count(r => r[0] == r[1]) >= 2;
        }

        private static string Increment(string s)
        {
            if (s == null || (s.Length == 0))
                return "a";

            var lastChar = s[s.Length - 1];
            var fragment = s.Substring(0, s.Length - 1);
            if (lastChar >= 'z')
                return Increment(fragment) + 'a';

            ++lastChar;
            return fragment + lastChar;
        }

        public string Part2(IEnumerable<string> lines)
		{
            var input = GetInput(lines);
            while (!IsValid(input))
                input = Increment(input);

            input = Increment(input);
            while (!IsValid(input))
                input = Increment(input);
            return input;
        }
	}
}