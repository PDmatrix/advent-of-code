using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CliUtils.Executors;
using CliUtils.Options;
using CommandLine;

namespace CliUtils
{
	internal static class Program
	{
		private static async Task HandleParse(ParseOptions options)
		{
			var html = await Parse.GetChallengeAsync(options.Year, options.Day);
			var md = await Parse.HtmlToMdAsync(html);
			Console.Write(md);
		}

		private static async Task HandleGen(GenerateOptions options)
		{
			await Generate.HandleGenerateAsync(
				options.Year, options.Day, options.Path, options.GenType);
		}

		private static async Task HandleError(IEnumerable<Error> errors)
		{
			Console.WriteLine(string.Join(", ", errors));
		}

		private static async Task Main(string[] args)
		{
			var parser = new Parser(cfg => cfg.CaseInsensitiveEnumValues = true);
			await parser.ParseArguments<ParseOptions, GenerateOptions>(args)
				.MapResult(
					async (ParseOptions opts) => await HandleParse(opts),
					async (GenerateOptions opts) => await HandleGen(opts),
					async errors => await HandleError(errors));
		}
	}
}