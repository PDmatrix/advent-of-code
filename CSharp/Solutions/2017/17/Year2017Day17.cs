using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._17
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day17 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var numberOfSteps = int.Parse(input.First());
			var array = new int[2018];
			var currentPos = 0;
			for (var i = 1; i < 2018; i++)
			{
				var nextPos = GetNextPosition(currentPos, i, numberOfSteps);
				Array.Copy(array, nextPos, array, nextPos + 1, array.Length - nextPos - 1);
				array[nextPos] = i;
				currentPos = nextPos;
			}

			return array[Array.IndexOf(array, 2017) + 1].ToString();
		}

		private static int GetNextPosition(int currentPosition, int value, int steps)
			=> (currentPosition + steps) % value + 1;

		public object Part2(IEnumerable<string> input)
		{
			var numberOfSteps = int.Parse(input.First());
			var currentPos = 0;
			var second = int.MinValue;
			for (var i = 1; i < 50_000_000; i++)
			{
				currentPos = GetNextPosition(currentPos, i, numberOfSteps);
				if (currentPos == 1)
				{
					second = i;
				}
			}

			return second.ToString();
		}
	}
}