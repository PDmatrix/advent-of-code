using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._10;

[UsedImplicitly]
public class Year2021Day10 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var pointsTable = new Dictionary<char, int>
		{
			[')'] = 3,
			[']'] = 57,
			['}'] = 1197,
			['>'] = 25137
		};
		var opening = new HashSet<char>()
		{
			'(', '[', '{', '<'
		};
		var lookup = new Dictionary<char, char>
		{
			[')'] = '(',
			[']'] = '[',
			['}'] = '{',
			['>'] = '<'
		};
		var answer = 0;
		foreach (var line in input)
		{
			var stack = new Stack<char>();
			foreach (var c in line)
			{
				if (opening.Contains(c))
				{
					stack.Push(c);
					continue;
				}

				var prev = stack.Pop();
				if (prev == lookup[c]) continue;

				answer += pointsTable[c];
				break;
			}
		}

		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var opening = new HashSet<char>()
		{
			'(', '[', '{', '<'
		};
		var lookup = new Dictionary<char, char>
		{
			[')'] = '(',
			[']'] = '[',
			['}'] = '{',
			['>'] = '<'
		};
		var incomplete = new Dictionary<string, Stack<char>>();
		foreach (var line in input)
		{
			var stack = new Stack<char>();
			var isFound = true;
			foreach (var c in line)
			{
				if (opening.Contains(c))
				{
					stack.Push(c);
					continue;
				}

				var prev = stack.Pop();
				if (prev == lookup[c]) continue;

				isFound = false;
				break;
			}

			if (isFound)
				incomplete.Add(line, stack);
		}

		var completions = new List<long>();
		var negativeLookup = new Dictionary<char, char>
		{
			['('] = ')',
			['['] = ']',
			['{'] = '}',
			['<'] = '>'
		};
		var points = new Dictionary<char, int>()
		{
			[')'] = 1,
			[']'] = 2,
			['}'] = 3,
			['>'] = 4
		};
		foreach (var kv in incomplete)
		{
			long score = 0;
			while (kv.Value.Count != 0)
			{
				var current = kv.Value.Pop();
				score *= 5;
				score += points[negativeLookup[current]];
			}

			completions.Add(score);
		}

		completions.Sort();
		return completions[completions.Count / 2];
	}
}