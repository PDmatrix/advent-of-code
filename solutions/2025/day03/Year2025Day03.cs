using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._3;

[UsedImplicitly]
public class Year2025Day03 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var answer = 0;
		foreach (var line in input)
		{
			var largest = 0;
			for (var i = 0; i < line.Length; i++)
			{
				for (var j = i + 1; j < line.Length; j++)
				{
					var current = int.Parse(line[i] + line[j].ToString());
					if (current > largest)
						largest = current;
				}
			}

			answer += largest;
		}

		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		long answer = 0;
		foreach (var line in input)
		{
			var result = "";
			var startPos = 0;

			for (var i = 0; i < 12; i++)
			{
				var remaining = 12 - i - 1;

				var maxSearchPos = line.Length - remaining - 1;

				var largestDigit = '0';
				var largestPos = startPos;
				for (var j = startPos; j <= maxSearchPos; j++)
				{
					if (line[j] <= largestDigit) continue;

					largestDigit = line[j];
					largestPos = j;
				}

				result += largestDigit;
				startPos = largestPos + 1;
			}

			answer += long.Parse(result);
		}

		return answer;
	}
}