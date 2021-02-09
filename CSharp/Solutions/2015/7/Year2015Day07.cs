using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._7
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day07 : ISolution
	{
		private Dictionary<string, string[]> 
			_instructions = new Dictionary<string, string[]>();
		public object Part1(IEnumerable<string> input)
		{
			_instructions = input.Select(r => r.Split()).ToDictionary(r => r.Last());
			var value = Process("a");
			return value.ToString();
		}

		public object Part2(IEnumerable<string> input)
		{
			var enumerable = input as string[] ?? input.ToArray();
			_instructions = enumerable.Select(r => r.Split()).ToDictionary(r => r.Last());
			var value = Process("a");
			_instructions = enumerable.Select(r => r.Split()).ToDictionary(r => r.Last());
			_instructions["b"] = new []{value.ToString(), "->", "b"};
			return Process("a").ToString();
		}

		private int Process(string input)
		{
			var ins = _instructions[input];
			var value = GetValue(ins); 
			_instructions[input] = new[] { value.ToString(), "->", input };
			return value;
		}

		private int GetValue(IReadOnlyList<string> instruction)
		{
			int ComputeValue(string x) => char.IsLetter(x[0]) ? Process(x) : int.Parse(x);
			int Assign(IReadOnlyList<string> x) => ComputeValue(x[0]);
			int And(IReadOnlyList<string> x) => ComputeValue(x[0]) & ComputeValue(x[2]);
			int Or(IReadOnlyList<string> x) => ComputeValue(x[0]) | ComputeValue(x[2]);
			int LShift(IReadOnlyList<string> x) => ComputeValue(x[0]) << ComputeValue(x[2]);
			int RShift(IReadOnlyList<string> x) => ComputeValue(x[0]) >> ComputeValue(x[2]);
			int Not(IReadOnlyList<string> x) => ~ComputeValue(x[1]);
			
			switch (instruction[1])
			{
				case "->":
					return Assign(instruction);
				case "AND":
					return And(instruction);
				case "OR":
					return Or(instruction);
				case "LSHIFT":
					return LShift(instruction);
				case "RSHIFT":
					return RShift(instruction);
				default:
					return instruction[0] == "NOT" ? Not(instruction) : 0;
			}
		}
	}
}