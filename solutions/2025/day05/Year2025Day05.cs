using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._5;

[UsedImplicitly]
public class Year2025Day05 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (ranges, numbers) = ParseInput(input);

		var answer = 0;
		foreach (var number in numbers)
		{
			foreach (var range in ranges)
			{
				if (number >= range.from && number <= range.to)
				{
					answer++;
					break;
				}
			}
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var (ranges, _) = ParseInput(input);
		ranges.Sort((a, b) => a.from.CompareTo(b.from));
		var removed = RemoveOverlaps(ranges);
		while (removed > 0)
		{
			ranges = ranges.Where(x => (x.to - x.from + 1) > 0).ToList();
			removed = RemoveOverlaps(ranges);
		}
		
		return ranges.Sum(x => x.to - x.from + 1);
	}
	
	private static int RemoveOverlaps(List<(long from, long to)> ranges)
	{
		var removed = 0;
		
		for (var i = 0; i < ranges.Count - 1; i++)
		{
			if (ranges[i].to < ranges[i + 1].from)
				continue;
			
			ranges[i + 1] = (ranges[i].to + 1, ranges[i + 1].to);
			removed++;
		}
		
		return removed;
	}
	
	private static (List<(long from, long to)>, List<long>) ParseInput(IEnumerable<string> input)
	{
		var sections = SplitByEmptyLine(input).ToList();
		var ranges = sections[0]
			.Select(line =>
			{
				var parts = line.Split('-');
				return (from: long.Parse(parts[0]), to: long.Parse(parts[1]));
			})
			.ToList();
		var numbers = sections[1].Select(long.Parse).ToList();
		return (ranges, numbers);
	}
	
	private static IEnumerable<List<T>> SplitByEmptyLine<T>(IEnumerable<T> source)
	{
		var group = new List<T>();
		foreach (var item in source)
		{
			if (item == null || item.Equals(default(T)) || item.Equals((T)(object)""))
			{
				if (group.Count > 0)
				{
					yield return group;
					group = new List<T>();
				}
			}
			else
			{
				group.Add(item);
			}
		}

		if (group.Count > 0)
			yield return group;
	}
}