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
			Console.WriteLine($"First Part: {solution.Part1(enumerable.ToArray())}");
			Console.WriteLine($"Second Part: {solution.Part2(enumerable.ToArray())}");
		}

		private static ISolution GetSolution(int year, int day)
		{
			var interfaceType = typeof(ISolution);
			var allSolutions = Assembly.GetExecutingAssembly().GetTypes()
				.Where(r =>interfaceType.IsAssignableFrom(r) && r != interfaceType);
			var solutionType = allSolutions
				.FirstOrDefault(r =>
				{
					var splitted = r.Name.Split("Day");
					var typeYear = int.Parse(splitted.First().Replace("Year", string.Empty));
					var typeDay = int.Parse(splitted.Last());
					return typeYear == year  && typeDay == day;
				});
			if (solutionType == null)
				throw new Exception("Solution not found");

			var instance = Activator.CreateInstance(solutionType) ?? throw new Exception();

			return (ISolution) instance;
		}
		
		private static async Task<IEnumerable<string>> GetInput(int year, int day)
		{
			return await File.ReadAllLinesAsync(
				$"Input/{year}/Day{day.ToString().PadLeft(2, '0')}.in");
		}
	}
}