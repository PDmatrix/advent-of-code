using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._17;

[UsedImplicitly]
public class Year2024Day17 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (registers, program) = ParseInput(input);

		return string.Join(",", RunProgram(program, registers));
	}

	private static long GetComboOperand(long val, Dictionary<string, long> registers)
	{
		if (val <= 3)
			return val;

		if (val == 4)
			return registers["A"];
		
		if (val == 5)
			return registers["B"];

		if (val == 6)
			return registers["C"];
		
		throw new Exception("Invalid operand");
	}

	public object Part2(IEnumerable<string> input)
	{
		var (registers, program) = ParseInput(input);

		var newRegisters = new Dictionary<string, long>(registers);
		
		long inc = 1;
		while (true)
		{
			newRegisters["A"] = inc;
			newRegisters["B"] = registers["B"];
			newRegisters["C"] = registers["C"];
			
			var programOut = RunProgram(program, newRegisters);
			
			if (program.Count == programOut.Count && IsListEqual(program, programOut))
				break;

			if (IsMatchTheLastPart(program, programOut))
				inc *= 8;
			else
				inc++;
		}

		return inc;
	}

	private static bool IsListEqual(List<long> a, List<long> b)
	{
		if (a.Count != b.Count)
			return false;

		for (var i = 0; i < a.Count; i++)
		{
			if (a[i] != b[i])
				return false;
		}

		return true;
	}
	
	private static bool IsMatchTheLastPart(List<long> orig, List<long> prog)
	{
		var lastIdx = orig.Count - 1;
		for (var i = prog.Count - 1; i >= 0 ; i--)
		{
			if (prog[i] != orig[lastIdx])
				return false;
			lastIdx--;
		}

		return true;
	}

	private static List<long> RunProgram(List<long> program, Dictionary<string, long> registers)
	{
		var instructionPointer = 0;
		var programOut = new List<long>();
		while (instructionPointer < program.Count && instructionPointer >= 0)
		{
			var opCode = program[instructionPointer];
			if (opCode == 0)
			{
				registers["A"] = (long)(registers["A"] /
				                        Math.Pow(2, GetComboOperand(program[instructionPointer + 1], registers)));
			}
			if (opCode == 1)
			{
				registers["B"] ^= program[instructionPointer + 1];
			}
			if (opCode == 2)
			{
				registers["B"] = GetComboOperand(program[instructionPointer + 1], registers) % 8;
			}
			if (opCode == 3)
			{
				if (registers["A"] == 0)
				{
					instructionPointer += 2;
					continue;
				}
				
				instructionPointer = (int) program[instructionPointer + 1];
				continue;
			}
			if (opCode == 4)
			{
				registers["B"] ^= registers["C"];
			}
			if (opCode == 5)
			{
				programOut.Add(GetComboOperand(program[instructionPointer + 1], registers) % 8);
			}
			if (opCode == 6)
			{
				registers["B"] = (long)(registers["A"] /
				                        Math.Pow(2, GetComboOperand(program[instructionPointer + 1], registers)));
			}
			if (opCode == 7)
			{
				registers["C"] = (long)(registers["A"] /
				                       Math.Pow(2, GetComboOperand(program[instructionPointer + 1], registers)));
			}


			instructionPointer += 2;
		}

		return programOut;
	}

	private static (Dictionary<string, long>, List<long>) ParseInput(IEnumerable<string> input)
	{
		var registers = new Dictionary<string, long>();
		var program = new List<long>();
		var parseRegisters = true;
		foreach (var line in input)
		{
			if (string.IsNullOrEmpty(line))
			{
				parseRegisters = false;
				continue;
			}

			if (parseRegisters)
			{
				var split = line.Split(": ");
				registers[split[0].Split(" ")[1].TrimEnd(':')] = int.Parse(split[1]);
			}
			else
			{
				var split = line.Split(": ");
				program.AddRange(split[1].Split(",").Select(long.Parse));
			}
		}

		return (registers, program);
	}
}