using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2018._21
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day21 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var enumerable = input as string[] ?? input.ToArray();
			var idx = int.Parse(enumerable.First(x => x.StartsWith('#'))[3..]);
			const string regex = @"(\w+) (\d+) (\d+) (\d+)";
			var instructionPointer = 0;
			var instructions = enumerable.Skip(1)
				.Select(x =>
				{
					var groups = Regex.Match(x, regex).Groups;
					return (op: groups[1].Value, a: int.Parse(groups[2].Value), b: int.Parse(groups[3].Value),
						c: int.Parse(groups[4].Value));
				}).ToList();
			
			const int targetIp = 28;
			var actions = new Dictionary<string, Action<IList<int>, int, int, int>>
			{
				["addr"] = AddR, ["addi"] = AddI,
				["mulr"] = MulR, ["muli"] = MulI,
				["banr"] = BanR, ["bani"] = BanI,
				["borr"] = BorR, ["bori"] = BorI,
				["setr"] = SetR, ["seti"] = SetI,
				["gtir"] = GtiR, ["gtrr"] = GtrR, ["gtri"] = GtrI,
				["eqir"] = EqiR, ["eqrr"] = EqrR, ["eqri"] = EqrI
			};

			var registers = new[] {0, 0, 0, 0, 0, 0};
			var targetRegisterHistory = new HashSet<int>();
			while (instructionPointer < instructions.Count)
			{
				registers[idx] = instructionPointer;

				var (op, a, b, c) = instructions[instructionPointer];

				if (instructionPointer == targetIp)
					return registers[a].ToString();
				
				actions[op](registers, a, b, c);
				
				instructionPointer = registers[idx];
				instructionPointer++;
			}

			return targetRegisterHistory.First().ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var enumerable = input as string[] ?? input.ToArray();
			var idx = int.Parse(enumerable.First(x => x.StartsWith('#'))[3..]);
			const string regex = @"(\w+) (\d+) (\d+) (\d+)";
			var instructionPointer = 0;
			var instructions = enumerable.Skip(1)
				.Select(x =>
				{
					var groups = Regex.Match(x, regex).Groups;
					return (op: groups[1].Value, a: int.Parse(groups[2].Value), b: int.Parse(groups[3].Value),
						c: int.Parse(groups[4].Value));
				}).ToList();
			
			const int targetIp = 28;
			var actions = new Dictionary<string, Action<IList<int>, int, int, int>>
			{
				["addr"] = AddR, ["addi"] = AddI,
				["mulr"] = MulR, ["muli"] = MulI,
				["banr"] = BanR, ["bani"] = BanI,
				["borr"] = BorR, ["bori"] = BorI,
				["setr"] = SetR, ["seti"] = SetI,
				["gtir"] = GtiR, ["gtrr"] = GtrR, ["gtri"] = GtrI,
				["eqir"] = EqiR, ["eqrr"] = EqrR, ["eqri"] = EqrI
			};

			var registers = new[] {0, 0, 0, 0, 0, 0};
			var targetRegisterHistory = new HashSet<int>();
			while (instructionPointer < instructions.Count)
			{
				registers[idx] = instructionPointer;

				var (op, a, b, c) = instructions[instructionPointer];

				if (instructionPointer == targetIp)
				{
					var targetRegisterValue = registers[a];
					
					if (targetRegisterHistory.Contains(targetRegisterValue))
						break;
					
					targetRegisterHistory.Add(targetRegisterValue);
				}
				
				actions[op](registers, a, b, c);
				
				instructionPointer = registers[idx];
				instructionPointer++;
			}
			
			return targetRegisterHistory.Last().ToString();
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