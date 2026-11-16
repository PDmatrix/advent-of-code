using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._3;

[UsedImplicitly]
public class Year2019Day03 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = new bool[50000, 50000];
		var curX = 25000;
		var curY = 25000;
		var reg = new Regex(@"(?<dir>\w)(?<num>\d+)", RegexOptions.Compiled);
		var path = new List<(string dir, int num)>();
		var enumerable = input as string[] ?? input.ToArray();
		foreach (var elem in enumerable.First().Split(',').ToArray())
		{
			var match = reg.Match(elem);
			var direction = match.Groups["dir"].Value;
			var number = int.Parse(match.Groups["num"].Value);
			path.Add((direction, number));
		}

		foreach (var (dir, num) in path)
		{
			for (var i = 0; i < num; i++)
			{
				switch (dir)
				{
					case "U":
						curY++;
						break;
					case "D":
						curY--;
						break;
					case "L":
						curX--;
						break;
					case "R":
						curX++;
						break;
				}

				grid[curX, curY] = true;
			}
		}

		path = new List<(string dir, int num)>();
		curX = 25000;
		curY = 25000;
		var minDistance = int.MaxValue;
		foreach (var elem in enumerable.Last().Split(',').ToArray())
		{
			var match = reg.Match(elem);
			var direction = match.Groups["dir"].Value;
			var number = int.Parse(match.Groups["num"].Value);
			path.Add((direction, number));
		}

		foreach (var (dir, num) in path)
		{
			for (var i = 0; i < num; i++)
			{
				switch (dir)
				{
					case "U":
						curY++;
						break;
					case "D":
						curY--;
						break;
					case "L":
						curX--;
						break;
					case "R":
						curX++;
						break;
				}

				if (!grid[curX, curY])
					continue;

				var distance = CalculateManhattanDistance(25000, curX, 25000, curY);
				if (distance < minDistance)
					minDistance = distance;
			}
		}


		return minDistance;
	}

	private static int CalculateManhattanDistance(int x1, int x2, int y1, int y2)
	{
		return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = new Dictionary<(int x, int y), int>();
		var curX = 25000;
		var curY = 25000;
		var reg = new Regex(@"(?<dir>\w)(?<num>\d+)", RegexOptions.Compiled);
		var path = new List<(string dir, int num)>();
		var enumerable = input as string[] ?? input.ToArray();
		foreach (var elem in enumerable.First().Split(',').ToArray())
		{
			var match = reg.Match(elem);
			var direction = match.Groups["dir"].Value;
			var number = int.Parse(match.Groups["num"].Value);
			path.Add((direction, number));
		}

		var count = 0;
		foreach (var (dir, num) in path)
		{
			for (var i = 0; i < num; i++)
			{
				count++;
				switch (dir)
				{
					case "U":
						curY++;
						break;
					case "D":
						curY--;
						break;
					case "L":
						curX--;
						break;
					case "R":
						curX++;
						break;
				}

				if (!grid.ContainsKey((curX, curY)))
					grid.Add((curX, curY), count);
			}
		}

		path = new List<(string dir, int num)>();
		curX = 25000;
		curY = 25000;
		var minDistance = int.MaxValue;
		foreach (var elem in enumerable.Last().Split(',').ToArray())
		{
			var match = reg.Match(elem);
			var direction = match.Groups["dir"].Value;
			var number = int.Parse(match.Groups["num"].Value);
			path.Add((direction, number));
		}

		count = 0;
		foreach (var (dir, num) in path)
		{
			for (var i = 0; i < num; i++)
			{
				count++;
				switch (dir)
				{
					case "U":
						curY++;
						break;
					case "D":
						curY--;
						break;
					case "L":
						curX--;
						break;
					case "R":
						curX++;
						break;
				}

				if (!grid.ContainsKey((curX, curY)))
					continue;

				var distance = grid[(curX, curY)] + count;
				if (distance < minDistance)
					minDistance = distance;
			}
		}


		return minDistance;
	}
}