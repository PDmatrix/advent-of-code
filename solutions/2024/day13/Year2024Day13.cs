using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._13;

[UsedImplicitly]
public class Year2024Day13 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var l = ParseInput(input);

		long answer = 0;
		foreach (var (a, b, prize) in l)
		{
			var denumerator = a.X * b.Y - a.Y * b.X;
			var num1 = a.X * prize.Y - a.Y * prize.X;
			var num2 = b.Y * prize.X - b.X * prize.Y;
			
			if (denumerator == 0)
				continue;
			
			if (num1 % denumerator != 0 || num2 % denumerator != 0)
				continue;
			
			var A = num1 / denumerator;
			var B = num2 / denumerator;
			if (A < 0 || B < 0)
				continue;
			answer += A + 3 *  B;
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var l = ParseInput(input);

		long answer = 0;
		foreach (var (a, b, prize) in l)
		{
			var newPrize = (X: prize.X + 10000000000000, Y: prize.Y + 10000000000000);
			var denumerator = a.X * b.Y - a.Y * b.X;
			var num1 = a.X * newPrize.Y - a.Y * newPrize.X;
			var num2 = b.Y * newPrize.X - b.X * newPrize.Y;

			if (denumerator == 0)
				continue;

			if (num1 % denumerator != 0 || num2 % denumerator != 0)
				continue;

			var A = num1 / denumerator;
			var B = num2 / denumerator;
			if (A < 0 || B < 0)
				continue;
			answer += A + 3 * B;
		}

		return answer;
	}

	private static List<((long X, long Y) A, (long X, long Y) B, (long X, long Y) Prize)> ParseInput(IEnumerable<string> input)
	{
		var list = new List<((long X, long Y) A, (long X, long Y) B, (long X, long Y) Prize)>();
		(long X, long Y) a = (0, 0);
		(long X, long Y) b = (0, 0);
		(long X, long Y) prize = (0, 0);
		var buttonRegex = new Regex(@"Button (A|B): X\+(?<x>\d+), Y\+(?<y>\d+)");
		var prizeRegex = new Regex(@"Prize: X=(?<x>\d+), Y=(?<y>\d+)");
		foreach (var line in input)
		{
			if (line.Contains("Button A"))
			{
				var match = buttonRegex.Match(line);
				a = (long.Parse(match.Groups["x"].ToString()), long.Parse(match.Groups["y"].ToString()));
			}
			
			if (line.Contains("Button B"))
			{
				var match = buttonRegex.Match(line);
				b = (long.Parse(match.Groups["x"].ToString()), long.Parse(match.Groups["y"].ToString()));
			}

			if (line.Contains("Prize"))
			{
				var match = prizeRegex.Match(line);
				prize = new (long.Parse(match.Groups["x"].ToString()), long.Parse(match.Groups["y"].ToString()));
				list.Add((a, b, prize));
			}
		}

		return list;
	}
}