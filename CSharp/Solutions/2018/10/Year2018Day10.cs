using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._10
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day10 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			const string regex = @"position=<\s*(-?\d+),\s*(-?\d+)> velocity=<\s*(-?\d+),\s*(-?\d+)>";
			var points = input.Select(x =>
			{
				var groups = Regex.Match(x, regex).Groups;
				return new Point
				{
					PositionX = int.Parse(groups[1].Value),
					PositionY = int.Parse(groups[2].Value),
					VelocityX = int.Parse(groups[3].Value),
					VelocityY = int.Parse(groups[4].Value)
				};
			}).ToList();

			const int letterHeight = 10;
			while (true)
			{
				foreach (var point in points)
				{
					point.PositionX += point.VelocityX;
					point.PositionY += point.VelocityY;
				}

				var minX = points.Min(x => x.PositionX);
				var minY = points.Min(x => x.PositionY);
				var maxX = points.Max(x => x.PositionX);
				var maxY = points.Max(x => x.PositionY);

				if (!(maxY - minY < letterHeight))
					continue;

				var set = new HashSet<(int x, int y)>();
				foreach (var point in points)
					set.Add((point.PositionX, point.PositionY));

				var ln = new List<List<char>>();
				for (var i = 0; i <= maxY - minY; i++)
				{
					var lst = new List<char>();
					for (var j = 0; j <= maxX - minX; j++)
						lst.Add('.');

					ln.Add(lst);
				}

				for (var x = minX; x <= maxX; x++)
				{
					for (var y = minY; y <= maxY; y++)
						ln[y - minY][x - minX] = set.Contains((x, y)) ? '#' : '.';
				}

				return Environment.NewLine +
				       string.Join(Environment.NewLine, ln.Select(c => string.Join(string.Empty, c)));
			}
		}

		public string Part2(IEnumerable<string> input)
		{
			const string regex = @"position=<\s*(-?\d+),\s*(-?\d+)> velocity=<\s*(-?\d+),\s*(-?\d+)>";
			var points = input.Select(x =>
			{
				var groups = Regex.Match(x, regex).Groups;
				return new Point
				{
					PositionX = int.Parse(groups[1].Value),
					PositionY = int.Parse(groups[2].Value),
					VelocityX = int.Parse(groups[3].Value),
					VelocityY = int.Parse(groups[4].Value)
				};
			}).ToList();

			const int letterHeight = 10;
			var count = 0;
			while (true)
			{
				count++;
				foreach (var point in points)
				{
					point.PositionX += point.VelocityX;
					point.PositionY += point.VelocityY;
				}

				var minY = points.Min(x => x.PositionY);
				var maxY = points.Max(x => x.PositionY);

				if (!(maxY - minY < letterHeight))
					continue;

				return count.ToString();
			}
		}

		private class Point
		{
			public int PositionX { get; set; }
			public int PositionY { get; set; }
			public int VelocityX { get; set; }
			public int VelocityY { get; set; }
		}
	}
}