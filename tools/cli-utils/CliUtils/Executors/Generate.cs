using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CliUtils.Options;

namespace CliUtils.Executors
{
	public static class Generate
	{
		private static async Task GenerateReadmeAsync(int year, int day, string path)
		{
			var md = await Parse.HtmlToMdAsync(await Parse.GetChallengeAsync(year, day));

			md = md
				.Replace("&lt;", "<")
				.Replace("&gt;", ">")
				.Replace("&nbsp;", " ");

			await File.WriteAllTextAsync(path, md, Encoding.UTF8);
		}

		private static async Task GenerateInputAsync(int year, int day, string path)
		{
			var input = await Parse.GetInputAsync(year, day);
			
			await File.WriteAllTextAsync(path, input, Encoding.UTF8);
		}
		
		public static Task HandleGenerateAsync(int year, int day, string path, GenType genType)
		{
			return genType switch
			{
				GenType.Readme => GenerateReadmeAsync(year, day, path),
				GenType.Input => GenerateInputAsync(year, day, path),
				_ => throw new ArgumentOutOfRangeException(nameof(genType), genType, null)
			};
		}
	}
}