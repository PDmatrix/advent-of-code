using CommandLine;

namespace CliUtils.Options
{
	public enum GenType
	{
		Readme,
		Input
	}
	
	// ReSharper disable once ClassNeverInstantiated.Global
	[Verb("generate", HelpText = "Generate values")]
	public class GenerateOptions
	{
		[Option('y', "year", Required = true)]
		public int Year { get; set; }
		
		[Option('d', "day", Required = true)]
		public int Day { get; set; }
		
		[Option('t', "type", Required = true)]
		public GenType GenType { get; set; }
		
		[Option('p', "path", Required = true)]
		public string Path { get; set; }
	}
}