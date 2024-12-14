using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._14;

[UsedImplicitly]
public class Year2024Day14 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var robots = ParseInput(input);

		const int width = 101;
		const int height = 103;
		const int seconds = 100;

		for (var i = 0; i < seconds; i++)
		{
			MoveRobots(robots, width, height);
		}

		var quadrants = new Dictionary<int, int>();
		foreach (var robot in robots)
		{
			var quadrant = robot.Position.X switch
			{
				< (width - 1) / 2 when robot.Position.Y < (height - 1) / 2 => 1,
				>= width - (width - 1) / 2 when robot.Position.Y < (height - 1) / 2 => 2,
				< (width - 1) / 2 when robot.Position.Y >= height - (height - 1) / 2 => 3,
				>= width - (width - 1) / 2 when robot.Position.Y >= height - (height - 1) / 2 => 4,
				_ => 0
			};

			if (quadrant == 0)
				continue;

			if (!quadrants.ContainsKey(quadrant))
				quadrants[quadrant] = 0;

			quadrants[quadrant]++;
		}

		return quadrants.Aggregate(1, (current, quadrant) => current * quadrant.Value);
	}

	public object Part2(IEnumerable<string> input)
	{
		var robots = ParseInput(input);

		const int width = 101;
		const int height = 103;
		const int seconds = 10000;

		var answer = 0;
		for (var i = 1; i <= seconds; i++)
		{
			MoveRobots(robots, width, height);

			var grid = new Dictionary<Point, int>();
			foreach (var (pos, _) in robots)
			{
				grid.TryAdd(pos, 0);
				grid[pos]++;
			}

			// check for the top of the tree
			/*
			 * .....1.....
			   ....111....
			   ...11111...
			   ..1111111..
			   .111111111.
			 */
			var foundTree = false;
			foreach (var (k, v) in grid)
			{
				var foundY = true;
				for (var j = 1; j <= 4; j++)
				{
					var foundX = true;
					for (var l = -j; l <= j; l++)
					{
						if (grid.ContainsKey(new Point(k.X + l, k.Y - j)))
							continue;

						foundX = false;
						break;
					}

					if (foundX)
						continue;

					foundY = false;
					break;
				}

				if (!foundY)
					continue;

				foundTree = true;
				break;
			}

			if (!foundTree)
				continue;

			answer = i;
			break;
		}


		return answer;
	}

	private static void MoveRobots(List<(Point Position, Point Velocity)> robots, int width, int height)
	{
		for (var j = 0; j < robots.Count; j++)
		{
			var robot = robots[j];
			var newX = (robot.Position.X + robot.Velocity.X) % width;
			if (newX < 0)
				newX += width;

			var newY = (robot.Position.Y + robot.Velocity.Y) % height;
			if (newY < 0)
				newY += height;

			robot.Position = new Point(newX, newY);
			robots[j] = robot;
		}
	}

	private static List<(Point Position, Point Velocity)> ParseInput(IEnumerable<string> input)
	{
		var robots = new List<(Point Position, Point Velocity)>();
		foreach (var line in input)
		{
			var split = line.Split(" ");
			var position = new Point(int.Parse(split[0].Split(",")[0].Substring(2)), int.Parse(split[0].Split(",")[1]));
			var velocity = new Point(int.Parse(split[1].Split(",")[0].Substring(2)), int.Parse(split[1].Split(",")[1]));
			robots.Add((position, velocity));
		}

		return robots;
	}
}