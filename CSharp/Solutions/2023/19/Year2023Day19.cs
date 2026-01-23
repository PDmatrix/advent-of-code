using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._19;

[UsedImplicitly]
public class Year2023Day19 : ISolution
{
	private static int _accepted;
	
	public object Part1(IEnumerable<string> input)
	{
		var (workflows, parts) = ParseInput(input);
		
		foreach (var part in parts)
		{
			var currentWorkflow = "in";
			while (true)
			{
				var conditions = workflows[currentWorkflow];
				var nextWorkflow = "";
				foreach (var condition in conditions)
				{
					nextWorkflow = condition(part);
					if (!string.IsNullOrEmpty(nextWorkflow))
						break;
				}

				if (string.IsNullOrEmpty(nextWorkflow))
					break;

				currentWorkflow = nextWorkflow;
			}
		}

		return _accepted;
	}
	
	public object Part2(IEnumerable<string> input)
	{
		var workflows = ParseInputPart2(input);
		
		var queue = new Queue<(string, PartRange, int)>();
		queue.Enqueue(("in", new PartRange(1, 4000, 1, 4000, 1, 4000, 1, 4000), 0));

		ulong ans = 0;
		while (queue.Count > 0)
		{
			var (currentWorkflow, range, conditionIdx) = queue.Dequeue();
			if (currentWorkflow == "R")
				continue;
			
			if (currentWorkflow == "A")
			{
				ans += (ulong)(range.EndX - range.StartX + 1) *
				        (ulong)(range.EndM - range.StartM + 1) *
				         (ulong)(range.EndA - range.StartA + 1) *
				          (ulong)(range.EndS - range.StartS + 1);
				continue;
			}
			
			var condition = workflows[currentWorkflow][conditionIdx];
			switch (condition.Operator)
			{
				case ">":
				{
					var newTrueRange = condition.Field switch
					{
						"x" => range with { StartX = Math.Max(range.StartX, condition.Value + 1) },
						"m" => range with { StartM = Math.Max(range.StartM, condition.Value + 1) },
						"a" => range with { StartA = Math.Max(range.StartA, condition.Value + 1) },
						"s" => range with { StartS = Math.Max(range.StartS, condition.Value + 1) },
						_ => range
					};
					queue.Enqueue((condition.NextWorkflow, newTrueRange, 0));
				
					var newFalseRange = condition.Field switch
					{
						"x" => range with { EndX = Math.Min(range.EndX, condition.Value) },
						"m" => range with { EndM = Math.Min(range.EndM, condition.Value) },
						"a" => range with { EndA = Math.Min(range.EndA, condition.Value) },
						"s" => range with { EndS = Math.Min(range.EndS, condition.Value) },
						_ => range
					};
					queue.Enqueue((currentWorkflow, newFalseRange, conditionIdx+1));
					break;
				}
				case "<":
				{
					var newTrueRange = condition.Field switch
					{
						"x" => range with { EndX = Math.Min(range.EndX, condition.Value - 1) },
						"m" => range with { EndM = Math.Min(range.EndM, condition.Value - 1) },
						"a" => range with { EndA = Math.Min(range.EndA, condition.Value - 1) },
						"s" => range with { EndS = Math.Min(range.EndS, condition.Value - 1) },
						_ => range
					};
					queue.Enqueue((condition.NextWorkflow, newTrueRange, 0));
				
					var newFalseRange = condition.Field switch
					{
						"x" => range with { StartX = Math.Max(range.StartX, condition.Value) },
						"m" => range with { StartM = Math.Max(range.StartM, condition.Value) },
						"a" => range with { StartA = Math.Max(range.StartA, condition.Value) },
						"s" => range with { StartS = Math.Max(range.StartS, condition.Value) },
						_ => range
					};
					queue.Enqueue((currentWorkflow, newFalseRange, conditionIdx+1));
					break;
				}
				default:
					queue.Enqueue((condition.NextWorkflow, range, 0));
					break;
			}
		}

		return ans;
	}

