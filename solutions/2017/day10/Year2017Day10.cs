using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2017._10;

[UsedImplicitly]
public class Year2017Day10 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var list = Enumerable.Range(0, 256).ToArray();
		var lengths = input.First().Split(',').Select(int.Parse);
		var currentPosition = 0;
		var skipSize = 0;
		foreach (var length in lengths)
		{
			list = ReverseSection(list, currentPosition, length).ToArray();
			currentPosition += length + skipSize;
			skipSize++;
		}

		return (list[0] * list[1]).ToString();
	}
		
	public object Part2(IEnumerable<string> input)
	{
		var list = Enumerable.Range(0, 256).ToArray();
		var lengths = input.First().Select(r => (int) r).Concat(new[] {17, 31, 73, 47, 23}).ToArray();
		var currentPosition = 0;
		var skipSize = 0;
		for (var i = 1; i <= 64; i++)
		{
			foreach (var length in lengths)
			{
				list = ReverseSection(list, currentPosition, length).ToArray();
				currentPosition += length + skipSize;
				skipSize++;
			}
		}

		var hash = string.Join(string.Empty,
			list
				.Select((num, idx) => new {num, idx})
				.GroupBy(r => r.idx / 16)
				.Select(r => r.Aggregate(0, (acc, el) => acc ^ el.num))
				.Select(r => r.ToString("X").PadLeft(2, '0'))
		);
			
		return hash.ToLower();
	}
		
	private static IEnumerable<int> ReverseSection(IList<int> list, int position, int length)
	{
		var newList = new List<int>(list);
		for (var i = 0; i < length; i++)
		{
			newList[(i + position) % newList.Count] =
				list[(position + length - i - 1) % newList.Count];
		}

		return newList;
	}
}