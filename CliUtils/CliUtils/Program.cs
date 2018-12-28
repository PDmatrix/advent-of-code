using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CliUtils.Options;
using CommandLine;

namespace CliUtils
{
	internal static class Program
	{
		private static async Task HandleParse(ParseOptions options)
		{
			var html = await Verbs.Parse.GetHtml(options.Year, options.Day);
			var md = await Verbs.Parse.HtmlToMd(html);
			Console.Write(md);
		}

		private static async Task HandleGen(GenerateOptions options)
		{
			Console.WriteLine();
		}

		private static async Task HandleError(IEnumerable<Error> errors)
		{
			Console.WriteLine(string.Join(", ", errors));
		}

		private static async Task Main(string[] args)
		{
			await Parser.Default.ParseArguments<ParseOptions, GenerateOptions>(args)
				.MapResult(
					async (ParseOptions opts) => await HandleParse(opts),
					async (GenerateOptions opts) => await HandleGen(opts),
					async errors => await HandleError(errors));
		}
	}
}