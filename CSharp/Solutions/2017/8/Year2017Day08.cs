using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._8
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day08 : ISolution
	{
		private readonly Dictionary<string, Func<Expression, Expression, Expression>> _opMap =
			new Dictionary<string, Func<Expression, Expression, Expression>>
			{
				{">", Expression.GreaterThan},
				{">=", Expression.GreaterThanOrEqual},
				{"<", Expression.LessThan},
				{"<=", Expression.LessThanOrEqual},
				{"==", Expression.Equal},
				{"!=", Expression.NotEqual}
			};

		private const string Pattern = @"(?<reg>\w+?) (?<op>inc|dec) (?<by>-?\d+?) if (?<condreg>\w+?) (?<condop>>|<|>=|<=|==|!=) (?<condval>-?\d+)";

		public object Part1(IEnumerable<string> input)
		{
			var registers = new Dictionary<string, int>();
			foreach (var el in input)
			{
				var groups = Regex.Match(el, Pattern).Groups;
				var instruction = GetInstruction(groups);
				Init(registers, instruction.Register);
				Init(registers, instruction.ConditionalRegister);

				var expression = instruction.ConditionalOperator(
					Expression.Constant(registers[instruction.ConditionalRegister]),
					Expression.Constant(instruction.ConditionalValue)
				);
				var func = Expression.Lambda<Func<bool>>(expression).Compile();
				
				if (!func()) 
					continue;
				
				registers[instruction.Register] +=
					instruction.Operation == "inc" ? instruction.ByValue : -instruction.ByValue;
			}

			return registers.Select(r => r.Value).Max().ToString();
		}

		private static void Init(IDictionary<string, int> registers, string key)
		{
			if(!registers.ContainsKey(key)) registers.Add(key, 0);
		}

		public object Part2(IEnumerable<string> input)
		{
			var registers = new Dictionary<string, int>();
			var maxValue = 0;
			foreach (var el in input)
			{
				var groups = Regex.Match(el, Pattern).Groups;
				var instruction = GetInstruction(groups);
				Init(registers, instruction.Register);
				Init(registers, instruction.ConditionalRegister);

				var expression = instruction.ConditionalOperator(
					Expression.Constant(registers[instruction.ConditionalRegister]),
					Expression.Constant(instruction.ConditionalValue)
				);
				var func = Expression.Lambda<Func<bool>>(expression).Compile();

				if (!func())
					continue;

				registers[instruction.Register] +=
					instruction.Operation == "inc" ? instruction.ByValue : -instruction.ByValue;

				var newMax = registers.Select(r => r.Value).Max();
				if (newMax > maxValue) maxValue = newMax;
			}

			return maxValue.ToString();
		}

		private Instruction GetInstruction(GroupCollection groups)
		{
			return new Instruction
			{
				Register = groups["reg"].Value,
				Operation = groups["op"].Value,
				ByValue = int.Parse(groups["by"].Value),
				ConditionalRegister = groups["condreg"].Value,
				ConditionalOperator = _opMap[groups["condop"].Value],
				ConditionalValue = int.Parse(groups["condval"].Value)
			};
		}
		
		private class Instruction
		{
			public string Register { get; set; } = null!;
			public string Operation { get; set; } = null!;
			public int ByValue { get; set; }
			public string ConditionalRegister { get; set; } = null!;
			public Func<Expression, Expression, Expression> ConditionalOperator { get; set; } = null!;
			public int ConditionalValue { get; set; }
		}
	}
}