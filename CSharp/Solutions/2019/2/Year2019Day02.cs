using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2019._2
{
	// ReSharper disable once UnusedMember.Global
	public class Year2019Day02 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var a = input.First().Split(',').Select(int.Parse).ToArray();

			a[1] = 12;
			a[2] = 2;
			
			return RunProgram(a);
		}

		private static int RunProgram(IList<int> a)
		{
			for (var i = 0; i < a.Count; i += 4)
			{
				if (a[i] == 99)
					break;

				a[a[i + 3]] = a[i] switch
				{
					1 => a[a[i + 1]] + a[a[i + 2]],
					2 => a[a[i + 1]] * a[a[i + 2]],
					_ => a[a[i + 3]]
				};
			}

			return a[0];
		}

		public object Part2(IEnumerable<string> input)
		{
			var a = input.First().Split(',').Select(int.Parse).ToArray();

			for (var noun = 0; noun <= 99; noun++)
			{
				for (var verb = 0; verb <= 99; verb++)
				{
					var newInput = a.ToList();
					newInput[1] = noun;
					newInput[2] = verb;
					var value = RunProgram(newInput);
					if (value == 19690720)
						return 100 * noun + verb;
				}
			}

			throw new Exception("No answer");
		}
	}
}