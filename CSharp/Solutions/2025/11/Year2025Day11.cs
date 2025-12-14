using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._11;

[UsedImplicitly]
public class Year2025Day11 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		// input = new[]
		// {
		// 	"aaa: you hhh",
		// 	"you: bbb ccc",
		// 	"bbb: ddd eee",
		// 	"ccc: ddd eee fff",
		// 	"ddd: ggg",
		// 	"eee: out",
		// 	"fff: out",
		// 	"ggg: out",
		// 	"hhh: ccc fff iii",
		// 	"iii: out",
		// };
		
		var devices = ParseInput(input);
		
		var q = new Queue<string>();
		q.Enqueue("you");
		var answer = 0;
		while (q.Any())
		{
			var current = q.Dequeue();
			var parts = devices[current];
			foreach (var part in parts)
			{
				if (part == "out")
				{
					answer++;
					continue;
				}
				
				q.Enqueue(part);
			}
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		// input = new[]
		// {
		// 	"svr: aaa bbb",
		// 	"aaa: fft",
		// 	"fft: ccc",
		// 	"bbb: tty",
		// 	"tty: ccc",
		// 	"ccc: ddd eee",
		// 	"ddd: hub",
		// 	"hub: fff",
		// 	"eee: dac",
		// 	"dac: fff",
		// 	"fff: ggg hhh",
		// 	"ggg: out",
		// 	"hhh: out",
		// };
		
		var devices = ParseInput(input);

		var memo = new Dictionary<State, long>();
		long Count(State state)
		{
			if (memo.ContainsKey(state))
				return memo[state];

			if (state.Device == "out")
			{
				var passedDacAndFft = state.Dac && state.Fft;
				memo[state] = passedDacAndFft ? 1 : 0;
				return memo[state];
			}
			
			if (state.Device == "fft")
				state.Fft = true;
			if (state.Device == "dac")
				state.Dac = true;
			
			var nextDevices = devices[state.Device];
			long total = 0;
			foreach (var next in nextDevices)
			{
				var nextState = state with { Device = next };
				total += Count(nextState);
			}
			memo[state] = total;
			
			return total;
		}
		
		return Count(new State("svr", false, false));
	}
	
	private record struct State(string Device, bool Dac, bool Fft);

	private static Dictionary<string, List<string>> ParseInput(IEnumerable<string> input)
	{
		var result = new Dictionary<string, List<string>>();
		foreach (var line in input)
		{
			var splitted = line.Split(": ");
			result.Add(splitted[0], splitted[1].Split(' ').ToList());
		}
		
		return result;
	}
}