using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2018._22
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day22 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var depth = int.Parse(Regex.Match(input.First(), @"depth: (\d+)").Groups[1].Value);
			var target = Regex.Match(input.Last(), @"target: (\d+),(\d+)").Groups;
			var (targetX, targetY) = (int.Parse(target[1].Value), int.Parse(target[2].Value));
			
			var erosionLevels = new Dictionary<(int x, int y), int>();

			int CalculateErosionLevel((int x, int y) pos)
			{
				var (x, y) = pos;
				if (erosionLevels.ContainsKey(pos))
					return erosionLevels[pos];

				if (pos == (0, 0))
					erosionLevels[pos] = depth % 20183;
				else if (x == targetX && y == targetY)
					erosionLevels[pos] = depth % 20183;
				else if (y == 0)
					erosionLevels[pos] = (x * 16807 + depth) % 20183;
				else if (x == 0)
					erosionLevels[pos] = (y * 48271 + depth) % 20183;
				else
					erosionLevels[pos] =
						(CalculateErosionLevel((x - 1, y)) * CalculateErosionLevel((x, y - 1)) + depth) % 20183;

				return erosionLevels[pos];
			}

			int RegionType((int x, int y) pos)
				=> CalculateErosionLevel(pos) % 3;

			var s = 0;
			for (var y = 0; y <= targetY; y++)
			{
				for (var x = 0; x <= targetX; x++)
				{
					s += RegionType((x, y));
				}
			}

			return s.ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var depth = int.Parse(Regex.Match(input.First(), @"depth: (\d+)").Groups[1].Value);
			var target = Regex.Match(input.Last(), @"target: (\d+),(\d+)").Groups;
			var (targetX, targetY) = (int.Parse(target[1].Value), int.Parse(target[2].Value));
			var erosionLevels = new Dictionary<(int x, int y), int>();

			int CalculateErosionLevel((int x, int y) pos)
			{
				var (x, y) = pos;
				if (erosionLevels.ContainsKey(pos))
					return erosionLevels[pos];

				if (pos == (0, 0))
					erosionLevels[pos] = depth % 20183;
				else if (x == targetX && y == targetY)
					erosionLevels[pos] = depth % 20183;
				else if (y == 0)
					erosionLevels[pos] = (x * 16807 + depth) % 20183;
				else if (x == 0)
					erosionLevels[pos] = (y * 48271 + depth) % 20183;
				else
					erosionLevels[pos] =
						(CalculateErosionLevel((x - 1, y)) * CalculateErosionLevel((x, y - 1)) + depth) % 20183;

				return erosionLevels[pos];
			}

			int RegionType((int x, int y) pos)
				=> CalculateErosionLevel(pos) % 3;

			var tools = new Dictionary<string, List<int>>
			{
				["neither"] = new List<int> {1, 2},
				["climbing_gear"] = new List<int> {0, 1},
				["torch"] = new List<int> {0, 2}
			};
			var toolsSwitch = new Dictionary<(int type, string tool), string>()
			{
				[(0, "climbing_gear")] = "torch",
				[(0, "torch")] = "climbing_gear",
				[(1, "climbing_gear")] = "neither",
				[(1, "neither")] = "climbing_gear",
				[(2, "torch")] = "neither",
				[(2, "neither")] = "torch"
			};

			var queue = new List<(int x, int y, string tool, int time, int cost)>
			{
				(0, 0, "torch", 0, targetX + targetY)
			};
			
			var visited = new HashSet<(int x, int y, string tool)>();
			while (queue.Any())
			{
				var (x, y, tool, time, _) = queue.First();
				queue.RemoveAt(0);

				if (visited.Contains((x, y, tool))) 
					continue;
				
				visited.Add((x, y, tool));

				if (x == targetX && y == targetY && tool == "torch")
					return time.ToString();

				var directions = new List<(int x, int y)>
				{
					(x + 1, y),
					(x - 1, y),
					(x, y + 1),
					(x, y - 1)
				}.Where(c => c.x >= 0 && c.y >= 0);

				var directionsDirect = directions
					.Where(c => tools[tool].Contains(RegionType((c.x, c.y))))
					.Select(c => (c, tool, time + 1))
					.ToList();

				var newTool = toolsSwitch[(RegionType((x, y)), tool)];
				var directionsSwitch = new List<((int x, int y), string tool, int time)>
				{
					((x, y), newTool, time + 7)
				};

				foreach (var (newPosition, newToolArr, newTime) in directionsDirect.Concat(directionsSwitch))
				{
					if (visited.Contains((newPosition.x, newPosition.y, newToolArr))) 
						continue;
					
					var newCost = Math.Abs(newPosition.x - targetX) + Math.Abs(newPosition.y - targetY);

					var i = 0;
					while (i < queue.Count && queue[i].cost < newTime + newCost)
						i++;
							
					queue.Insert(i, (newPosition.x, newPosition.y, newToolArr, newTime, newTime + newCost));
				}
			}

			throw new Exception("No solution");
		}
	}
}