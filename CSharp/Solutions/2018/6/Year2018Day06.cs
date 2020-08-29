using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._6
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day06 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var coords = input.Select(x =>
			{
				var splitted = x.Split(", ");
				return (x: int.Parse(splitted[0]), y: int.Parse(splitted[1]));
			}).ToList();

			var xMax = coords.Select(coordinate => coordinate.x).Max();
			var xMin = coords.Select(coordinate => coordinate.x).Min();
			var yMax = coords.Select(coordinate => coordinate.y).Max();
			var yMin = coords.Select(coordinate => coordinate.y).Min();

			var cnt = new List<int>(new int[coords.Count]);
			for (var x = xMin; x <= xMax; x++)
			{
				for (var y = yMin; y <= yMax; y++)
				{
					var distances = coords.Select(coordinate => ManhattanDistance(x, coordinate.x, y, coordinate.y))
						.ToList();

					var minCount = distances.Count(dist => dist == distances.Min());
					if (minCount > 1)
						continue;

					cnt[distances.FindIndex(x => x == distances.Min())]
						+= !(x == xMax || x == xMin || y == yMax || y == yMin) ? 1 : 1000000;
				}
			}

			return cnt.Where(x => x < 1000000).Max().ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var coords = input.Select(x =>
			{
				var splitted = x.Split(", ");
				return (x: int.Parse(splitted[0]), y: int.Parse(splitted[1]));
			}).ToList();

			var xMax = coords.Select(coordinate => coordinate.x).Max();
			var xMin = coords.Select(coordinate => coordinate.x).Min();
			var yMax = coords.Select(coordinate => coordinate.y).Max();
			var yMin = coords.Select(coordinate => coordinate.y).Min();

			var nin = 0;
			for (var x = xMin; x <= xMax; x++)
			{
				for (var y = yMin; y <= yMax; y++)
				{
					var distances = coords.Select(coordinate => ManhattanDistance(x, coordinate.x, y, coordinate.y));

					if (distances.Sum() < 10000)
						nin++;
				}
			}

			return nin.ToString();
		}

		private static int ManhattanDistance(int x1, int x2, int y1, int y2)
		{
			return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
		}
	}
}