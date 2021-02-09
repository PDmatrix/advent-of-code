using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._6
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day06 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var instructions = GetInstructions(input);
			var grid = new bool[1000, 1000];
			foreach (var instruction in instructions)
			{
				for (var x = instruction.StartX; x <= instruction.EndX; x++)
				{
					for (var y = instruction.StartY; y <= instruction.EndY; y++)
					{
						var newValue = grid[x, y];
						switch (instruction.Operation)
						{
							case "turn on":
								newValue = true;
								break;
							case "turn off":
								newValue = false;
								break;
							default:
								newValue = !newValue;
								break;
						}
						grid[x, y] = newValue;
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
		
		public object Part2(IEnumerable<string> input)
		{
			var instructions = GetInstructions(input);
			var grid = new int[1000, 1000];
			foreach (var instruction in instructions)
			{
				for (var x = instruction.StartX; x <= instruction.EndX; x++)
				{
					for (var y = instruction.StartY; y <= instruction.EndY; y++)
					{
						var newValue = grid[x, y];
						switch (instruction.Operation)
						{
							case "turn on":
								newValue++;
								break;
							case "turn off":
								newValue = newValue == 0 ? 0 : newValue - 1;
								break;
							default:
								newValue += 2;
								break;
						}
						grid[x, y] = newValue;
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
			const string pattern = 
				@"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)";
			return input.Select(r => Regex.Match(r, pattern).Groups)
				.Select(r => new Instruction
				{
					Operation = r[1].Value,
					StartX = int.Parse(r[2].Value),
					StartY = int.Parse(r[3].Value),
					EndX = int.Parse(r[4].Value),
					EndY = int.Parse(r[5].Value)
				});
		}

		private class Instruction
		{
			public int StartX { get; set; }
			public int EndX { get; set; }
			public int StartY { get; set; }
			public int EndY { get; set; }
			public string Operation { get; set; } = null!;
		}
	}
}