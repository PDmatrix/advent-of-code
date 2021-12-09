using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using CSharpx;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2018._2;

[UsedImplicitly]
public class Year2018Day02 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var a = 0;
		var b = 0;
		foreach (var s in input)
		{
			var dict = new Dictionary<char, int>();
			s.ForEach(x =>
			{
				if (!dict.ContainsKey(x))
				{
					dict.Add(x, 1);
					return;
				}

				dict[x] += 1;
			});
				
			if (dict.ContainsValue(2))
				a += 1;
				
			if (dict.ContainsValue(3))
				b += 1;
		}

		return (a * b).ToString();
	}

	public object Part2(IEnumerable<string> input)
	{
		var arr = input.ToArray();
		for (var i = 0; i < arr.Length; i++)
		{
			for (var j = i + 1; j < arr.Length; j++)
			{
				if (IsDifferent(arr[i], arr[j]))
					return GetResult(arr[i], arr[j]);
			}
		}
		return "error";
	}

	private static string GetResult(string s, string s1)
	{
		var result = new StringBuilder();
		for (var i = 0; i < s.Length; i++)
		{
			if (s[i] == s1[i])
				result.Append(s[i]);
		}

		return result.ToString();
	}

	private static bool IsDifferent(string s, string s1)
	{
		var cnt = 0;
		for (var i = 0; i < s.Length; i++)
		{
			if (s[i] != s1[i])
				cnt++;

			if (cnt > 1)
				return false;
		}

		return true;
	}
}