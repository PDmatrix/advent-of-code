using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._13;

[UsedImplicitly]
public class Year2023Day13 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		return GetResults(ParseInput(input)).Sum();
	}

	private static List<int> GetResults(List<List<char[]>> patterns)
	{
		var result = new List<int>();
		foreach (var pattern in patterns)
		{
			var (_, num) = GetNum(pattern);
			result.Add(num);
		}

		return result;
	}

	private static (bool, int) GetNum(List<char[]> pattern, int ignore = -1)
	{
		bool found;
		for (var i = 0; i < pattern.Count - 1; i++)
		{
			if (new string(pattern[i]) != new string(pattern[i + 1]))
				continue;
			
			found = true;
			var backward = i;
			var forward = i + 1;
			while (forward < pattern.Count && backward >= 0)
			{
				if (new string(pattern[backward]) != new string(pattern[forward]))
				{
					found = false;
					break;
				}

				backward--;
				forward++;
			}
					
			if (!found)
				continue;
				
			if (ignore == 100 * (i + 1))
				continue;
					
			return (true, 100 * (i + 1));
		}
			
		for (var i = 0; i < pattern.First().Length - 1; i++)
		{
			var current = GetString(pattern, i);
			var next = GetString(pattern, i + 1);
			if (current != next) 
				continue;
			
			found = true;
			var backward = i;
			var forward = i + 1;
			while (forward < pattern.First().Length && backward >= 0)
			{
				if (GetString(pattern, backward) != GetString(pattern, forward))
				{
					found = false;
					break;
				}

				backward--;
				forward++;
			}
					
			if (!found)
				continue;
				
			if (ignore == i + 1)
				continue;
					
			return (true, i + 1);
		}
		
		return (false, 0);
	}

	public object Part2(IEnumerable<string> input)
	{
		var patterns = ParseInput(input);

		var results = GetResults(patterns);

		for (var i = 0; i < patterns.Count; i++)
		{
			var f = false;
			for (var y = 0; y < patterns[i].Count; y++)
			{
				for (var x = 0; x < patterns[i][y].Length; x++)
				{
					patterns[i][y][x] = patterns[i][y][x] == '#' ? '.' : '#';
					
					var (found, num) = GetNum(patterns[i], results[i]);
					if (found)
					{
						results[i] = num;
						f = true;
						break;
					}
					patterns[i][y][x] = patterns[i][y][x] == '#' ? '.' : '#';
				}
				
				if (f)
					break;
			}
		}
		
		return results.Sum();
	}

	private static string GetString(List<char[]> pattern, int col)
	{
		return pattern.Aggregate(string.Empty, (current, line) => current + line[col]);
	}
	
	private static List<List<char[]>> ParseInput(IEnumerable<string> input)
	{
		var result = new List<List<char[]>>();
		var temp = new List<char[]>();
		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				result.Add(temp);
				temp = new List<char[]>();
				continue;
			}
			
			temp.Add(line.ToCharArray());
		}
		
		result.Add(temp);

		return result;
	}
}