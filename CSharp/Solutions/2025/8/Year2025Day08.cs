using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._8;

[UsedImplicitly]
public class Year2025Day08 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		// input = new[]
		// {
		// 	"162,817,812",
		// 	"57,618,57",
		// 	"906,360,560",
		// 	"592,479,940",
		// 	"352,342,300",
		// 	"466,668,158",
		// 	"542,29,236",
		// 	"431,825,988",
		// 	"739,650,466",
		// 	"52,470,668",
		// 	"216,146,977",
		// 	"819,987,18",
		// 	"117,168,530",
		// 	"805,96,715",
		// 	"346,949,466",
		// 	"970,615,88",
		// 	"941,993,340",
		// 	"862,61,35",
		// 	"984,92,344",
		// 	"425,690,689",
		// };

		var points = ParseInput(input);
		var distances = new Dictionary<double, (Point3D A, Point3D B)>();
		for (var i = 0; i < points.Count; i++)
		{
			for (var j = i + 1; j < points.Count; j++)
			{
				distances[GetDistance(points[i], points[j])] = (points[i], points[j]);
			}
		}

		var circuits = new List<HashSet<Point3D>>();
		var orderedDistances = distances.Keys.OrderBy(x => x).ToList();
		var count = 0;
		while (count < 1000)
		{
			var d = orderedDistances[count];
			count++;

			var (a, b) = distances[d];
			var circuitA = circuits.FirstOrDefault(x => x.Contains(a));
			var circuitB = circuits.FirstOrDefault(x => x.Contains(b));
			if (circuitA == null && circuitB == null)
			{
				circuits.Add(new HashSet<Point3D> { a, b });
				continue;
			}

			if (circuitA != null && circuitB != null && circuitA != circuitB)
			{
				circuitA.UnionWith(circuitB);
				circuits.Remove(circuitB);
				continue;
			}

			if (circuitA != null && circuitB != null)
				continue;

			if (circuitA == null)
			{
				circuitB.Add(a);
				continue;
			}

			if (circuitB == null)
				circuitA.Add(b);
		}

		return circuits.OrderByDescending(x => x.Count).Take(3).Aggregate(1, (acc, set) => acc * set.Count);
	}

	public object Part2(IEnumerable<string> input)
	{
		var points = ParseInput(input);
		var distances = new Dictionary<double, (Point3D A, Point3D B)>();
		for (var i = 0; i < points.Count; i++)
		{
			for (var j = i + 1; j < points.Count; j++)
			{
				distances[GetDistance(points[i], points[j])] = (points[i], points[j]);
			}
		}

		var circuits = new List<HashSet<Point3D>>();
		var orderedDistances = distances.Keys.OrderBy(x => x).ToList();
		var count = 0;
		long answer = 0;
		while (true)
		{
			var d = orderedDistances[count];
			count++;

			var (a, b) = distances[d];
			var circuitA = circuits.FirstOrDefault(x => x.Contains(a));
			var circuitB = circuits.FirstOrDefault(x => x.Contains(b));
			if (circuitA == null && circuitB == null)
			{
				circuits.Add(new HashSet<Point3D> { a, b });
				continue;
			}

			if (circuitA != null && circuitB != null && circuitA != circuitB)
			{
				circuitA.UnionWith(circuitB);
				circuits.Remove(circuitB);
			} else if (circuitA != null && circuitB != null)
			{
				continue;
			} else if (circuitA == null)
			{
				circuitB.Add(a);
			} else if (circuitB == null)
			{
				circuitA.Add(b);
			}
			if (circuits.First().Count == points.Count)
			{
				answer = a.X * b.X;
				break;
			}
		}

		return answer;
	}

	private static double GetDistance(Point3D a, Point3D b)
	{
		return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
	}

	private static List<Point3D> ParseInput(IEnumerable<string> input)
	{
		var points = new List<Point3D>();
		var regex = new Regex(@"(\d+),(\d+),(\d+)");
		foreach (var line in input)
		{
			var match = regex.Match(line);
			if (!match.Success)
				continue;

			var x = int.Parse(match.Groups[1].Value);
			var y = int.Parse(match.Groups[2].Value);
			var z = int.Parse(match.Groups[3].Value);
			points.Add(new Point3D(x, y, z));
		}

		return points;
	}

	private record struct Point3D(long X, long Y, long Z);
}