using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._4;

[UsedImplicitly]
public class Year2019Day04 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var arr = input.First().Split('-').Select(int.Parse).ToArray();
		var min = arr[0];
		var max = arr[1];

		// min = 123789;
		// max = 123789;
		var count = 0;
		for (var i = min; i <= max; i++)
		{
			var digitArr = DigitArr2(i).ToArray();
			var lastDigit = digitArr.First();
			var decreaseCondition = true;
			var doubleCondition = false;
			for (var j = 1; j < digitArr.Length; j++)
			{
				if (lastDigit > digitArr[j])
					decreaseCondition = false;

				if (lastDigit == digitArr[j])
					doubleCondition = true;

				lastDigit = digitArr[j];
			}

			if (decreaseCondition && doubleCondition)
				count++;
		}

		return count;
	}

	private static IEnumerable<int> DigitArr2(int n)
	{
		var result = new int[6];
		for (var i = result.Length - 1; i >= 0; i--)
		{
			result[i] = n % 10;
			n /= 10;
		}

		return result;
	}

	public object Part2(IEnumerable<string> input)
	{
		var arr = input.First().Split('-').Select(int.Parse).ToArray();
		var min = arr[0];
		var max = arr[1];

		// min = 266789;
		// max = 266789;
		var count = 0;
		for (var i = min; i <= max; i++)
		{
			var digitArr = DigitArr2(i).ToArray();
			var lastDigit = digitArr.First();
			var decreaseCondition = true;
			var doubleCondition = false;
			var dict = new Dictionary<int, int>();
			for (var j = 1; j < digitArr.Length; j++)
			{
				if (lastDigit > digitArr[j])
					decreaseCondition = false;

				if (lastDigit == digitArr[j])
				{
					doubleCondition = true;
					if (dict.ContainsKey(lastDigit))
						dict[lastDigit]++;
					else
						dict.Add(lastDigit, 1);
				}

				lastDigit = digitArr[j];
			}

			if (decreaseCondition && doubleCondition && dict.Values.Any(x => x == 1))
				count++;
		}

		return count;
	}
}