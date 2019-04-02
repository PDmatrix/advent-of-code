using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._2
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day02 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var result = 0;
			foreach (var dimension in input)
			{
				var dims = dimension.Split('x').Select(int.Parse).ToList();
				int l = dims[0],
					w = dims[1],
					h = dims[2];
				var smallestSide = Math.Min(Math.Min(l * w, w * h), h * l);
				result += smallestSide + 2 * l * w + 2 * w * h + 2 * h * l;
			}
			return result.ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var result = 0;
			foreach (var dimension in input)
			{
				var dims = dimension.Split('x').Select(int.Parse).ToList();
				dims.Sort();
				result += dims[0] * 2 + dims[1] * 2 + dims.Aggregate((i, w) => i * w);
			}
			return result.ToString();
		}
	}
}