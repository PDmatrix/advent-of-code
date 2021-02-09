using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._5
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day05 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			bool HasThreeVowels(string str) => 
				Regex.Matches(str, @"[aeiou]").Count >= 3;

			bool HasDoubledLetter(string str) => 
				Regex.IsMatch(str, @"(\w)\1");

			bool ContainsNaughtyStrings(string str) => 
				Regex.IsMatch(str, @"ab|cd|pq|xy");

			bool IsNiceString(string str) =>
				HasThreeVowels(str)
				&& HasDoubledLetter(str)
				&& !ContainsNaughtyStrings(str);
			
			var niceStrings = input.Count(IsNiceString);
			return niceStrings.ToString();
		}

		public object Part2(IEnumerable<string> input)
		{
			bool HasPair(string str) => 
				Regex.IsMatch(str, @"(\w{2}).*\1");
			
			bool HasDuplicate(string str) => 
				Regex.IsMatch(str, @"(\w).\1");

			bool IsNiceString(string str) =>
				HasDuplicate(str)
				&& HasPair(str);
			
			var niceStrings = input.Count(IsNiceString);
			return niceStrings.ToString();
		}
	}
}