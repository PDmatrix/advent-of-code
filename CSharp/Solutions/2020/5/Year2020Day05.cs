using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._5;

[UsedImplicitly]
public class Year2020Day05 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var highest = 0;
		foreach (var line in input)
		{
			var newLine = line;
			newLine = newLine.Replace('F', '0');
			newLine = newLine.Replace('B', '1');
			newLine = newLine.Replace('L', '0');
			newLine = newLine.Replace('R', '1');
			var num = Convert.ToInt32(newLine, 2);
			highest = Math.Max(num, highest);
		}
		
		return highest;
	}

	public object Part2(IEnumerable<string> input)
	{
		var set = new HashSet<int>();
		foreach (var line in input)
		{
			var newLine = line;
			newLine = newLine.Replace('F', '0');
			newLine = newLine.Replace('B', '1');
			newLine = newLine.Replace('L', '0');
			newLine = newLine.Replace('R', '1');
			var num = Convert.ToInt32(newLine, 2);
			set.Add(num);
		}

		foreach (var num in set)
		{
			if (!set.Contains(num + 1) && set.Contains(num + 2))
				return num + 1;
		}

		throw new Exception("Answer is not found");
	}
}