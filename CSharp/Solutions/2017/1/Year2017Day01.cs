using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._1
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day01 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var sequence = input.First();
			var answer = 0;
			for (var i = 0; i < sequence.Length; i++)
			{
				if (sequence[i] == GetNextChar(i, sequence))
					answer += int.Parse(sequence[i].ToString());
			}
			return answer.ToString();
		}
		
		public string Part2(IEnumerable<string> input)
		{
			var sequence = input.First();
			var answer = 0;
			for (var i = 0; i < sequence.Length; i++)
			{
				if (sequence[i] == GeHalfwayChar(i, sequence))
					answer += int.Parse(sequence[i].ToString());
			}
			return answer.ToString();
		}

		private static char GetNextChar(int index, string value)
		{
			return index + 1 == value.Length ? value[0] : value[index + 1];
		}
		
		private static char GeHalfwayChar(int index, string value)
		{
			var stepsForward = value.Length / 2;
			return value[(index + stepsForward) % value.Length];
		}
	}
}