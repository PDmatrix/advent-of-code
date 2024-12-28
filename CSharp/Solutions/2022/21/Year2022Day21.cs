using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using CSharpx;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._21;

[UsedImplicitly]
public partial class Year2022Day21 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		// input = new[]
		// {
		// 	"root: pppw + sjmn",
		// 	"dbpl: 5",
		// 	"cczh: sllz + lgvd",
		// 	"zczc: 2",
		// 	"ptdq: humn - dvpt",
		// 	"dvpt: 3",
		// 	"lfqf: 4",
		// 	"humn: 5",
		// 	"ljgn: 2",
		// 	"sjmn: drzm * dbpl",
		// 	"sllz: 4",
		// 	"pppw: cczh / lfqf",
		// 	"lgvd: ljgn * ptdq",
		// 	"drzm: hmdt - zczc",
		// 	"hmdt: 32",
		// };
		
		var jobs = ParseInput(input);
		
		return GetResult(jobs, "root");
	}

	public object Part2(IEnumerable<string> input)
	{
		var jobs = ParseInput(input);
		jobs["root"] = new Job(jobs["root"].A, jobs["root"].B, "=", 0);

		var rootA = jobs["root"].A;
		var rootB = jobs["root"].B;

		long low = 1;
		var high = 5_000_000_000_000;
		
		while (low < high)
		{
			var current = low + (high - low) / 2;
			
			var copyJobs = new Dictionary<string, Job>(jobs);
			copyJobs["humn"] = new Job("", "", "", current);
			var r = GetResult(copyJobs, "root");
			
			if (copyJobs[rootA].Result > copyJobs[rootB].Result)
				low = current - 1;
			else
				high = current + 1;
			
			if (r == 1)
				return current;
		}
		
		throw new Exception("Solution not found");
	}

	private static long GetResult(Dictionary<string, Job> jobs, string root)
	{
		if (jobs[root].Op == "")
			return jobs[root].Result;
		
		var a = GetResult(jobs, jobs[root].A);
		var b = GetResult(jobs, jobs[root].B);

		var result = jobs[root].Op switch
		{
			"+" => a + b,
			"-" => a - b,
			"*" => a * b,
			"/" => a / b,
			"=" => a == b ? 1 : 0
		};
		
		jobs[root] = new Job("", "", "", result);
		
		return result;
	}
	
	private static Dictionary<string, Job> ParseInput(IEnumerable<string> input)
	{
		var jobs = new Dictionary<string, Job>();
		foreach (var line in input)
		{
			var split = line.Split(": ");
			if (long.TryParse(split[1], out var sp))
			{
				jobs[split[0]] = new Job("", "", "", sp);
				continue;
			}
			
			var split2 = split[1].Split(" ");
			jobs[split[0]] = new Job(split2[0], split2[2], split2[1], 0);
		}

		return jobs;
	}

	private record struct Job(string A, string B, string Op, long Result);
}