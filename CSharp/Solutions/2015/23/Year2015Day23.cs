using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._23
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day23 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var instructions = GetInstructions(input);
			var currentIdx = 0;
			var registerB = 0;
			while (instructions.Count > currentIdx && currentIdx >= 0)
			{
				(currentIdx, registerB) = instructions[currentIdx].Operation();
			}
			return registerB.ToString();
		}
		
		public string Part2(IEnumerable<string> input)
		{
			var instructions = GetInstructions(input, 1);
			var currentIdx = 0;
			var registerB = 0;
			while (instructions.Count > currentIdx && currentIdx >= 0)
			{
				(currentIdx, registerB) = instructions[currentIdx].Operation();
			}
			return registerB.ToString();
		}

		private static ref int GetRegister(string instruction, ref int registerA, ref int registerB)
		{
			if (instruction.Trim(',') == "a")
				return ref registerA;
			return ref registerB;
		}
		
		private static IReadOnlyList<Instruction> GetInstructions(IEnumerable<string> input, int overrideRegisterA = 0)
		{
			var instructions = new List<Instruction>();
			var registerA = overrideRegisterA;
			var registerB = 0;
			var currentIdx = 0;
			foreach (var instruction in input)
			{
				var splitted = instruction.Split(" ").ToArray();
				var operation = splitted[0];
				var newInstruction = new Instruction();

				switch (operation)
				{
					case "hlf":
						newInstruction.Operation = () =>
						{
							ref var register = ref GetRegister(splitted[1], ref registerA, ref registerB);
							register /= 2;
							return (++currentIdx, registerB);
						};
						break;
					case "tpl":
						newInstruction.Operation = () =>
						{
							ref var register = ref GetRegister(splitted[1], ref registerA, ref registerB);
							register *= 3;
							return (++currentIdx, registerB);
						};
						break;
					case "inc":
						newInstruction.Operation = () =>
						{
							ref var register = ref GetRegister(splitted[1], ref registerA, ref registerB);
							register++;
							return (++currentIdx, registerB);
						};
						break;
					case "jmp":
						newInstruction.Operation = () =>
						{
							currentIdx += int.Parse(splitted[1]);
							return (currentIdx, registerB);
						};
						break;
					case "jie":
						newInstruction.Operation = () =>
						{
							ref var register = ref GetRegister(splitted[1], ref registerA, ref registerB);
							if (register % 2 == 0)
								currentIdx += int.Parse(splitted[2]);
							else
								currentIdx++;

							return (currentIdx, registerB);
						};
						break;
					case "jio":
						newInstruction.Operation = () =>
						{
							ref var register = ref GetRegister(splitted[1], ref registerA, ref registerB);
							if (register == 1)
								currentIdx += int.Parse(splitted[2]);
							else
								currentIdx++;

							return (currentIdx, registerB);
						};
						break;
					default:
						throw new Exception("Unknown operation");
				}
				instructions.Add(newInstruction);
			}

			return instructions.ToArray();
		}

		private class Instruction
		{
			public Func<(int, int)> Operation { get; set; } = null!;
		}
	}
}