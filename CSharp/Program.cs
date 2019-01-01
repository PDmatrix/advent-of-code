using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AdventOfCode.Common;
using CommandLine;

namespace AdventOfCode
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			Parser.Default.ParseArguments<SolutionOptions>(args)
				.WithParsed(opts => HandleSolution(opts.Year, opts.Day).Wait());
		}

		private static async Task HandleSolution(int year, int day)
		{
			var solution = GetSolution(year, day);
			var input = await GetInput(year, day);
			var enumerable = input as string[] ?? input.ToArray();
			Console.WriteLine($"First Part: {solution.Part1(enumerable)}");
			Console.WriteLine($"Second Part: {solution.Part2(enumerable)}");
		}

		private static ISolution GetSolution(int year, int day)
		{
			var interfaceType = typeof(ISolution);
			var allSolutions = Assembly.GetExecutingAssembly().GetTypes()
				.Where(r =>interfaceType.IsAssignableFrom(r) && r != interfaceType);
			var solutionType = allSolutions
				.FirstOrDefault(r => (int) r.GetField("Year").GetValue(null) == year
				                     && (int) r.GetField("Day").GetValue(null) == day);
			if (solutionType == null)
				throw new Exception("Solution not found");
			
			return (ISolution)Activator.CreateInstance(solutionType);
		}
		
		private static async Task<IEnumerable<string>> GetInput(int year, int day)
		{
			return await File.ReadAllLinesAsync(
				$"Input/{year}/Day{day.ToString().PadLeft(2, '0')}.in");
		}
	}
}