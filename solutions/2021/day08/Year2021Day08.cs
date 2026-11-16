using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2021._8;

[UsedImplicitly]
public class Year2021Day08 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var answer = 0;
		foreach (var line in input)
		{
			var splitted = line.Split(" | ");
			var digits = splitted[1].Split();
			answer += digits.Count(digit => digit.Length is 2 or 3 or 4 or 7);
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var signals = new List<List<string>>();
		var segments = new List<List<string>>();
		foreach (var line in input)
		{
			var splitted = line.Split(" | ");
			signals.Add(splitted[0].Split().ToList());
			segments.Add(splitted[1].Split().ToList());
		}

		var correctCodes = new Dictionary<string, int>
		{
			["abcefg"] = 0,
			["cf"] = 1,
			["acdeg"] = 2,
			["acdfg"] = 3,
			["bcdf"] = 4,
			["abdfg"] = 5,
			["abdefg"] = 6,
			["acf"] = 7,
			["abcdefg"] = 8,
			["abcdfg"] = 9
		};

		var answer = 0;
		for (int i = 0; i < signals.Count; i++)
		{
			var signal = signals[i];
			
			var dict = new Dictionary<string, string>();
			var number1 = signal.Single(x => x.Length == 2);
			signal.Remove(number1);

			var number7 = signal.Single(x => x.Length == 3);
			signal.Remove(number7);

			var number4 = signal.Single(x => x.Length == 4);
			signal.Remove(number4);

			var number8 = signal.Single(x => x.Length == 7);
			signal.Remove(number8);

			dict["a"] = Diff(number7, number1);

			var number9 = signal.Single(x => x.Length == 6 && number4.All(x.Contains));
			signal.Remove(number9);

			dict["e"] = Diff(number8, number9);

			var number0 = signal.Single(x => x.Length == 6 && number1.All(x.Contains));
			signal.Remove(number0);

			dict["d"] = Diff(number8, number0);

			var number6 = signal.Single(x => x.Length == 6);
			signal.Remove(number6);

			dict["c"] = Diff(number8, number6);

			dict["f"] = Diff(number1, dict["c"]);

			dict["b"] = Diff(number4, dict["c"] + dict["d"] + dict["f"]);

			dict["g"] = Diff(number0, dict["a"] + dict["b"] + dict["c"] + dict["e"] + dict["f"]);

			var newCodes = new Dictionary<string, int>();
			foreach (var kv in correctCodes)
			{
				var sb = new StringBuilder();
				foreach (var c in kv.Key)
				{
					sb.Append(dict[c.ToString()]);
				}

				newCodes[Sort(sb.ToString())] = kv.Value;
			}

			var sum = new StringBuilder();
			foreach (var segment in segments[i])
				sum.Append(newCodes[Sort(segment)]);

			answer += int.Parse(sum.ToString());
		}
		
		
		return answer;
	}
	
	private static string Sort(string input)
	{
		var characters = input.ToArray();
		Array.Sort(characters);
		return new string(characters);
	}

	private static string Diff(string a, string b)
	{
		var hs = a.ToHashSet();
		hs.ExceptWith(b.ToHashSet());

		var sb = new StringBuilder();
		foreach (var c in hs)
			sb.Append(c);

		return sb.ToString();
	}
}