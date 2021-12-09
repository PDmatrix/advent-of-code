using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2015._19;

[UsedImplicitly]
public class Year2015Day19 : ISolution
{
	private class Replacement
	{
		public string From { get; set; } = null!;
		public string To { get; set; } = null!;
	}
		
	private static IEnumerable<Replacement> GetReplacements(IEnumerable<string> lines)
	{
		return lines
			.TakeWhile(line => line.IndexOf(" => ", StringComparison.Ordinal) != -1)
			.Select(line => line.Split(" => "))
			.Select(splitted => new Replacement
			{
				From = splitted.First(),
				To = splitted.Last()
			});
	}
		
	public object Part1(IEnumerable<string> lines)
	{
		var enumerable = lines as string[] ?? lines.ToArray();
		var replacements = GetReplacements(enumerable);
		var value = enumerable.Last();
		var count = CountDistinctMolecules(replacements, value);
		return count.ToString();
	}
		
	private static IEnumerable<int> AllIndexesOf(string str, string searchString)
	{
		var minIndex = str.IndexOf(searchString, StringComparison.Ordinal);
		while (minIndex != -1)
		{
			yield return minIndex;
			minIndex = str.IndexOf(searchString, minIndex + searchString.Length, StringComparison.Ordinal);
		}
	}
		
	private static string Replace(string text, int index, int length, string replace)
	{
		return text.Substring(0, index) + replace + text.Substring(index + length);
	}
		
	private static int CountDistinctMolecules(IEnumerable<Replacement> replacements, string value)
	{
		var molecules = new HashSet<string>();
		foreach (var replacement in replacements)
		{
			var indexes = AllIndexesOf(value, replacement.From);
			foreach (var index in indexes)
			{
				molecules.Add(Replace(value, index, replacement.From.Length, replacement.To));
			}
		}

		return molecules.Count;
	}

	public object Part2(IEnumerable<string> lines)
	{
			
		var molecule = lines.Last();
		// Hacky solution
		// https://www.reddit.com/r/adventofcode/comments/3xflz8/day_19_solutions/cy4h7ji?utm_source=share&utm_medium=web2x
		int CountStr(string x)
		{
			var count = 0;
			for (var index = molecule.IndexOf(x, StringComparison.Ordinal);
			     index >= 0;
			     index = molecule.IndexOf(x, index + 1, StringComparison.Ordinal), ++count)
			{
			}

			return count;
		}
		var num = molecule.Count(char.IsUpper) - CountStr("Rn") - CountStr("Ar") - 2 * CountStr("Y") - 1;
		return num.ToString();
	}
}