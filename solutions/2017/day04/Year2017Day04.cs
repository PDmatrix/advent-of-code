using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2017._4;

[UsedImplicitly]
public class Year2017Day04 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		return input.Select(UniqueWords).Sum().ToString();
	}

	private static int UniqueWords(string value)
	{
		var arrayOfWords = value.Split();
		return arrayOfWords.Length == arrayOfWords.Distinct().Count() ? 1 : 0;
	}

	public object Part2(IEnumerable<string> input)
	{
		return input.Select(UniqueNonRearrangeWords).Sum().ToString();
	}

	private static int UniqueNonRearrangeWords(string value)
	{
		var arrayOfWords = value.Split().Select(x => string.Concat(x.OrderBy(c => c))).ToArray();
		return arrayOfWords.Length == arrayOfWords.Distinct().Count() ? 1 : 0;
	}
}