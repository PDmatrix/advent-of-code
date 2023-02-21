using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2021._18;

[UsedImplicitly]
public class Year2021Day18 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var lists = ParseInput(input);

		var final = lists.First();
		for (var i = 1; i < lists.Count; i++)
		{
			final = Sum(final, lists[i]);
		}

		return Magnitude(final);
	}

	public object Part2(IEnumerable<string> input)
	{
		var lists = ParseInput(input);

		var max = int.MinValue;
		for (var i = 0; i < lists.Count; i++)
		{
			for (var j = 0; j < lists.Count; j++)
			{
				if (i == j)
					continue;
				var sum = Sum(lists[i], lists[j]);
				max = Math.Max(max, Magnitude(sum));
			}
		}

		return max;
	}

	private static List<List<(int a, int depth)>> ParseInput(IEnumerable<string> input)
	{
		var lists = new List<List<(int a, int depth)>>();
		foreach (var line in input)
		{
			var list = new List<(int a, int depth)>();
			var depth = -1;
			foreach (var c in line)
			{
				switch (c)
				{
					case '[':
						depth++;
						continue;
					case ']':
						depth--;
						continue;
					case ',':
						continue;
					default:
						list.Add((int.Parse(c.ToString()), depth));
						break;
				}
			}

			lists.Add(list);
		}

		return lists;
	}

	private static (List<(int a, int depth)>, bool) MagnitudeRed(List<(int a, int depth)> list, int level)
	{
		for (var i = 0; i < list.Count; i++)
		{
			if (list[i].depth != level) continue;

			list[i] = (list[i].a * 3 + 2 * list[i + 1].a, list[i].depth - 1);
			list[i + 1] = (-1, -1);

			return (list.Where(x => x != (-1, -1)).ToList(), true);
		}

		return (list, false);
	}

	private static int Magnitude(List<(int a, int depth)> list)
	{
		var c = true;
		while (c)
			(list, c) = MagnitudeRed(list, 3);
		c = true;
		while (c)
			(list, c) = MagnitudeRed(list, 2);
		c = true;
		while (c)
			(list, c) = MagnitudeRed(list, 1);

		return list[0].a * 3 + 2 * list[1].a;
	}

	private static List<(int a, int depth)> Sum(List<(int a, int depth)> a, List<(int a, int depth)> b)
	{
		var temp = new List<(int a, int depth)>();
		foreach (var val in a)
			temp.Add((val.a, val.depth + 1));
		foreach (var val in b)
			temp.Add((val.a, val.depth + 1));

		while (true)
		{
			var (tmp, x) = Explode(temp);
			temp = tmp;
			if (x)
				continue;

			var (tmp2, x2) = Split(temp);
			temp = tmp2;
			if (x2)
				continue;

			break;
		}

		return temp;
	}


	private static (List<(int a, int depth)>, bool) Split(List<(int a, int depth)> list)
	{
		var newList = new List<(int a, int depth)>();
		for (var i = 0; i < list.Count; i++)
		{
			var el = list[i];
			if (el.a < 10) continue;

			for (var j = 0; j < i; j++)
				newList.Add(list[j]);

			newList.Add(((int)Math.Floor((double)el.a / 2), el.depth + 1));
			newList.Add(((int)Math.Ceiling((double)el.a / 2), el.depth + 1));
			for (var j = i + 1; j < list.Count; j++)
				newList.Add(list[j]);

			return (newList, true);
		}

		return (list, false);
	}

	private static (List<(int a, int depth)>, bool) Explode(List<(int a, int depth)> list)
	{
		for (var i = 0; i < list.Count; i++)
		{
			var el = list[i];
			if (el.depth != 4)
				continue;

			if (i > 0)
				list[i - 1] = (list[i - 1].a + el.a, list[i - 1].depth);

			if (i + 2 < list.Count)
				list[i + 2] = (list[i + 2].a + list[i + 1].a, list[i + 2].depth);

			list[i] = (0, 3);

			if (i + 1 < list.Count)
				list[i + 1] = (-1, -1);

			return (list.Where(x => x != (-1, -1)).ToList(), true);
		}

		return (list, false);
	}
}