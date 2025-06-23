using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AdventOfCode.Common;
using CSharpx;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._8;

[UsedImplicitly]
public class Year2023Day08 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (turns, network) = ParseInput(input);
		
		var current = "AAA";
		const string end = "ZZZ";
		var idx = 0;
		while (current != end)
		{
			var currentTurn = turns[idx % turns.Length];
			current = currentTurn switch
			{
				'R' => network[current].Right,
				'L' => network[current].Left,
				_ => current
			};
			idx++;
		}
		
		return idx;
	}

	public object Part2(IEnumerable<string> input)
	{
		var (turns, network) = ParseInput(input);
		
		var currentList = network.Where(x => x.Key.EndsWith('A')).Select(x => x.Key).ToList();
		
		var seenMap = new Dictionary<string, HashSet<(string, int)>>();
		foreach (var c in currentList)
		{
			var current = c;
			var idx = 0;
			var seen = new HashSet<(string, int)>();
			while (true)
			{
				var currentTurn = turns[idx % turns.Length];
				var next = currentTurn switch
				{
					'R' => network[current].Right,
					'L' => network[current].Left,
					_ => current
				};
			
				current = next;
				if (!seen.Add((current, idx % turns.Length)))
					break;

				idx++;
			}

			seenMap[c] = seen;
		}
		
		return seenMap
			.Select(x => (long)x.Value.ToList().FindIndex(xy => xy.Item1.EndsWith('Z')) + 1)
			.Aggregate((long)1, Lcm);
	}
	
	private static (string, Dictionary<string, (string Left, string Right)>) ParseInput(IEnumerable<string> input)
	{
		var lines = input.ToList();
		var turns = lines[0];
		var network = new Dictionary<string, (string Left, string Right)>();
		
		for (var i = 2; i < lines.Count; i++)
		{
			var parts = lines[i].Split();
			network[parts[0]] = (parts[2].TrimStart('(').TrimEnd(','), parts[3].TrimEnd(')'));
		}
		
		return (turns, network);
	}
	
	private static long Gcf(long a, long b)
	{
		while (b != 0)
		{
			var temp = b;
			b = a % b;
			a = temp;
		}
		return a;
	}

	private static long Lcm(long a, long b)
	{
		return (a / Gcf(a, b)) * b;
	}
}