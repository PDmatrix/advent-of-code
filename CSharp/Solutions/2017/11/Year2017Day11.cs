using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2017._11
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day11 : ISolution
	{
		private readonly IDictionary<string, (int x, int y, int z)> _map
			= new Dictionary<string, (int x, int y, int z)>
			{
				{"n", (0, 1, -1)},
				{"ne", (1, 0, -1)},
				{"se", (1, -1, 0)},
				{"s", (0, -1, 1)},
				{"sw", (-1, 0, 1)},
				{"nw", (-1, 1, 0)},
			};
		public object Part1(IEnumerable<string> input)
		{
			var paths = input.First().Split(',');
			var point = (x: 0, y: 0, z: 0);
			foreach (var path in paths)
			{
				var (x, y, z) = _map[path];
				point.x += x;
				point.y += y;
				point.z += z;
			}

			return GetDistance(point).ToString();
		}

		public object Part2(IEnumerable<string> input)
		{
			var paths = input.First().Split(',');
			var point = (x: 0, y: 0, z: 0);
			var max = int.MinValue;
			foreach (var path in paths)
			{
				var (x, y, z) = _map[path];
				point.x += x;
				point.y += y;
				point.z += z;
				max = Math.Max(GetDistance(point), max);
			}

			return max.ToString();
		}

		private static int GetDistance((int x, int y, int z) point) =>
			Math.Max(Math.Abs(point.x), Math.Max(Math.Abs(point.y), Math.Abs(point.z)));
	}
}