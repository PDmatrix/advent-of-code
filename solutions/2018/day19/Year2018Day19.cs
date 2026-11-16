using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2018._19;

[UsedImplicitly]
public class Year2018Day19 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var enumerable = input as string[] ?? input.ToArray();
		var idx = int.Parse(enumerable.First(x => x.StartsWith('#'))[3..]);
		var instructions = enumerable.Skip(1).ToList();
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
		const string regex = @"(\w+) (\d+) (\d+) (\d+)";
		for (var i = 0; i < 100; i++)
		{
			var instructionPointer = registers[idx];
			if (instructionPointer < 0 || instructionPointer > instructions.Count)
				break;
				
			var current = registers[idx];
				
			var groups = Regex.Match(instructions[current], regex).Groups;
			actions[groups[1].Value](registers, int.Parse(groups[2].Value), int.Parse(groups[3].Value),
				int.Parse(groups[4].Value));
			registers[idx]++;
		}

		var seed = registers[5];
		return DivSum(seed).ToString();
	}

	private static int DivSum(int n) 
	{ 
		if(n == 1) 
			return 1; 
  
		var result = 0; 
		for (var i = 2; i <= Math.Sqrt(n); i++)
		{
			if (n % i != 0) 
				continue;
				
			if (i == n / i) 
				result += i; 
			else
				result += i + n / i;
		} 
      
		return result + n + 1; 
	} 
		
	public object Part2(IEnumerable<string> input)
	{
		var enumerable = input as string[] ?? input.ToArray();
		var idx = int.Parse(enumerable.First(x => x.StartsWith('#'))[3..]);
		var instructions = enumerable.Skip(1).ToList();
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

		var registers = new[] {1, 0, 0, 0, 0, 0};
		const string regex = @"(\w+) (\d+) (\d+) (\d+)";
		for (var i = 0; i < 100; i++)
		{
			var instructionPointer = registers[idx];
			if (instructionPointer < 0 || instructionPointer > instructions.Count)
				break;
				
			var current = registers[idx];
				
			var groups = Regex.Match(instructions[current], regex).Groups;
			actions[groups[1].Value](registers, int.Parse(groups[2].Value), int.Parse(groups[3].Value),
				int.Parse(groups[4].Value));
			registers[idx]++;
		}

		var seed = registers[5];
		return DivSum(seed).ToString();
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