using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._15
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day15 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var (lastA, lastB) = Parse(input);
			var res = 0;
			for (var i = 0; i < 40_000_000; i++)
			{
				lastA = ComputeNextGeneratorValue('a', lastA);
				lastB = ComputeNextGeneratorValue('b', lastB);
				var binA = Convert.ToString(lastA, 2);
				var binB = Convert.ToString(lastB, 2);
				
				CompareLowestBinary(binA, binB, ref res);
			}
			
			return res.ToString();
		}

		private static (long a, long b) Parse(IEnumerable<string> input)
		{
			input = input.ToList();
			var lastA = long.Parse(input.First().Split()[4]);
			var lastB = long.Parse(input.Last().Split()[4]);

			return (lastA, lastB);
		}

		private static void CompareLowestBinary(string binA, string binB, ref int res)
		{
			if (binA.Length < 16 || binB.Length < 16) return;
			if (binA[^16..] == binB[^16..])
			{
				res++;
			}
		}

		private static long ComputeNextGeneratorValue(char generatorName, long lastValue)
		{
			return generatorName switch
			{
				'a' => GenerateValue(lastValue, 16807),
				'b' => GenerateValue(lastValue, 48271),
				_ => throw new Exception("Wrong generator name")
			};
		}

		private static long GenerateValue(long last, int factor)
		{
			const long del = 2147483647;
			return last * factor % del;
		}

		public string Part2(IEnumerable<string> input)
		{
			var (lastA, lastB) = Parse(input);
			var queueA = new Queue<long>();
			var queueB = new Queue<long>();
			while (queueA.Count < 5_000_000 || queueB.Count < 5_000_000)
			{
				lastA = ComputeNextGeneratorValue('a', lastA);
				if (lastA % 4 == 0)
				{
					queueA.Enqueue(lastA);
				}
				
				lastB = ComputeNextGeneratorValue('b', lastB);
				if (lastB % 8 == 0)
				{
					queueB.Enqueue(lastB);
				}
			}

			var res = 0;
			while (queueA.Count != 0 && queueB.Count != 0)
			{
				var itA = queueA.Dequeue();
				var itB = queueB.Dequeue();
				var binA = Convert.ToString(itA, 2);
				var binB = Convert.ToString(itB, 2);
				
				CompareLowestBinary(binA, binB, ref res);
			}

			return res.ToString();
		}
	}
}