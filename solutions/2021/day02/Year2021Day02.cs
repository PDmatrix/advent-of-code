using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._2;

[UsedImplicitly]
public class Year2021Day02 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var horizontalPosition = 0;
		var depth = 0;
		foreach (var line in input)
		{
			if (line.StartsWith("forward"))
			{
				horizontalPosition += int.Parse(line.Split(' ').Last());
			} else if (line.StartsWith("down"))
			{
				depth += int.Parse(line.Split(' ').Last());
			}
			else
			{
				depth -= int.Parse(line.Split(' ').Last());
			}
		}
		
		return horizontalPosition * depth;
	}

	public object Part2(IEnumerable<string> input)
	{
		var horizontalPosition = 0;
		var depth = 0;
		var aim = 0;
		foreach (var line in input)
		{
			if (line.StartsWith("forward"))
			{
				horizontalPosition += int.Parse(line.Split(' ').Last());
				depth += aim * int.Parse(line.Split(' ').Last());
			} else if (line.StartsWith("down"))
			{
				aim += int.Parse(line.Split(' ').Last());
			}
			else
			{
				aim -= int.Parse(line.Split(' ').Last());
			}
		}
		
		return horizontalPosition * depth;
	}
}