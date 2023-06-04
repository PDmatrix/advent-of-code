using System;
using System.Collections.Generic;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._2;

[UsedImplicitly]
public class Year2022Day02 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var result = 0;
		foreach (var line in input)
		{
			var split = line.Split();
			var f = split[0][0];
			var s = split[1][0];

			var outcome = s - 23 - f;

			result += outcome switch
			{
				-2 => 6,
				-1 => 0,
				0 => 3,
				1 => 6,
				2 => 0,
				_ => throw new Exception($"Got {outcome}. {f} and {s}")
			};

			result += s - 87;

		}
		
		return result;
	}

	public object Part2(IEnumerable<string> input)
	{
		var result = 0;
		foreach (var line in input)
		{
			var split = line.Split();
			var f = split[0][0];
			var s = split[1][0];

			result += s switch
			{
				'X' => 0,
				'Y' => 3,
				'Z' => 6
			};
			
			result += s switch
			{
				'X' => (f + 2) % 65 % 3 + 1,
				'Y' => f - 64,
				'Z' => (f + 1) % 65 % 3 + 1
			};
		}
		
		return result;
	}
}