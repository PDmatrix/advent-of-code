using System.Collections.Generic;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._8
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day08 : ISolution
	{
		// ReSharper disable once UnusedMember.Global
		public static int Year = 2015;
		// ReSharper disable once UnusedMember.Global
		public static int Day = 8;
		
		public string Part1(IEnumerable<string> lines)
		{
			int numberOfCharsLiteral = 0, numberOfCharsMemory = 0;
			foreach (var line in lines)
			{
				numberOfCharsLiteral += line.Length;
				var mutable = line.Substring(1, line.Length - 2);
				var idx = 0;
				while (idx <= mutable.Length - 1)
				{
					switch (mutable[idx])
					{
						case '\\' when idx + 1 != mutable.Length - 1 && mutable[idx + 1] == 'x':
							idx += 3;
							break;
						case '\\':
							idx++;
							break;
					}

					idx++;
					numberOfCharsMemory++;
				}
			}
			return (numberOfCharsLiteral - numberOfCharsMemory).ToString();
		}

		public string Part2(IEnumerable<string> lines)
		{
			int numberOfCharsLiteral = 0, numberOfNewCharsLiteral = 0;
			foreach (var line in lines)
			{
				numberOfCharsLiteral += line.Length;
				var mutable = line;
				foreach (var c in mutable)
				{
					if (c == '"' || c == '\\')
					{
						numberOfNewCharsLiteral++;
					}
					numberOfNewCharsLiteral++;
				}

				numberOfNewCharsLiteral += 2;
			}
			return (numberOfNewCharsLiteral - numberOfCharsLiteral).ToString();
		}
	}
}