	private static (Dictionary<string, List<Func<Part, string>>>, List<Part>) ParseInput(IEnumerable<string> input)
	{
		var accepted = new Func<Part, string>(p =>
		{
			_accepted += p.X + p.M + p.A + p.S;
			return "";
		});
		var rejected = new Func<Part, string>(p => "");

		var workflows = new Dictionary<string, List<Func<Part, string>>>
		{
			["A"] = [accepted],
			["R"] = [rejected]
		};
		var parts = new List<Part>();

		var processingParts = false;
		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				processingParts = true;
				continue;
			}
			
			if (processingParts)
			{
				var regex = new Regex(@"{x=(?<x>\d+),m=(?<m>\d+),a=(?<a>\d+),s=(?<s>\d+)}");
				var match = regex.Match(line);
				parts.Add(new Part(
					X: int.Parse(match.Groups["x"].ToString()), 
					M: int.Parse(match.Groups["m"].ToString()), 
					A: int.Parse(match.Groups["a"].ToString()), 
					S: int.Parse(match.Groups["s"].ToString()))
				);
			}
			else
			{
				var splitted = line.Split('{');
				var wfName = splitted[0];
				var conditions = splitted[1][..^1].Split(',', StringSplitOptions.RemoveEmptyEntries);
				var conditionsList = new List<Func<Part, string>>();
				foreach (var condition in conditions)
				{
					if (condition.Contains(':'))
					{
						var regex = new Regex(@"(?<a>\w)(?<b>[><])(?<c>\d+):(?<d>\w+)");
						var match = regex.Match(condition);
						conditionsList.Add((p) =>
						{
							if (match.Groups["b"].ToString() == ">" && p.GetValue(match.Groups["a"].ToString()) > int.Parse(match.Groups["c"].ToString(), CultureInfo.InvariantCulture) ||
							    match.Groups["b"].ToString() == "<" && p.GetValue(match.Groups["a"].ToString()) < int.Parse(match.Groups["c"].ToString(), CultureInfo.InvariantCulture))
							{
								return match.Groups["d"].ToString();
							}

							return "";
						});
					}
					else
					{
						conditionsList.Add((_) => condition);
					}
				}
				workflows.Add(wfName, conditionsList);
			}
		}

		return (workflows, parts);
	}
	
	private static Dictionary<string, List<Condition>> ParseInputPart2(IEnumerable<string> input)
	{
		var d = new Dictionary<string, List<Condition>>();
		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
				break;

			var splitted = line.Split('{');
			var wfName = splitted[0];
			var conditions = splitted[1][..^1].Split(',', StringSplitOptions.RemoveEmptyEntries);
			var conditionsList = new List<Condition>();
			foreach (var condition in conditions)
			{
				if (condition.Contains(':'))
				{
					var regex = new Regex(@"(?<a>\w)(?<b>[><])(?<c>\d+):(?<d>\w+)");
					var match = regex.Match(condition);
					conditionsList.Add(new Condition(
						Field: match.Groups["a"].ToString(),
						Operator: match.Groups["b"].ToString(),
						Value: int.Parse(match.Groups["c"].ToString(), CultureInfo.InvariantCulture),
						NextWorkflow: match.Groups["d"].ToString()
					));
				}
				else
				{
					conditionsList.Add(new Condition(
						Field: "",
						Operator: "",
						Value: 0,
						NextWorkflow: condition
					));
				}
			}
			
			d[wfName] = conditionsList;
		}
		
		return d;
	}
	
	private readonly record struct Condition(string Field, string Operator, int Value, string NextWorkflow);

	private readonly record struct PartRange(int StartX, int EndX, int StartM, int EndM, int StartA, int EndA, int StartS, int EndS);

	private readonly record struct Part(int X, int M, int A, int S)
	{
		public int GetValue(string c) => c switch
		{
			"x" => X,
			"m" => M,
			"a" => A,
			"s" => S,
			_ => throw new ArgumentException("Invalid part property: " + c)
		};
	}
}