using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._8;

[UsedImplicitly]
public class Year2020Day08 : ISolution
{
	private record Instruction(string Op, int Arg);
	
	public object Part1(IEnumerable<string> input)
	{
		var instructions = new List<Instruction>();
		foreach (var line in input)
		{
			var regex = new Regex(@"(?<op>nop|acc|jmp) (?<arg>.+)", RegexOptions.Compiled);
			var match = regex.Match(line);
			var op = match.Groups["op"].Value;
			var arg = int.Parse(string.Join(string.Empty, match.Groups["arg"].Value.Skip(1)));
			if (match.Groups["arg"].Value[0] == '-')
				arg *= -1;
			 
			instructions.Add(new Instruction(op, arg));
		}

		var acc = 0;
		CheckLoop(instructions, ref acc);

		return acc;
	}

	private static bool CheckLoop(IReadOnlyList<Instruction> instructions, ref int acc)
	{
		var index = 0;
		var instructionIndexes = new HashSet<int>();
		var looped = false;
		while (index < instructions.Count)
		{
			if (instructionIndexes.Contains(index))
			{
				looped = true;
				break;
			}

			instructionIndexes.Add(index);
			var current = instructions[index];
			switch (current.Op)
			{
				case "acc":
					acc += current.Arg;
					break;
				case "jmp":
					index += current.Arg;
					continue;
			}

			index++;
		}

		return looped;
	}


	public object Part2(IEnumerable<string> input)
	{
		var instructions = new List<Instruction>();
		foreach (var line in input)
		{
			var regex = new Regex(@"(?<op>nop|acc|jmp) (?<arg>.+)", RegexOptions.Compiled);
			var match = regex.Match(line);
			var op = match.Groups["op"].Value;
			var arg = int.Parse(string.Join(string.Empty, match.Groups["arg"].Value.Skip(1)));
			if (match.Groups["arg"].Value[0] == '-')
				arg *= -1;
			 
			instructions.Add(new Instruction(op, arg));
		}

		var answers = new List<int>();
		
		Parallel.Invoke(
			() => answers.Add(ReorderInstructions(instructions, "jmp", "nop")),
			() => answers.Add(ReorderInstructions(instructions, "nop", "jmp")));

		return answers.Single(x => x != -1);
	}

	private static int ReorderInstructions(List<Instruction> instructions, string from, string to)
	{
		for (var i = 0; i < instructions.Count; i++)
		{
			var newInstructions = new List<Instruction>(instructions);
			if (newInstructions[i].Op != from)
				continue;

			newInstructions[i] = new Instruction(to, instructions[i].Arg);
			var acc = 0;
			if (!CheckLoop(newInstructions, ref acc))
				return acc;
		}

		return -1;
	}
}