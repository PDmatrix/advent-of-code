using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._11;

[UsedImplicitly]
public class Year2022Day11 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		/*
		input = new[]
		{
			"Monkey 0:",
			"  Starting items: 79, 98",
			"  Operation: new = old * 19",
			"  Test: divisible by 23",
			"	 If true: throw to monkey 2",
			"	 If false: throw to monkey 3",
			"",
			"Monkey 1:",
			"  Starting items: 54, 65, 75, 74",
			"  Operation: new = old + 6",
			"  Test: divisible by 19",
			"	 If true: throw to monkey 2",
			"	 If false: throw to monkey 0",
			"",
			"Monkey 2:",
			"  Starting items: 79, 60, 97",
			"  Operation: new = old * old",
			"  Test: divisible by 13",
			"	 If true: throw to monkey 1",
			"	 If false: throw to monkey 3",
			"",
			"Monkey 3:",
			"  Starting items: 74",
			"  Operation: new = old + 3",
			"  Test: divisible by 17",
			"	 If true: throw to monkey 0",
			"	 If false: throw to monkey 1",
		};
		*/
		
		var monkeys = ParseInput(input).ToList();
		for (var round = 0; round < 20; round++)
		{
			foreach (var monkey in monkeys)
			{
				while (monkey.Items.Count != 0)
				{
					var item = monkey.Items.Dequeue();
					monkey.ItemsTouched++;
					switch (monkey.Op)
					{
						case "*":
							item *= monkey.LeftArg == "old" ? item : int.Parse(monkey.LeftArg);
							break;
						case "+":
							item += monkey.LeftArg == "old" ? item : int.Parse(monkey.LeftArg);
							break;
					}

					item /= 3;
					if (monkey.Test(item))
						monkeys[monkey.IfTrue].Items.Enqueue(item);
					else
						monkeys[monkey.IfFalse].Items.Enqueue(item);
				}
			}
		}

		var itemsTouched = monkeys.Select(x => x.ItemsTouched).ToList();
		itemsTouched.Sort();
		
		return itemsTouched[^1] * itemsTouched[^2];
	}
	
	public object Part2(IEnumerable<string> input)
	{
		var monkeys = ParseInput(input).ToList();
		var globalModulo = monkeys.Select(m => m.Divisor).Aggregate((m,i) => m*i);
		
		for (var round = 0; round < 10000; round++)
		{
			foreach (var monkey in monkeys)
			{
				while (monkey.Items.Count != 0)
				{
					var item = monkey.Items.Dequeue();
					monkey.ItemsTouched++;
					switch (monkey.Op)
					{
						case "*":
							item *= monkey.LeftArg == "old" ? item : int.Parse(monkey.LeftArg);
							break;
						case "+":
							item += monkey.LeftArg == "old" ? item : int.Parse(monkey.LeftArg);
							break;
					}

					item %= globalModulo;
					if (monkey.Test(item))
						monkeys[monkey.IfTrue].Items.Enqueue(item);
					else
						monkeys[monkey.IfFalse].Items.Enqueue(item);
				}
			}
		}

		var itemsTouched = monkeys.Select(x => x.ItemsTouched).ToList();
		itemsTouched.Sort();
		
		return itemsTouched[^1] * itemsTouched[^2];
	}
	
	private static IEnumerable<Monkey> ParseInput(IEnumerable<string> input)
	{
		var monkeys = new List<Monkey>();
		var currentMonkey = new Monkey();
		foreach (var line in input)
		{
			if (string.IsNullOrEmpty(line))
			{
				monkeys.Add(currentMonkey);
				continue;
			}
			
			if (line.StartsWith("Monkey"))
			{
				currentMonkey = new Monkey();
				continue;
			}

			var trimmed = line.Trim();
			if (trimmed.StartsWith("Starting"))
			{
				var items = trimmed.Split(": ")[1].Split(", ").Select(int.Parse);
				foreach (var item in items)
					currentMonkey.Items.Enqueue(item);
				continue;
			}
			
			if (trimmed.StartsWith("Operation"))
			{
				var expr = trimmed.Split(" = ")[1];
				currentMonkey.RightArg = expr.Split()[0];
				currentMonkey.Op = expr.Split()[1];
				currentMonkey.LeftArg = expr.Split()[2];
				continue;
			}
			
			if (trimmed.StartsWith("Test"))
			{
				var by = int.Parse(trimmed.Split().Last());
				currentMonkey.Test = c => c % by == 0;
				currentMonkey.Divisor = by;
				continue;
			}
			
			if (trimmed.StartsWith("If true"))
			{
				currentMonkey.IfTrue = int.Parse(trimmed.Split().Last());
				continue;
			}
			
			if (trimmed.StartsWith("If false"))
			{
				currentMonkey.IfFalse = int.Parse(trimmed.Split().Last());
				continue;
			}
		}
		
		monkeys.Add(currentMonkey);

		return monkeys;
	}
	
	private class Monkey
	{
		public Queue<long> Items { get; set; } = new();
		public Predicate<long> Test { get; set; }
		public long ItemsTouched { get; set; }
		public int Divisor { get; set; }

		public string LeftArg { get; set; }
		public string RightArg { get; set; }
		public string Op { get; set; }

		public int IfTrue { get; set; }
		public int IfFalse { get; set; }
	}
}