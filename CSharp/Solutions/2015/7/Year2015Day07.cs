using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._7
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day07 : ISolution
	{
		// ReSharper disable once UnusedMember.Global
		public static int Year = 2015;
		// ReSharper disable once UnusedMember.Global
		public static int Day = 7;

		private class Instruction
		{
			public Instruction()
			{
				Depends = new List<string>();
				Value = null;
				RValue = null;
				LValue = null;
			}
			public Func<int, int, int> BinaryOperation { get; set; }
			public Func<int, int> UnaryOperation { get; set; }
			public string Wire { get; set; }
			public int? Value { get; set; }
			public List<string> Depends { get; set; }
			public int? RValue { get; set; }
			public int? LValue { get; set; }
		}

		private static void Common(IReadOnlyList<string> splt, Instruction inst, List<string> dp, ICollection<Instruction> list)
		{
            if (int.TryParse(splt[0].Trim(), out var lval))
            {
                inst.LValue = lval;
            }
            else
            {
	            dp[0] = splt[0].Trim();
            }
            if (int.TryParse(splt[1].Trim(), out var rval))
            {
                inst.RValue = rval;
            }
            else
            {
	            dp[1] = splt[1].Trim();
            }

            inst.Depends = dp;
            list.Add(inst);
		}

		private static IEnumerable<Instruction> Parse(IEnumerable<string> lines, int? bVal = null)
		{
			var list = new List<Instruction>();
			foreach (var instruction in lines)
			{
				var split = instruction.Split("->");
				var wire = split[1].Trim();
				if (split[0].Contains("NOT"))
				{
					var str = split[0].Replace("NOT", "").Trim();
					if(int.TryParse(str, out var value))
					{
						list.Add(new Instruction
						{
							Wire = wire,
							Value = ~value
						});
					}
					else
					{
						list.Add(new Instruction
						{
							Wire = wire,
							Depends = new List<string>{str},
							UnaryOperation = i => ~i
						});
					}
				}
				else if (split[0].Contains("AND"))
				{
					var splt = split[0].Split("AND");
					var dp = new List<string>{"MARKED", "MARKED"};
					var inst = new Instruction
					{
						Wire = wire,
						BinaryOperation = (lvalue, rvalue) => lvalue & rvalue
					};
					Common(splt, inst, dp, list);
				}
				else if (split[0].Contains("OR"))
				{
					var splt = split[0].Split("OR");
					var dp = new List<string>{"MARKED", "MARKED"};
					var inst = new Instruction
					{
						Wire = wire,
						BinaryOperation = (lvalue, rvalue) => lvalue | rvalue
					};
					Common(splt, inst, dp, list);
				}
				else if (split[0].Contains("RSHIFT"))
				{
					var splt = split[0].Split("RSHIFT");
					var dp = new List<string>{"MARKED", "MARKED"};
					var inst = new Instruction
					{
						Wire = wire,
						BinaryOperation = (lvalue, rvalue) => lvalue >> rvalue
					};
					Common(splt, inst, dp, list);
				}
				else if (split[0].Contains("LSHIFT"))
				{
					var splt = split[0].Split("LSHIFT");
					var dp = new List<string>{"MARKED", "MARKED"};
					var inst = new Instruction
					{
						Wire = wire,
						BinaryOperation = (lvalue, rvalue) => lvalue << rvalue
					};
					Common(splt, inst, dp, list);
				}
				else
				{
					var val = split[0].Trim();
					if(int.TryParse(val, out var value))
					{
						if (bVal.HasValue)
						{
							if (wire.Equals("b"))
							{
								value = bVal.Value;
							}
						}
						list.Add(new Instruction
						{
							Wire = wire,
							Value = value
						});
					}
					else
					{
						list.Add(new Instruction
						{
							Wire = wire,
							Depends = new List<string>{val}
						});
					}
				}
			}

			return list;
		}

		private static int Process(IReadOnlyCollection<Instruction> instructions)
		{
			var processed = new List<string>();
			while(true) 
			{
				var el = instructions.FirstOrDefault(r => !r.Depends.Select(d => d != "MARKED").Any() && !processed.Contains(r.Wire));
				if (el == null)
					break;
				processed.Add(el.Wire);
				if (el.UnaryOperation != null)
				{
					el.Value = el.UnaryOperation(el.LValue.Value);
				}
				else if(el.BinaryOperation != null)
				{
					el.Value = el.BinaryOperation(el.LValue.Value, el.RValue.Value);
				}
				else if(el.LValue.HasValue)
				{
					el.Value = el.LValue;
				}
				foreach (var instruction in instructions)
				{
					if (!instruction.Depends.Contains(el.Wire)) 
						continue;

					if (instruction.Depends.Count == 2)
					{
						if (instruction.Depends[0] == "MARKED")
						{
							instruction.RValue = el.Value;
							instruction.Depends.Clear(); 
						}
						else if (instruction.Depends[1] == "MARKED")
						{
							instruction.LValue = el.Value;
							instruction.Depends.Clear(); 
						}
						else
						{
							var index = instruction.Depends.IndexOf(el.Wire);
							switch (index)
							{
								case 0:
									instruction.LValue = el.Value;
									instruction.Depends[0] = "MARKED";
									break;
								case 1:
									instruction.RValue = el.Value;
									instruction.Depends[1] = "MARKED";
									break;
							}
						}
					}
					else
					{
						instruction.LValue = el.Value;
						instruction.Depends.Clear();
					}
				}
			}

			return instructions.FirstOrDefault(r => r.Wire.Equals("a")).Value.Value;
		}
		
		public string Part1(IEnumerable<string> lines)
		{
			var parsed = Parse(lines);
			var instructions = parsed.ToList();
			return Process(instructions).ToString();
		}

		public string Part2(IEnumerable<string> lines)
		{
			var enumerable = lines as string[] ?? lines.ToArray();
			var aVal = int.Parse(Part1(enumerable));
			var parsed = Parse(enumerable, aVal);
			var instructions = parsed.ToList();
			return Process(instructions).ToString();
		}
	}
}