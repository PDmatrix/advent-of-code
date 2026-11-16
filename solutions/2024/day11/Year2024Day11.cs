using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._11;

[UsedImplicitly]
public class Year2024Day11 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var stones = input.First().Split().Select(long.Parse).ToList();
		var d = new Dictionary<long, Dictionary<int, long>>();
		return stones.Sum(stone => Compute(d, stone, 0));
	}
	
	public object Part2(IEnumerable<string> input)
	{
		var stones = input.First().Split().Select(long.Parse).ToList();
		var d = new Dictionary<long, Dictionary<int, long>>();
		return stones.Sum(stone => Compute(d, stone, 0, 75));
	}

	private static long Compute(Dictionary<long, Dictionary<int, long>> d, long c, int step, int total = 25)
	{
		if (step == total)
			return 1;

		if (d.TryGetValue(c, out var compute))
		{
			if (compute.TryGetValue(total - step - 1, out var v))
				return v;
		}

		long answer = 0;
		if (c == 0)
		{
			answer += Compute(d, 1, step + 1, total);
			if (!d.ContainsKey(c))
				d[c] = new Dictionary<int, long>();

			d[c][total - step - 1] = answer;

			return answer;
		}

		var cStr = c.ToString();
		if (cStr.Length % 2 == 0)
		{
			var left = long.Parse(cStr[..(cStr.Length / 2)]);
			answer += Compute(d, left, step + 1, total);
			
			var rightStr = cStr[(cStr.Length / 2)..].TrimStart('0');
			if (string.IsNullOrEmpty(rightStr))
				rightStr = "0";
			var right = long.Parse(rightStr);

		    answer += Compute(d, right, step + 1, total);

			if (!d.ContainsKey(c))
				d[c] = new Dictionary<int, long>();
			
			d[c][total - step - 1] = answer;

			return answer;
		}
		
		answer += Compute(d, c * 2024, step + 1, total);
		
		if (!d.ContainsKey(c))
			d[c] = new Dictionary<int, long>();
			
		d[c][total - step - 1] = answer;

		return answer;
	}
}