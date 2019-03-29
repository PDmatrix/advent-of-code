using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._1
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day01 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
 			var directions = input.First();
			return directions.Sum(r => r == '(' ? 1 : -1).ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var directions = input.First();
			var floor = 0;
			for (var i = 0; i < directions.Length; i++)
			{
				floor += directions[i] == '(' ? 1 : -1;
				if (floor == -1)
					return (i + 1).ToString();
			}
			throw new Exception("Answer not found");
		}
	}
}