using CommandLine;

namespace CliUtils.Options
{
	[Verb("parse", HelpText = "Parse content as markdown")]
	public class ParseOptions
	{
		[Option('y', "year")]
		public int Year { get; set; }
		
		[Option('d', "day")]
		public int Day { get; set; }
	}
}