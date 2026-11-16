using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._6;

[UsedImplicitly]
public class Year2025Day06 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (numbers, ops) = ParseInput(input);

		long answer = 0;
		for (var i = 0; i < numbers[0].Count; i++)
		{
			long current = 0;
			var op = ops[i];
			if (op == "*")
				current = 1;
			
			foreach (var num in numbers)
			{
				current = op switch
				{
					"*" => current * num[i],
					"+" => current + num[i],
					_ => current
				};
			}
			
			answer += current;
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var (grid, ops) = ParseInputAsGrid(input);
		
		var maxX = grid.Max(x => x.Key.X);
		var maxY = grid.Max(x => x.Key.Y);
		var opIndex = ops.Count - 1;
		long answer = 0;
		
		long current = 0;
		if (ops[opIndex] ==  "*")
			current = 1;
		
		for (var x = maxX; x >= 0; x--)
		{
			var numStr = "";
			for (var y = 0; y <= maxY; y++)
			{
				var point = new Point(x, y);
				if (grid[point] == " ")
					continue;
				
				numStr += grid[point];
			}

			if (numStr == "")
			{
				opIndex--;
				answer += current;
				current = 0;
				if (ops[opIndex] ==  "*")
					current = 1;
				
				continue;
			}
			
			var num = long.Parse(numStr);
			var op = ops[opIndex];
			current = op switch
			{
				"*" => current * num,
				"+" => current + num,
				_ => current
			};
		}
		
		answer += current;

		return answer;
	}

	private static (List<List<long>> numbers, List<string> ops) ParseInput(IEnumerable<string> input)
	{
		var arr = input.ToArray();
		var numbers = new List<List<long>>();
		var ops = new List<string>();
		var numRegex = new Regex(@"(?<n>\d+)");
		foreach (var line in arr[..^1])
		{
			var matches = numRegex.Matches(line);
			var n = new List<long>();
			foreach (Match match in matches)
				n.Add(long.Parse(match.Groups["n"].Value));
			
			numbers.Add(n);
		}
		
		var lastLine = arr.Last();
		var opRegex = new Regex(@"(?<o>\+|\*)");
		var opMatches = opRegex.Matches(lastLine);
		foreach (Match match in opMatches)
			ops.Add(match.Groups["o"].Value);

		return (numbers, ops);
	}
	
	private static (Dictionary<Point, string> grid, List<string> ops) ParseInputAsGrid(IEnumerable<string> input)
	{
		var arr = input.ToArray();
		var arrWithoutLast = arr[..^1];
		var grid = new Dictionary<Point, string>();
		var ops = new List<string>();
		for (var y = 0; y < arrWithoutLast.Length; y++)
		{
			var x = 0;
			foreach (var c in arrWithoutLast[y])
			{
				grid[new Point(x, y)] = c.ToString();
				x++;
			}
		}
		
		var lastLine = arr.Last();
		var opRegex = new Regex(@"(?<o>\+|\*)");
		var opMatches = opRegex.Matches(lastLine);
		foreach (Match match in opMatches)
			ops.Add(match.Groups["o"].Value);

		return (grid, ops);
	}
}