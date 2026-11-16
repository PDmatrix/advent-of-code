using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Channels;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._25;

[UsedImplicitly]
public partial class Year2022Day25 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		return ToSnafu(input.Sum(FromSnafu));
	}

	public object Part2(IEnumerable<string> input)
	{
		return "Congratulations!";
	}
	
	private static long FromSnafu(string snafu)
	{
		long answer = 0;
		for (var i = snafu.Length - 1; i >= 0; i--)
		{
			var multiplier = (long)Math.Pow(5, i);
			answer += snafu[snafu.Length - 1 - i] switch
			{
				'-' => -1 * multiplier,
				'=' => -2 * multiplier,
				_ => long.Parse(snafu[snafu.Length - 1 - i].ToString()) * multiplier
			};
		}
		
		return answer;
	}
	
	
	private static string ToSnafu(long number)
	{
		var snafu = "";

		while (number != 0)
		{
			var place = (number + 2) % 5;
			number = (number + 2) / 5;
			snafu += "=-012"[(int)place];
		}

		return string.Join("", snafu.Reverse());
	}
}