using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._23
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day23 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var list = new List<(int x, int y, int z, int r)>();
			const string? regex = @"pos=<(?<x>-?\d+),(?<y>-?\d+),(?<z>-?\d+)>, r=(?<r>\d+)";
			foreach (var line in input)
			{
				var groups = Regex.Match(line, regex).Groups;

				list.Add((int.Parse(groups["x"].Value), int.Parse(groups["y"].Value),
					int.Parse(groups["z"].Value), int.Parse(groups["r"].Value)));
			}

			var (x, y, z, r) = list.OrderByDescending(selector => selector.r).First();

			return list.Select(val => Math.Abs(x - val.x) + Math.Abs(y - val.y) + Math.Abs(z - val.z))
				.Select(dst => dst <= r ? 1 : 0).Sum().ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var list = new List<(int x, int y, int z, int r)>();
			const string? regex = @"pos=<(?<x>-?\d+),(?<y>-?\d+),(?<z>-?\d+)>, r=(?<r>\d+)";
			foreach (var line in input)
			{
				var groups = Regex.Match(line, regex).Groups;

				list.Add((int.Parse(groups["x"].Value), int.Parse(groups["y"].Value),
					int.Parse(groups["z"].Value), int.Parse(groups["r"].Value)));
			}

			static bool InRange((int x, int y, int z, int r) a, (int x, int y, int z) b)
			{
				return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z) <= a.r;
			}

			(int x, int y, int z) bestLocation = (0, 0, 0);
			int inRange, maxInRange, bestSum;
			(int minX, int minY, int minZ, int maxX, int maxY, int maxZ) limits;
			limits = (list.Min(bot => bot.x), list.Min(bot => bot.y), list.Min(bot => bot.z), list.Max(bot => bot.y),
				list.Max(bot => bot.y), list.Max(bot => bot.z));
			int xRange = limits.maxX - limits.minX,
				yRange = limits.maxY - limits.minY,
				zRange = limits.maxZ - limits.minZ;
			var grain = (int) Math.Pow(2, 26);
			do
			{
				maxInRange = 0;
				bestSum = int.MaxValue;
				for (var x = limits.minX; x < limits.maxX; x += grain)
				for (var y = limits.minY; y < limits.maxY; y += grain)
				for (var z = limits.minZ; z < limits.maxZ; z += grain)
					if ((inRange = list.Count(bot => InRange(bot, (x, y, z)))) > maxInRange ||
					    inRange == maxInRange && Math.Abs(x) + Math.Abs(y) + Math.Abs(z) < bestSum)
					{
						maxInRange = inRange;
						bestLocation = (x, y, z);
						bestSum = Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
					}

				grain /= 2;
				xRange /= 2;
				yRange /= 2;
				zRange /= 2;
				limits = (bestLocation.x - xRange / 2, bestLocation.y - yRange / 2, bestLocation.z - zRange / 2,
					bestLocation.x + xRange / 2, bestLocation.y + yRange / 2, bestLocation.z + zRange / 2);
			} while (grain >= 1);

			return bestSum.ToString();
		}
	}
}