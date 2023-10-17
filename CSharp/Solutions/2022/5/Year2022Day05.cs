using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._5;

[UsedImplicitly]
public class Year2022Day05 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		return Solve(input, Move);
	}

	public object Part2(IEnumerable<string> input)
	{
		return Solve(input, MoveMultiple);
	}

	private static string Solve(IEnumerable<string> input, Action<IReadOnlyList<Stack<char>>, int, int, int> solver)
	{
		var state = 0;
		var positions = new List<string>();
		var startingPositions = new List<Stack<char>>();
		var regex = new Regex(@"move (?<c>\d+) from (?<f>\d+) to (?<t>\d+)");
		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
				continue;

			switch (state)
			{
				case 0 when line[1] == '1':
					state = 1;
					var stackCount = int.Parse(line[^2].ToString());
					startingPositions = ParseStartingPosition(positions, stackCount).ToList();
					break;
				case 0:
					positions.Add(line);
					break;
				case 1:
					var match = regex.Match(line);
					var count = int.Parse(match.Groups["c"].ToString());
					var from = int.Parse(match.Groups["f"].ToString()) - 1;
					var to = int.Parse(match.Groups["t"].ToString()) - 1;
					solver(startingPositions, count, from, to);
					break;
			}
		}

		var sb = new StringBuilder();
		foreach (var stack in startingPositions)
			sb.Append(stack.Pop());

		return sb.ToString();

	}

	private static IEnumerable<Stack<char>> ParseStartingPosition(IEnumerable<string> input, int stackCount)
	{
		var cleaned = new List<Stack<char>>();
		for (var i = 0; i < stackCount; i++)
			cleaned.Add(new Stack<char>());

		foreach (var line in input.Reverse())
		{
			var idx = 0;
			for (var i = 0; i < line.Length; i += 4)
			{
				var curChar = line[i + 1];
				if (curChar == ' ')
				{
					idx++;
					continue;
				}

				cleaned[idx].Push(curChar);

				idx++;
			}
		}

		return cleaned;
	}

	private static void Move(IReadOnlyList<Stack<char>> state, int count, int from, int to)
	{
		for (var i = 0; i < count; i++)
		{
			var cur = state[from].Pop();
			state[to].Push(cur);
		}
	}
	
	private static void MoveMultiple(IReadOnlyList<Stack<char>> state, int count, int from, int to)
	{
		var temp = new List<char>();
		for (var i = 0; i < count; i++)
		{
			var cur = state[from].Pop();
			temp.Add(cur);
		}

		temp.Reverse();
		foreach (var cur in temp)
			state[to].Push(cur);
	}

}