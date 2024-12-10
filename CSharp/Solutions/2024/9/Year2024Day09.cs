using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._9;

[UsedImplicitly]
public class Year2024Day09 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var diskMap = input.First();

		var disk = new List<string>();
		var isDisk = true;
		var diskNum = 0;
		foreach (var d in diskMap)
		{
			var parsed = int.Parse(d.ToString());
			if (isDisk)
			{
				for (var i = 0; i < parsed; i++)
					disk.Add(diskNum.ToString());
				isDisk = false;
				diskNum++;
				continue;
			}

			for (var i = 0; i < parsed; i++)
				disk.Add(".");
			isDisk = true;
		}

		var left = 0;
		var right = disk.Count - 1;
		while (left < right)
		{
			if (disk[left] != ".")
			{
				left++;
				continue;
			}
			
			if (disk[right] == ".")
			{
				right--;
				continue;
			}
			
			disk[left] = disk[right];
			disk[right] = ".";
		}

		long answer = 0;
		for (var i = 0; i < disk.Count; i++)
		{
			if (disk[i] == ".")
				break;
			
			answer += int.Parse(disk[i]) * i;
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var diskMap = input.First();

		var isDisk = true;
		var files = new Dictionary<int, (int startPos, int len)>();
		var gaps = new Dictionary<int, PriorityQueue<int, int>>();
		var pos = 0;
		var diskNum = 0;
		foreach (var d in diskMap)
		{
			var parsed = int.Parse(d.ToString());
			if (isDisk)
			{
				files.Add(diskNum, (pos, parsed));
				isDisk = false;
				pos += parsed;
				diskNum++;
				continue;
			}

			if (parsed != 0)
			{
				if (!gaps.ContainsKey(parsed))
					gaps[parsed] = new PriorityQueue<int, int>();

				gaps[parsed].Enqueue(pos, pos);
			}

			pos += parsed;
			isDisk = true;
		}

		foreach (var file in files.Keys.OrderByDescending(x => x).ToList())
		{
			var possibleGaps = gaps
				.Where(x => x.Key >= files[file].len)
				.Select(x => (Start: x.Value.Peek(), Length: x.Key))
				.OrderBy(x => x.Item1)
				.FirstOrDefault();
			
			if (possibleGaps == default)
				continue;

			if (files[file].startPos < possibleGaps.Start)
				continue;

			files[file] = (possibleGaps.Start, files[file].len);
			var remainingGapLen = possibleGaps.Length - files[file].len;
			gaps[possibleGaps.Length].Dequeue();
			if (gaps[possibleGaps.Length].Count == 0)
				gaps.Remove(possibleGaps.Length);

			if (remainingGapLen <= 0)
				continue;
			
			if (!gaps.ContainsKey(remainingGapLen))
				gaps[remainingGapLen] = new PriorityQueue<int, int>();
			gaps[remainingGapLen].Enqueue(possibleGaps.Start + files[file].len, possibleGaps.Start + files[file].len);
		}
		
		return files.Sum(item =>
		{
			var (start, length) = item.Value;
			return item.Key * (start * length + (long)length * (length - 1) / 2);
		});
	}
}