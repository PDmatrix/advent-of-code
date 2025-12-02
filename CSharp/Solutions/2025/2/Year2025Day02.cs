using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._2;

[UsedImplicitly]
public class Year2025Day02 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		long answer = 0;
		foreach (var range in input.First().Split(','))
		{
			var start = long.Parse(range.Split('-').First());
			var end = long.Parse(range.Split('-').Last());

			for (var i = start; i <= end; i++)
			{
				if (!IsInvalidHalf(i))
					continue;

				answer += i;
			}
		}

		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		long answer = 0;
		foreach (var range in input.First().Split(','))
		{
			var start = long.Parse(range.Split('-').First());
			var end = long.Parse(range.Split('-').Last());

			for (var i = start; i <= end; i++)
			{
				if (!IsInvalidRepeated(i))
					continue;

				answer += i;
			}
		}

		return answer;
	}

	private static bool IsInvalidHalf(long number)
	{
		var numStr = number.ToString();
		if (numStr.Length % 2 != 0)
			return false;

		var half = numStr.Length / 2;
		var right = numStr[half..];
		var left = numStr[..half];
		return right == left;
	}

	private static bool IsInvalidRepeated(long number)
	{
		var numStr = number.ToString();
		for (var i = 1; i <= numStr.Length / 2; i++)
		{
			if (numStr.Length % i == 1) continue;
			var left = numStr[..i];
			var valid = true;
			for (var j = i; j < numStr.Length; j += i)
			{
				if (Substr(numStr, j, i) == left)
					continue;

				valid = false;
				break;
			}

			if (valid)
				return true;
		}

		return false;
	}

	// Helper to avoid out of range exceptions
	private static string Substr(string s, int start, int len)
	{
		return start + len > s.Length ? s[start..] : s.Substring(start, len);
	}
}