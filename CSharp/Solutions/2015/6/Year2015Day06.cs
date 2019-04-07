using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._6
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day06 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var instructions = GetInstructions(input);
			var grid = new bool[1000, 1000];
			foreach (var instruction in instructions)
			{
				for (var x = instruction.StartX; x <= instruction.EndX; x++)
				{
					for (var y = instruction.StartY; y <= instruction.EndY; y++)
					{
						grid[x, y] = instruction.Operation(grid[x, y]);
					}
				}
			}

			var result = 0;
			for (var x = 0; x < 1000; x++)
			{
				for (var y = 0; y < 1000; y++)
				{
					result += grid[x, y] ? 1 : 0;
				}
			}

			return result.ToString();
		}
		
		public string Part2(IEnumerable<string> input)
		{
			var instructions = GetCorrectInstructions(input);
			var grid = new int[1000, 1000];
			foreach (var instruction in instructions)
			{
				for (var x = instruction.StartX; x <= instruction.EndX; x++)
				{
					for (var y = instruction.StartY; y <= instruction.EndY; y++)
					{
						grid[x, y] = instruction.Operation(grid[x, y]);
					}
				}
			}

			var result = 0;
			for (var x = 0; x < 1000; x++)
			{
				for (var y = 0; y < 1000; y++)
				{
					result += grid[x, y];
				}
			}

			return result.ToString();
		}

		private static IEnumerable<Instruction> GetInstructions(IEnumerable<string> input)
		{
			foreach (var instruction in input)
			{
				var splitted = instruction.Split().ToArray();
				var coordinates = Regex.Matches(instruction, @"\d*")
					.Where(r => !string.IsNullOrWhiteSpace(r.Value))
					.Select(r => int.Parse(r.Value))
					.ToArray();
				var ins = new Instruction
				{
					StartX = coordinates[0],
					StartY = coordinates[1],
					EndX = coordinates[2],
					EndY = coordinates[3]
				};
				switch (splitted[1])
				{
					case "on":
						ins.Operation = b => true;
						break;
					case "off":
						ins.Operation = b => false;
						break;
					default:
						ins.Operation = b => !b;
						break;
				}

				yield return ins;
			}
		}

		private class Instruction
		{
			public int StartX { get; set; }
			public int EndX { get; set; }
			public int StartY { get; set; }
			public int EndY { get; set; }
			public Func<bool, bool> Operation { get; set; }
		}
		
		private class CorrectInstruction
		{
			public int StartX { get; set; }
			public int EndX { get; set; }
			public int StartY { get; set; }
			public int EndY { get; set; }
			public Func<int, int> Operation { get; set; }
		}
		
		private static IEnumerable<CorrectInstruction> GetCorrectInstructions(IEnumerable<string> input)
		{
			foreach (var instruction in input)
			{
				var splitted = instruction.Split().ToArray();
				var coordinates = Regex.Matches(instruction, @"\d*")
					.Where(r => !string.IsNullOrWhiteSpace(r.Value))
					.Select(r => int.Parse(r.Value))
					.ToArray();
				var ins = new CorrectInstruction
				{
					StartX = coordinates[0],
					StartY = coordinates[1],
					EndX = coordinates[2],
					EndY = coordinates[3]
				};
				switch (splitted[1])
				{
					case "on":
						ins.Operation = r => ++r;
						break;
					case "off":
						ins.Operation = r => r == 0 ? 0 : --r;
						break;
					default:
						ins.Operation = r => r + 2;
						break;
				}

				yield return ins;
			}
		}
	}
}