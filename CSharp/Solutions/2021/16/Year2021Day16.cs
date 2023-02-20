using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2021._16;

[UsedImplicitly]
public class Year2021Day16 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var hexToBin = new Dictionary<char, string>
		{
			['0'] = "0000",
			['1'] = "0001",
			['2'] = "0010",
			['3'] = "0011",
			['4'] = "0100",
			['5'] = "0101",
			['6'] = "0110",
			['7'] = "0111",
			['8'] = "1000",
			['9'] = "1001",
			['A'] = "1010",
			['B'] = "1011",
			['C'] = "1100",
			['D'] = "1101",
			['E'] = "1110",
			['F'] = "1111",
		};
		var sb = new StringBuilder();
		foreach (var el in input.First())
		{
			sb.Append(hexToBin[el]);
		}

		var bin = sb.ToString();
		var index = 0;
		return Eval(bin, ref index);
	}

	public object Part2(IEnumerable<string> input)
	{
		var hexToBin = new Dictionary<char, string>
		{
			['0'] = "0000",
			['1'] = "0001",
			['2'] = "0010",
			['3'] = "0011",
			['4'] = "0100",
			['5'] = "0101",
			['6'] = "0110",
			['7'] = "0111",
			['8'] = "1000",
			['9'] = "1001",
			['A'] = "1010",
			['B'] = "1011",
			['C'] = "1100",
			['D'] = "1101",
			['E'] = "1110",
			['F'] = "1111",
		};
		var sb = new StringBuilder();
		foreach (var el in input.First())
		{
			sb.Append(hexToBin[el]);
		}

		var bin = sb.ToString();
		var index = 0;
		return EvalV2(bin, ref index);
	}

	private static int Eval(string bin, ref int index)
	{
		var sb = new StringBuilder();
		sb.Append('0');
		for (var i = index; i < index + 3; i++)
			sb.Append(bin[i]);
		var version = Convert.ToInt32(sb.ToString(), 2);
		index += 3;

		sb.Clear();
		sb.Append('0');
		for (var i = index; i < index + 3; i++)
			sb.Append(bin[i]);
		var type = Convert.ToInt32(sb.ToString(), 2);
		index += 3;

		if (type == 4)
		{
			var notLast = true;
			while (notLast)
			{
				notLast = bin[index] == '1';
				index += 5;
			}
		}
		else
		{
			var lengthType = bin[index];
			index++;

			if (lengthType == '1')
			{
				sb.Clear();
				for (int i = index; i < index + 11; i++)
					sb.Append(bin[i]);
				index += 11;
				var len = Convert.ToInt32(sb.ToString(), 2);
				for (var i = 0; i < len; i++)
					version += Eval(bin, ref index);
			}
			else
			{
				sb.Clear();
				for (int i = index; i < index + 15; i++)
					sb.Append(bin[i]);
				index += 15;
				var len = Convert.ToInt32(sb.ToString(), 2);
				var curIndex = index;
				while (index - curIndex < len)
				{
					version += Eval(bin, ref index);
				}
			}
		}

		return version;
	}

	private static long EvalV2(string bin, ref int index)
	{
		var sb = new StringBuilder();
		sb.Append('0');
		for (var i = index; i < index + 3; i++)
			sb.Append(bin[i]);
		index += 3;

		sb.Clear();
		sb.Append('0');
		for (var i = index; i < index + 3; i++)
			sb.Append(bin[i]);
		var type = Convert.ToInt32(sb.ToString(), 2);
		index += 3;

		if (type == 4)
		{
			var notLast = true;
			sb.Clear();
			while (notLast)
			{
				notLast = bin[index] == '1';
				index += 1;
				for (int i = index; i < index + 4; i++)
					sb.Append(bin[i]);
				index += 4;
			}

			return Convert.ToInt64(sb.ToString(), 2);
		}

		var lengthType = bin[index];
		index++;

		var elems = new List<long>();
		if (lengthType == '1')
		{
			sb.Clear();
			for (int i = index; i < index + 11; i++)
				sb.Append(bin[i]);
			index += 11;
			var len = Convert.ToInt32(sb.ToString(), 2);
			for (var i = 0; i < len; i++)
				elems.Add(EvalV2(bin, ref index));
		}
		else
		{
			sb.Clear();
			for (int i = index; i < index + 15; i++)
				sb.Append(bin[i]);
			index += 15;
			var len = Convert.ToInt32(sb.ToString(), 2);
			var curIndex = index;
			while (index - curIndex < len)
				elems.Add(EvalV2(bin, ref index));
		}

		return type switch
		{
			0 => elems.Sum(),
			1 => elems.Aggregate<long, long>(1, (x, acc) => acc * x),
			2 => elems.Min(),
			3 => elems.Max(),
			5 => elems.First() > elems.Last() ? 1 : 0,
			6 => elems.First() < elems.Last() ? 1 : 0,
			7 => elems.First() == elems.Last() ? 1 : 0,
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}