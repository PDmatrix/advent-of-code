using CommandLine;

namespace CliUtils.Options
{
	// ReSharper disable once ClassNeverInstantiated.Global
	[Verb("parse", HelpText = "Parse content as markdown")]
	public class ParseOptions
	{
		[Option('y', "year", Required = true)]
		public int Year { get; set; }
		
		[Option('d', "day", Required = true)]
		public int Day { get; set; }
	}
}