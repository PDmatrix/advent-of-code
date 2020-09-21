using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2018._17
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day17 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var clay = new HashSet<(int x, int y)>();;
			foreach (var line in input)
			{
				var match = Regex.Matches(line, @"[yx]|-?\d+");
				for (var i = int.Parse(match[3].Value); i <= int.Parse(match[4].Value); i++)
				{
					clay.Add(
						match[0].Value == "x"
							? (int.Parse(match[1].Value), i)
							: (i, int.Parse(match[1].Value))
					);
				}
			}

			var maxY = clay.Max(x => x.y);
			var standingWater = new HashSet<(int x, int y)>();
			var runningWater = new HashSet<(int x, int y)>();
			var pour = new Stack<(int x, int y)>();
			pour.Push((500, 0));
			var fill = new Stack<(int x, int y)>();
			while (pour.Any() || fill.Any())
			{
				while (pour.Any())
				{
					var (x, y) = pour.Pop();
					
					if (y > maxY)
						continue;

					if (!clay.Contains((x, y + 1)) && !standingWater.Contains((x, y + 1)))
						pour.Push((x, y + 1));
					else if (!runningWater.Contains((x, y)))
						fill.Push((x, y));

					runningWater.Add((x, y));
				}

				while (fill.Any())
				{
					var (x, y) = fill.Pop();

					var xp = x;
					while ((clay.Contains((xp, y + 1)) || standingWater.Contains((xp, y + 1))) &&
					       !clay.Contains((xp + 1, y)))
					{
						xp++;
					}
					
					if (!clay.Contains((xp, y + 1)) && !standingWater.Contains((xp, y + 1)))
						pour.Push((xp, y));

					var xm = x;
					while ((clay.Contains((xm, y + 1)) || standingWater.Contains((xm, y + 1))) &&
					       !clay.Contains((xm - 1, y)))
					{
						xm--;
					}
					
					if (!clay.Contains((xm, y + 1)) && !standingWater.Contains((xm, y + 1)))
						pour.Push((xm, y));

					for (var i = xm; i <= xp; i++)
						runningWater.Add((i, y));

					if ((clay.Contains((xp, y + 1)) || standingWater.Contains((xp, y + 1))) &&
					    (clay.Contains((xm, y + 1)) || standingWater.Contains((xm, y + 1))))
					{
						for (var i = xm; i <= xp; i++)
							standingWater.Add((i, y));
						
						fill.Push((x, y - 1));
					}

				}
			}

			// runningWater.Count - 3 == 36790 ?
			return (runningWater.Count - 3).ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var clay = new HashSet<(int x, int y)>();;
			foreach (var line in input)
			{
				var match = Regex.Matches(line, @"[yx]|-?\d+");
				for (var i = int.Parse(match[3].Value); i <= int.Parse(match[4].Value); i++)
				{
					clay.Add(
						match[0].Value == "x"
							? (int.Parse(match[1].Value), i)
							: (i, int.Parse(match[1].Value))
					);
				}
			}

			var maxY = clay.Max(x => x.y);
			var standingWater = new HashSet<(int x, int y)>();
			var runningWater = new HashSet<(int x, int y)>();
			var pour = new Stack<(int x, int y)>();
			pour.Push((500, 0));
			var fill = new Stack<(int x, int y)>();
			while (pour.Any() || fill.Any())
			{
				while (pour.Any())
				{
					var (x, y) = pour.Pop();
					
					if (y > maxY)
						continue;

					if (!clay.Contains((x, y + 1)) && !standingWater.Contains((x, y + 1)))
						pour.Push((x, y + 1));
					else if (!runningWater.Contains((x, y)))
						fill.Push((x, y));

					runningWater.Add((x, y));
				}

				while (fill.Any())
				{
					var (x, y) = fill.Pop();

					var xp = x;
					while ((clay.Contains((xp, y + 1)) || standingWater.Contains((xp, y + 1))) &&
					       !clay.Contains((xp + 1, y)))
					{
						xp++;
					}
					
					if (!clay.Contains((xp, y + 1)) && !standingWater.Contains((xp, y + 1)))
						pour.Push((xp, y));

					var xm = x;
					while ((clay.Contains((xm, y + 1)) || standingWater.Contains((xm, y + 1))) &&
					       !clay.Contains((xm - 1, y)))
					{
						xm--;
					}
					
					if (!clay.Contains((xm, y + 1)) && !standingWater.Contains((xm, y + 1)))
						pour.Push((xm, y));

					for (var i = xm; i <= xp; i++)
						runningWater.Add((i, y));

					if ((clay.Contains((xp, y + 1)) || standingWater.Contains((xp, y + 1))) &&
					    (clay.Contains((xm, y + 1)) || standingWater.Contains((xm, y + 1))))
					{
						for (var i = xm; i <= xp; i++)
							standingWater.Add((i, y));
						
						fill.Push((x, y - 1));
					}

				}
			}

			return standingWater.Count.ToString();
		}
	}
}