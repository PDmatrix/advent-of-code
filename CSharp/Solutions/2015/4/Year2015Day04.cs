using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._4
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day04 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var key = input.First();
			var number = 1;
			while (true)
			{
				var hash = CreateMd5($"{key}{number}");
				if(hash.StartsWith("00000"))
					break;
				number++;
			}
			return number.ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var key = input.First();
			var number = 1;
			while (true)
			{
				var hash = CreateMd5($"{key}{number}");
				if(hash.StartsWith("000000"))
					break;
				number++;
			}
			return number.ToString();
		}

		private static string CreateMd5(string input)
		{
			using (var md5 = MD5.Create())
			{
				var inputBytes = Encoding.ASCII.GetBytes(input);
				var hashBytes = md5.ComputeHash(inputBytes);
				var sb = new StringBuilder();
				foreach (var hashByte in hashBytes)
				{
					sb.Append(hashByte.ToString("X2"));
				}

				return sb.ToString();
			}
		}

	}
}