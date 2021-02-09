using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2019._1
{
	// ReSharper disable once UnusedMember.Global
	public class Year2019Day01 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			return input
				.Select(int.Parse)
				.Sum(mass => (int) Math.Round((double) mass / 3, MidpointRounding.ToZero) - 2);
		}

		public object Part2(IEnumerable<string> input)
		{
			var masses = input.Select(int.Parse);
			var requirement = 0;
			foreach (var mass in masses)
			{
				var currentMass = mass;
				while (currentMass > 0)
				{
					var res = (int) Math.Round((double) currentMass / 3, MidpointRounding.ToZero) - 2;
					requirement += res > 0 ? res : 0;
					currentMass = res;
				}
			}
			
			return requirement;
		}
	}
}