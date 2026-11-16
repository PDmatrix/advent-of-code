using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._15;

[UsedImplicitly]
public class Year2023Day15 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var words = input.First().Split(',').ToList();

		return words.Sum(Hash);
	}

	public object Part2(IEnumerable<string> input)
	{
		var words = input.First().Split(',').ToList();
		
		var boxes = new Dictionary<int, LinkedList<(string, int)>>();

		var equalRegex = new Regex(@"(?<d>\w+)=(?<v>\d)");
		var minusRegex = new Regex(@"(?<d>\w+)-");
		foreach (var word in words)
		{
			var equalMatch = equalRegex.Match(word);
			var minusMatch = minusRegex.Match(word);
			if (equalMatch.Success)
			{
				var d = equalMatch.Groups["d"].Value;
				var v = int.Parse(equalMatch.Groups["v"].Value);
				var h = Hash(d);
				if (!boxes.ContainsKey(h))
					boxes[h] = new LinkedList<(string, int)>();
				
				var node = boxes[h].First;
				var found = false;
				while (node != null)
				{
					if (node.Value.Item1 == d)
					{
						found = true;
						node.Value = (d, v);
						break;
					}
					node = node.Next;
				}
				if (!found)
					boxes[h].AddLast((d, v));
			}
			else if (minusMatch.Success)
			{
				var d = minusMatch.Groups["d"].Value;
				var h = Hash(d);
				if (!boxes.TryGetValue(h, out var box))
					continue;

				if (box.Count <= 0) 
					continue;
				
				var node = box.First;
				while (node != null)
				{
					if (node.Value.Item1 == d)
					{
						boxes[h].Remove(node);
						break;
					}

					node = node.Next;
				}
			}
		}

		var result = 0;
		foreach (var key in boxes.Keys)
		{
			if (boxes[key].Count == 0)
				continue;

			var slot = 0;
			var node = boxes[key].First;
			while (node != null)
			{
				slot++;
				result += (key + 1) * slot * node.Value.Item2;
				node = node.Next;
			}
		}
		
		return result;
	}

	private static int Hash(string w)
	{
		var val = 0;
		foreach (var c in w)
		{
			val += c;
			val *= 17;
			val %= 256;
		}
		
		return val;
	}
}