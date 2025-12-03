using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._16;

[UsedImplicitly]
public class Year2023Day16 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		
		var start = (new Point(0, 0), Direction.Right);
		var energized = Energize(start, grid);
		
		return energized.Count;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseInput(input);
		
		var maxX = grid.Keys.Max(x => x.X);
		var maxY = grid.Keys.Max(x => x.Y);

		var maxEnergized = 0;
		for (var i = 0; i <= maxX; i++)
		{
			var start = (new Point(i, 0), Direction.Down);
			var energized = Energize(start, grid);
			if (energized.Count > maxEnergized)
				maxEnergized = energized.Count;
		}
		for (var i = 0; i <= maxY; i++)
		{
			var start = (new Point(0, i), Direction.Right);
			var energized = Energize(start, grid);
			if (energized.Count > maxEnergized)
				maxEnergized = energized.Count;
		}
		for (var i = 0; i <= maxX; i++)
		{
			var start = (new Point(i, maxY), Direction.Up);
			var energized = Energize(start, grid);
			if (energized.Count > maxEnergized)
				maxEnergized = energized.Count;
		}
		for (var i = 0; i <= maxY; i++)
		{
			var start = (new Point(maxX, i), Direction.Left);
			var energized = Energize(start, grid);
			if (energized.Count > maxEnergized)
				maxEnergized = energized.Count;
		}
		
		return maxEnergized;
	}
	
	private static HashSet<Point> Energize((Point, Direction Right) start, Dictionary<Point, string> grid)
	{
		var q = new Queue<(Point, Direction)>();
		q.Enqueue(start);
		var visited = new HashSet<(Point, Direction)>();
		var energized = new HashSet<Point>();
		while (q.Any())
		{
			var (currentPoint, currentDirection) = q.Dequeue();
			if (!visited.Add((currentPoint, currentDirection)))
				continue;
			
			energized.Add(currentPoint);

			if (grid[currentPoint] == "." ||
			    (grid[currentPoint] == "|" && currentDirection is Direction.Up or Direction.Down) ||
			    (grid[currentPoint] == "-" && currentDirection is Direction.Left or Direction.Right))
			{
				var nextPoint = currentDirection switch
				{
					Direction.Up => currentPoint with { Y = currentPoint.Y - 1 },
					Direction.Down => currentPoint with { Y = currentPoint.Y + 1 },
					Direction.Left => currentPoint with { X = currentPoint.X - 1 },
					Direction.Right => currentPoint with { X = currentPoint.X + 1 },
				};
				if (!grid.ContainsKey(nextPoint))
					continue;
				
				q.Enqueue((nextPoint, currentDirection));
				continue;
			}

			if (grid[currentPoint] == "|")
			{
				var up = currentPoint with { Y = currentPoint.Y - 1 };
				var down = currentPoint with { Y = currentPoint.Y + 1 };
				if (grid.ContainsKey(up))
					q.Enqueue((up, Direction.Up));
				if (grid.ContainsKey(down))
					q.Enqueue((down, Direction.Down));
				
				continue;
			}

			if (grid[currentPoint] == "-")
			{
				var left = currentPoint with { X = currentPoint.X - 1 };
				var right = currentPoint with { X = currentPoint.X + 1 };
				if (grid.ContainsKey(left))
					q.Enqueue((left, Direction.Left));
				if (grid.ContainsKey(right))
					q.Enqueue((right, Direction.Right));
				
				continue;
			}

			if (grid[currentPoint] == "/")
			{
				var next = currentDirection switch
				{
					Direction.Up => currentPoint with { X = currentPoint.X + 1 },
					Direction.Down => currentPoint with { X = currentPoint.X - 1 },
					Direction.Left => currentPoint with { Y = currentPoint.Y + 1 },
					Direction.Right => currentPoint with { Y = currentPoint.Y - 1 },
				};
				if (!grid.ContainsKey(next))
					continue;
				
				var nextDirection = currentDirection switch
				{
					Direction.Up => Direction.Right,
					Direction.Down => Direction.Left,
					Direction.Left => Direction.Down,
					Direction.Right => Direction.Up,
				};
				q.Enqueue((next, nextDirection));
				continue;
			}
			
			if (grid[currentPoint] == "\\")
			{
				var next = currentDirection switch
				{
					Direction.Up => currentPoint with { X = currentPoint.X - 1 },
					Direction.Down => currentPoint with { X = currentPoint.X + 1 },
					Direction.Left => currentPoint with { Y = currentPoint.Y - 1 },
					Direction.Right => currentPoint with { Y = currentPoint.Y + 1 },
				};
				if (!grid.ContainsKey(next))
					continue;
				
				var nextDirection = currentDirection switch
				{
					Direction.Up => Direction.Left,
					Direction.Down => Direction.Right,
					Direction.Left => Direction.Up,
					Direction.Right => Direction.Down,
				};
				q.Enqueue((next, nextDirection));
				continue;
			}
		}

		return energized;
	}
	
	private enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}
	
	private static Dictionary<Point, string> ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, string>();
		var lines = input.ToList();
		for (var y = 0; y < lines.Count; y++)
		{
			var line = lines[y];
			for (var x = 0; x < line.Length; x++)
			{
				grid[new Point(x, y)] = line[x].ToString();
			}
		}

		return grid;
	}
}