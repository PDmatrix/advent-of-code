using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2017._9;

[UsedImplicitly]
public class Year2017Day09 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var str = input.First();
		str = StripIgnored(str);
		RemoveGarbage(ref str);
		str = str.Replace(",", "");
		return CountGroups(str).ToString();
	}
		
	public object Part2(IEnumerable<string> input)
	{
		var str = input.First();
		str = StripIgnored(str);
		return RemoveGarbageWithCount(ref str).ToString();
	}

	private static int CountGroups(string str)
	{
		var stack = new Stack<char>();
		var result = 0;
		foreach (var c in str)
		{
			if (c == '{')
			{
				stack.Push(c);
				continue;
			}

			result += stack.Count;
			stack.Pop();
		}

		return result;
	}

	private static void RemoveGarbage(ref string str)
	{
		while (true)
		{
			var startIndex = str.IndexOf('<');
			if (startIndex == -1) return;
			var endIndex = str.IndexOf('>');
			str = str.Remove(startIndex, endIndex - startIndex + 1);
		}
	}
		
	private static int RemoveGarbageWithCount(ref string str)
	{
		var result = 0;
		while (true)
		{
			var startIndex = str.IndexOf('<');
			if (startIndex == -1) return result;
			var endIndex = str.IndexOf('>');
			str = str.Remove(startIndex, endIndex - startIndex + 1);
			result += endIndex - startIndex - 1;
		}
	}

	private static string StripIgnored(string str)
	{
		var sb = new StringBuilder(str);
		var i = 0;
		while (i < sb.Length)
		{
			if (sb[i] != '!')
			{
				i++;
				continue;
			}

			sb[i] = '@';
			sb[i + 1] = '@';
			i += 2;
		}
		return sb.ToString().Replace("@", "");
	}
}