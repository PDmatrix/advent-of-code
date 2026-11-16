using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._23;

[UsedImplicitly]
public partial class Year2022Day23 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		const int rounds = 10;

		var directions = new LinkedList<Point>();
		directions.AddLast(new Point(0, -1)); // N
		directions.AddLast(new Point(0, 1)); // S
		directions.AddLast(new Point(-1, 0)); // W
		directions.AddLast(new Point(1, 0)); // E

		for (var i = 0; i < rounds; i++)
		{
			var proposes = new Dictionary<Point, List<Point>>();
			var elves = grid.Where(x => x.Value == "#").Select(x => x.Key).ToList();
			
			foreach (var elf in elves)
			{
				var currentDirection = directions.First;
				var adjacent = GetAdjacent(elf, new Point(0, 0));
				if (adjacent.TrueForAll(x => grid.GetValueOrDefault(x, ".") == "."))
					continue;

				while (currentDirection != null)
				{
					adjacent = GetAdjacent(elf, currentDirection.Value);
					
					if (adjacent.TrueForAll(x => grid.GetValueOrDefault(x, ".") == "."))
						break;
					
					currentDirection = currentDirection.Next;
				}
				
				if (currentDirection == null)
					continue;
				
				var newPoint = new Point(elf.X + currentDirection.Value.X, elf.Y + currentDirection.Value.Y);

				if (!proposes.ContainsKey(newPoint))
					proposes[newPoint] = new List<Point> { elf };
				else
					proposes[newPoint].Add(elf);
			}
			
			foreach (var (key, value) in proposes)
			{
				if (value.Count > 1)
					continue;
				
				grid[key] = "#";
				grid[value.First()] = ".";
			}
			
			directions.AddLast(directions.First.Value);
			directions.RemoveFirst();
		}
		
		var minX = grid.Where(x => x.Value == "#").Min(x => x.Key.X);
		var maxX = grid.Where(x => x.Value == "#").Max(x => x.Key.X);
		var minY = grid.Where(x => x.Value == "#").Min(x => x.Key.Y);
		var maxY = grid.Where(x => x.Value == "#").Max(x => x.Key.Y);

		var answer = 0;
		for (var y = minY; y <= maxY; y++)
		{
			for (var x = minX; x <= maxX; x++)
			{
				answer += grid.GetValueOrDefault(new Point(x, y), ".") == "." ? 1 : 0;
			}
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		const int rounds = 100000;

		var directions = new LinkedList<Point>();
		directions.AddLast(new Point(0, -1)); // N
		directions.AddLast(new Point(0, 1)); // S
		directions.AddLast(new Point(-1, 0)); // W
		directions.AddLast(new Point(1, 0)); // E

		for (var i = 0; i < rounds; i++)
		{
			var proposes = new Dictionary<Point, List<Point>>();
			var elves = grid.Where(x => x.Value == "#").Select(x => x.Key).ToList();
			
			foreach (var elf in elves)
			{
				var currentDirection = directions.First;
				var adjacent = GetAdjacent(elf, new Point(0, 0));
				if (adjacent.TrueForAll(x => grid.GetValueOrDefault(x, ".") == "."))
					continue;

				while (currentDirection != null)
				{
					adjacent = GetAdjacent(elf, currentDirection.Value);
					
					if (adjacent.TrueForAll(x => grid.GetValueOrDefault(x, ".") == "."))
						break;
					
					currentDirection = currentDirection.Next;
				}
				
				if (currentDirection == null)
					continue;
				
				var newPoint = new Point(elf.X + currentDirection.Value.X, elf.Y + currentDirection.Value.Y);

				if (!proposes.ContainsKey(newPoint))
					proposes[newPoint] = new List<Point> { elf };
				else
					proposes[newPoint].Add(elf);
			}

			if (proposes.Count == 0)
				return i + 1;
			
			foreach (var (key, value) in proposes)
			{
				if (value.Count > 1)
					continue;
				
				grid[key] = "#";
				grid[value.First()] = ".";
			}
			
			directions.AddLast(directions.First.Value);
			directions.RemoveFirst();
		}
		
		var minX = grid.Where(x => x.Value == "#").Min(x => x.Key.X);
		var maxX = grid.Where(x => x.Value == "#").Max(x => x.Key.X);
		var minY = grid.Where(x => x.Value == "#").Min(x => x.Key.Y);
		var maxY = grid.Where(x => x.Value == "#").Max(x => x.Key.Y);

		var answer = 0;
		for (var y = minY; y <= maxY; y++)
		{
			for (var x = minX; x <= maxX; x++)
			{
				answer += grid.GetValueOrDefault(new Point(x, y), ".") == "." ? 1 : 0;
			}
		}
		
		return answer;
	}
	
	private static List<Point> GetAdjacent(Point point, Point direction)
	{
		// all 8 positions
		if (direction == new Point(0, 0))
		{
			return new List<Point>
			{
				new(-1, -1), new(0, -1), new(1, -1),
				new(-1, 0), new(1, 0),
				new(-1, 1), new(0, 1), new(1, 1),
			}.Select(x => new Point(point.X + x.X, point.Y + x.Y)).ToList();
		}

		return direction switch
		{
			{ X: 0, Y: -1 } => new List<Point>
			{
				new(point.X - 1, point.Y - 1),
				point with { Y = point.Y - 1 },
				new(point.X + 1, point.Y - 1),
			},
			{ X: 0, Y: 1 } => new List<Point>
			{
				new(point.X - 1, point.Y + 1),
				point with { Y = point.Y + 1 },
				new(point.X + 1, point.Y + 1),
			},
			{ X: -1, Y: 0 } => new List<Point>
			{
				new(point.X - 1, point.Y - 1),
				point with { X = point.X - 1 },
				new(point.X - 1, point.Y + 1),
			},
			{ X: 1, Y: 0 } => new List<Point>
			{
				new(point.X + 1, point.Y - 1),
				point with { X = point.X + 1 },
				new(point.X + 1, point.Y + 1),
			},
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}

	private static Dictionary<Point, string> ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, string>();
		var y = 0;
		foreach (var line in input)
		{
			for (var x = 0; x < line.Length; x++)
			{
				grid[new Point(x, y)] = line[x].ToString();
			}

			y++;
		}

		return grid;
	}
}