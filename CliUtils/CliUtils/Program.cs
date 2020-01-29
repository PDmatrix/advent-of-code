using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CliUtils.Executors;
using CliUtils.Options;
using CommandLine;
using CommandLine.Text;

namespace CliUtils
{
	internal static class Program
	{
		private static async Task HandleParseAsync(ParseOptions options)
		{
			var html = await Parse.GetChallengeAsync(options.Year, options.Day);
			var md = await Parse.HtmlToMdAsync(html);
			Console.Write(md);
		}

		private static async Task HandleGenAsync(GenerateOptions options)
		{
			await Generate.HandleGenerateAsync(
				options.Year, options.Day, options.Path, options.GenType);
		}

		private static Task HandleError(IEnumerable<Error> errors)
		{
			Console.WriteLine(string.Join(", ", errors));
			
			return Task.CompletedTask;
		}
		
		private static Task Main(string[] args)
		{
			var parser = new Parser(cfg => cfg.CaseInsensitiveEnumValues = true);
			
			return parser.ParseArguments<ParseOptions, GenerateOptions>(args)
				.MapResult(
					(ParseOptions opts) => HandleParseAsync(opts),
					(GenerateOptions opts) => HandleGenAsync(opts),
					HandleError);
		}
	}
}