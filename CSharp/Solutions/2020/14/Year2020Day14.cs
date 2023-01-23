using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._14;

[UsedImplicitly]
public class Year2020Day14 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var memory = new Dictionary<int, long>();
		var currentMask = string.Empty;
		var regex = new Regex(@"mem\[(?<address>\d+)\] = (?<value>\d+)", RegexOptions.Compiled);
		foreach (var line in input)
		{
			if (line.StartsWith("mask"))
			{
				currentMask = line.Split(" = ").Last();
				continue;
			}

			var match = regex.Match(line);
			var value = match.Groups["value"].Value;
			var binaryValue = Convert.ToString((long)Convert.ToUInt64(value), 2).PadLeft(36,'0');
			var sb = new StringBuilder();
			for (var i = 0; i < binaryValue.Length; i++)
			{
				sb.Append(currentMask[i] switch
				{
					'X' => binaryValue[i],
					'1' => '1',
					'0' => '0'
				});
			}
			
			memory[int.Parse(match.Groups["address"].Value)] = (long) Convert.ToUInt64(sb.ToString(), 2);
		}

		return memory.Sum(x => x.Value);
	}

	public object Part2(IEnumerable<string> input)
	{
		var memory = new Dictionary<long, long>();
		var currentMask = string.Empty;
		var regex = new Regex(@"mem\[(?<address>\d+)\] = (?<value>\d+)", RegexOptions.Compiled);
		foreach (var line in input)
		{
			if (line.StartsWith("mask"))
			{
				currentMask = line.Split(" = ").Last();
				continue;
			}

			var match = regex.Match(line);
			var value = match.Groups["value"].Value;
			var address = match.Groups["address"].Value;
			var binaryAddress = Convert.ToString((long)Convert.ToUInt64(address), 2).PadLeft(36,'0');
			var stringBuilders = new List<StringBuilder>
			{
				new()
			};
			
			for (var i = 0; i < binaryAddress.Length; i++)
			{
				if (currentMask[i] == 'X')
				{
					var copy = stringBuilders.Select(x => new StringBuilder(x.ToString())).ToList();
					foreach (var sb in stringBuilders)
						sb.Append('1');
					foreach (var sb in copy)
						sb.Append('0');
					stringBuilders.AddRange(copy);
					continue;
				}

				foreach (var sb in stringBuilders)
				{
					sb.Append(currentMask[i] switch
					{
						'X' => 'X',
						'1' => '1',
						'0' => binaryAddress[i]
					});
				}
			}

			var addresses = stringBuilders.Where(x => x.Length == 36).Select(x => x.ToString());
			foreach (var addr in addresses)
				memory[(long)Convert.ToUInt64(addr, 2)] = long.Parse(value);
		}

		return memory.Sum(x => x.Value);
	}
}