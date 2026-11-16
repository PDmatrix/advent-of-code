using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._15;

[UsedImplicitly]
public class Year2022Day15 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var sensorBeaconPairs = ParseInput(input);

		const int targetRow = 2000000;
		var excludedPositions = new HashSet<int>();
		var beaconPositions = new HashSet<Point>();

		foreach (var (sensor, beacon) in sensorBeaconPairs)
		{
			beaconPositions.Add(beacon);
			var distance = ManhattanDistance(sensor, beacon);

			var dy = Math.Abs(sensor.Y - targetRow);
			if (dy > distance)
				continue;
			
			var dx = distance - dy;
			for (var x = sensor.X - dx; x <= sensor.X + dx; x++)
				excludedPositions.Add(x);
		}
		
		foreach (var beacon in beaconPositions.Where(beacon => beacon.Y == targetRow))
			excludedPositions.Remove(beacon.X);

		return excludedPositions.Count;
	}

	private static List<(Point, Point)> ParseInput(IEnumerable<string> input)
	{
		var list = new List<(Point, Point)>();
		foreach (var line in input)
		{
			var splitted = line.Split(" ");
			var x = int.Parse(splitted[2].Replace("x=", string.Empty).Replace(",", string.Empty));
			var y = int.Parse(splitted[3].Replace("y=", string.Empty).Replace(":", string.Empty));
			var x2 = int.Parse(splitted[8].Replace("x=", string.Empty).Replace(",", string.Empty));
			var y2 = int.Parse(splitted[9].Replace("y=", string.Empty));
			list.Add((new Point(x, y), new Point(x2, y2)));
		}

		return list;
	}

	private static int ManhattanDistance(Point p1, Point p2)
	{
		return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
	}
	
	public object Part2(IEnumerable<string> input)
	{
		var sensorBeaconPairs = ParseInput(input);
		const int maxCoordinate = 4000000;
		var boundaries = new List<(int x, int y)>();

		foreach (var (sensor, beacon) in sensorBeaconPairs)
		{
			var distance = ManhattanDistance(sensor, beacon);

			for (var dx = -distance - 1; dx <= distance + 1; dx++)
			{
				var dy = (distance + 1) - Math.Abs(dx);
				if (sensor.X + dx < 0 || sensor.X + dx > maxCoordinate)
					continue;
				
				if (sensor.Y + dy >= 0 && sensor.Y + dy <= maxCoordinate)
					boundaries.Add((sensor.X + dx, sensor.Y + dy));
				if (sensor.Y - dy >= 0 && sensor.Y - dy <= maxCoordinate)
					boundaries.Add((sensor.X + dx, sensor.Y - dy));
			}
		}

		foreach (var point in boundaries)
		{
			var covered = false;
			foreach (var (sensor, beacon) in sensorBeaconPairs)
			{
				var distance = ManhattanDistance(sensor, beacon);
				var distanceToPoint = ManhattanDistance(sensor, new Point(point.x, point.y));

				if (distanceToPoint > distance)
					continue;
				
				covered = true;
				break;
			}

			if (!covered)
				return (long)point.x * 4000000 + point.y;
		}

		throw new Exception("No solution found");
	}
}
