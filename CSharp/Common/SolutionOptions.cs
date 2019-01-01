using CommandLine;

namespace AdventOfCode.Common
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class SolutionOptions
	{
		[Option('y', "year", Required = true)]
		public int Year { get; set; }
		
		[Option('d', "day", Required = true)]
		public int Day { get; set; }
	}
}