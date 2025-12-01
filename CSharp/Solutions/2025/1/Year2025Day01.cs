using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._1;

[UsedImplicitly]
public class Year2025Day01 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var answer = 0;
		var current = 50;
		foreach (var line in input)
		{
			var d = line.First();
			var value = int.Parse(line[1..]);
			current = d switch
			{
				'L' => (current + value) % 100,
				'R' => (current - value) % 100,
				_ => throw new ArgumentOutOfRangeException()
			};

			if (current == 0)
				answer++;
		}

		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var answer = 0;
		var current = 50;
		foreach (var line in input)
		{
			var d = line.First();
			var value = int.Parse(line[1..]);
			for (var i = 0; i < value; i++)
			{
				current = d switch
				{
					'L' => (current + 1) % 100,
					'R' => (current - 1) % 100,
					_ => throw new ArgumentOutOfRangeException()
				};
				if (current == 0)
					answer++;
			}
		}

		return answer;
	}
}