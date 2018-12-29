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
			await File.WriteAllTextAsync(path, md, Encoding.UTF8);
		}
		
		private static async Task GenerateInputAsync(int year, int day, string path)
		{
			var input = await Parse.GetInputAsync(year, day);
			await File.WriteAllTextAsync(path, input, Encoding.UTF8);
		}
		
		public static async Task HandleGenerateAsync(int year, int day, string path, GenType genType)
		{
			switch (genType)
			{
				case GenType.Readme:
					await GenerateReadmeAsync(year, day, path);
					break;
				case GenType.Input:
					await GenerateInputAsync(year, day, path);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(genType), genType, null);
			}
		}
	}
}