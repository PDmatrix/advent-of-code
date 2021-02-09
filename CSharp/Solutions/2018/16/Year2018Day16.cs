using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2018._16
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day16 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var instructions = input.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
			var actions = new List<Action<IList<int>, int, int, int>>
			{
				AddR, AddI,
				MulR, MulI,
				BanR, BanI,
				BorR, BorI,
				SetR, SetI,
				GtiR, GtrR, GtrI,
				EqiR, EqrR, EqrI
			};
			var result = 0;
			for (var i = 0; i < instructions.Count; i += 3)
			{
				if (!instructions[i].StartsWith("Before"))
					continue;

				var initArr = GetArrayFromInput(instructions[i], @"Before: \[(.+)\]").ToArray();
				var parameters = GetArrayFromInput(instructions[i + 1], @"(.*)", ' ').ToArray();
				var resArr = GetArrayFromInput(instructions[i + 2], @"After:  \[(.+)\]").ToArray();
				var fits = 0;
				foreach (var action in actions)
				{
					var clone = initArr.Clone() as int[] ?? throw new NullReferenceException();
					action(clone, parameters[1], parameters[2], parameters[3]);
					if (!resArr.Where((t, j) => clone[j] != t).Any())
						fits++;
				}

				if (fits >= 3)
					result++;
			}

			return result.ToString();
		}

		private static IEnumerable<int> GetArrayFromInput(string input, string pattern, char separator = ',')
		{
			return Regex.Match(input, pattern)
				.Groups[1]
				.Value
				.Split(separator)
				.Select(int.Parse);
		}

		public object Part2(IEnumerable<string> input)
		{
			var instructions = input.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
			var actions = new Dictionary<Action<IList<int>, int, int, int>, Dictionary<int, int>>
			{
				{AddR, new Dictionary<int, int>()}, {AddI, new Dictionary<int, int>()},
				{MulR, new Dictionary<int, int>()}, {MulI, new Dictionary<int, int>()},
				{BanR, new Dictionary<int, int>()}, {BanI, new Dictionary<int, int>()},
				{BorR, new Dictionary<int, int>()}, {BorI, new Dictionary<int, int>()},
				{SetR, new Dictionary<int, int>()}, {SetI, new Dictionary<int, int>()},
				{GtiR, new Dictionary<int, int>()}, {GtrR, new Dictionary<int, int>()},
				{GtrI, new Dictionary<int, int>()},
				{EqiR, new Dictionary<int, int>()}, {EqrR, new Dictionary<int, int>()},
				{EqrI, new Dictionary<int, int>()}
			};
			for (var i = 0; i < instructions.Count; i += 3)
			{
				if (!instructions[i].StartsWith("Before"))
					continue;

				var initArr = GetArrayFromInput(instructions[i], @"Before: \[(.+)\]").ToArray();
				var parameters = GetArrayFromInput(instructions[i + 1], @"(.*)", ' ').ToArray();
				var resArr = GetArrayFromInput(instructions[i + 2], @"After:  \[(.+)\]").ToArray();
				foreach (var action in actions.Keys)
				{
					var clone = initArr.Clone() as int[] ?? throw new NullReferenceException();
					action(clone, parameters[1], parameters[2], parameters[3]);
					var opCode = parameters[0];
					if (!resArr.Where((t, j) => clone[j] != t).Any())
					{
						actions[action][opCode] =
							actions[action].ContainsKey(opCode) ? actions[action][opCode] + 1 : 1;
					}
				}
			}

			var knownOps = new Dictionary<int, Action<IList<int>, int, int, int>>();
			while (knownOps.Count != 16)
			{
				var (key, value) = actions.First(x => x.Value.Count == 1);
				var elem = value.ToDictionary(x => x.Key, x => x.Value);
				foreach (var keyValuePair in actions)
				{
					keyValuePair.Value.Remove(elem.First().Key);
				}

				knownOps.Add(elem.First().Key, key);
			}

			var registers = new[] {0, 0, 0, 0};
			for (var i = 0; i < instructions.Count; i++)
			{
				if (instructions[i].StartsWith("Before"))
				{
					i += 2;
					continue;
				}

				var parameters = GetArrayFromInput(instructions[i], @"(.*)", ' ').ToArray();
				knownOps[parameters[0]](registers, parameters[1], parameters[2], parameters[3]);
			}

			return registers[0].ToString();
		}

		private static void AddR(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] + regs[b];

		private static void AddI(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] + b;

		private static void MulR(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] * regs[b];

		private static void MulI(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] * b;

		private static void BanR(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] & regs[b];

		private static void BanI(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] & b;

		private static void BorR(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] | regs[b];

		private static void BorI(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] | b;

		private static void SetR(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a];

		private static void SetI(IList<int> regs, int a, int b, int c)
			=> regs[c] = a;

		private static void GtiR(IList<int> regs, int a, int b, int c)
			=> regs[c] = a > regs[b] ? 1 : 0;

		private static void GtrI(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] > b ? 1 : 0;

		private static void GtrR(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] > regs[b] ? 1 : 0;

		private static void EqiR(IList<int> regs, int a, int b, int c)
			=> regs[c] = a == regs[b] ? 1 : 0;

		private static void EqrI(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] == b ? 1 : 0;

		private static void EqrR(IList<int> regs, int a, int b, int c)
			=> regs[c] = regs[a] == regs[b] ? 1 : 0;
	}
}