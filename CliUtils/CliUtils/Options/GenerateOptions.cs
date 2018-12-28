using CommandLine;

namespace CliUtils.Options
{
	[Verb("generate", HelpText = "Generate values")]
	public class GenerateOptions
	{
		[Option('y', "year")]
		public int Year { get; set; }
		
		[Option('d', "day")]
		public int Day { get; set; }
	}
}