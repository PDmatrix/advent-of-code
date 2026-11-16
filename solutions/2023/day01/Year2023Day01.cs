using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._1;

[UsedImplicitly]
public class Year2023Day01 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		return input
			.Select(line => line.Where(char.IsDigit).ToList())
			.Sum(digits => int.Parse(digits.First().ToString()) * 10 + int.Parse(digits.Last().ToString()));
	}

	public object Part2(IEnumerable<string> input)
	{
		var stringDigits = new Dictionary<string, string>
		{
			{"one", "o1e"},
			{"two", "t2o"},
			{"three", "t3e"},
			{"four", "f4r"},
			{"five", "f5e"},
			{"six", "s6x"},
			{"seven", "s7n"},
			{"eight", "e8t"},
			{"nine", "n9e"},
		};

		var answer = 0;
		foreach (var line in input)
		{
			var newLine = line;
			foreach (var (k, v) in stringDigits)
				newLine = newLine.Replace(k, v);
			
			var digits = new List<int>();
			foreach (var c in newLine)
			{
				if (char.IsDigit(c))
					digits.Add(int.Parse(c.ToString()));
			}
			
			answer += digits.First() * 10 + digits.Last();
		}
		
		return answer;
	}
}