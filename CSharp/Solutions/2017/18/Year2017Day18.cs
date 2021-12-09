using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2017._18;

[UsedImplicitly]
public class Year2017Day18 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		long sent = -1;
		RunUntilBlocked(Interpreter(input.ToArray(), 0, n => sent = n, () => null));
			
		return sent.ToString();
	}
		
	private static void RunUntilBlocked(Func<bool> interpreter) {
		while (!interpreter()) {}
	}
		
		
	private static Func<bool> Interpreter(string[] instructions, int id, Action<long> send, Func<long?> recv) {
		var regex = new Regex(@"(?<op>snd|set|add|mul|mod|rcv|jgz) (?<f>.+?) ?(?<s>.+)?", RegexOptions.Compiled);
		var registers = new Dictionary<string, long> {["p"] = id};
		var blocked = false;
		long curPos = 0;

		long GetVal(string group)
		{
			if (long.TryParse(group, out var value))
			{
				return value;
			}

			if (registers.ContainsKey(group))
			{
				return registers[group];
			}

			registers[group] = 0;
			
			return 0;	
		}

		var commands = new Dictionary<string, Action<string, string>>
		{
			["snd"] = (x, _) => send(GetVal(x)),
			["set"] = (x, y) => registers[x] = GetVal(y),
			["add"] = (x, y) => registers[x] = GetVal(x) + GetVal(y),
			["mul"] = (x, y) => registers[x] = GetVal(x) * GetVal(y),
			["mod"] = (x, y) => registers[x] = GetVal(x) % GetVal(y),
			["rcv"] = (x, _) =>
			{
				var r = recv();
				if (r == null) blocked = true;
				else
				{
					registers[x] = r.Value;
					blocked = false;
				}
			},
			["jgz"] = (x, y) => curPos += GetVal(x) > 0 ? GetVal(y) : 1
		};

		bool Execute()
		{
			var groups = regex.Match(instructions[curPos]).Groups;
			var (ins, arg1, arg2) = (groups["op"].Value, groups["f"].Value, groups["s"].Value);
			commands[ins](arg1, arg2);
			if (!blocked && ins != "jgz") curPos++;
			if (curPos < 0 || curPos >= instructions.Length)
			{
				throw new Exception($"Program {id} exited");
			}

			return blocked;
		}

		return Execute;
	}

	public object Part2(IEnumerable<string> input)
	{
		var instructions = input.ToArray();
		var sent0 = new Queue<long>();
		var sent1 = new Queue<long>();
		var i0 = Interpreter(instructions, 0, n => sent0.Enqueue(n), () =>
		{
			if (sent1.Count == 0) return null;
				
			return sent1.Dequeue();
		});
		long totalSent1 = 0;
		var i1 = Interpreter(instructions, 1, n =>
		{
			sent1.Enqueue(n);
			totalSent1++;
		}, () =>
		{
			if (sent0.Count == 0) return null;

			return sent0.Dequeue();
		});

		do
		{
			RunUntilBlocked(i0);
			RunUntilBlocked(i1);
		} while (sent0.Count > 0 || sent1.Count > 0);
				
		return totalSent1.ToString();
	}
}