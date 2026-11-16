using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._16;

[UsedImplicitly]
public class Year2024Day16 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);
		var start = grid.Single(x => x.Value == "S").Key;
		var end = grid.Single(x => x.Value == "E").Key;

		var queue = new Queue<(Point Current, int Points, (int X, int Y) Direction)>();
		queue.Enqueue((start, 0, (1, 0)));

		var visited = new Dictionary<(Point Position, (int, int) Direction), int>();

		var minPoints = int.MaxValue;
		while (queue.Count != 0)
		{
			var (current, points, direction) = queue.Dequeue();
			if (points > minPoints)
				continue;

			if (current == end)
			{
				minPoints = Math.Min(minPoints, points);
				continue;
			}


			if (visited.GetValueOrDefault((current, direction), int.MaxValue) > points)
				visited[(current, direction)] = points;
			else
				continue;

			if (direction.X != 0)
			{
				queue.Enqueue((current, points + 1000, (0, -1)));
				queue.Enqueue((current, points + 1000, (0, 1)));
			}
			else
			{
				queue.Enqueue((current, points + 1000, (-1, 0)));
				queue.Enqueue((current, points + 1000, (1, 0)));
			}

			if (grid[new Point(current.X + direction.X, current.Y + direction.Y)] != "#")
				queue.Enqueue((new Point(current.X + direction.X, current.Y + direction.Y), points + 1, direction));
		}

		return minPoints;
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = ParseGrid(input);
		var start = grid.Single(x => x.Value == "S").Key;
		var end = grid.Single(x => x.Value == "E").Key;

		var queue = new Queue<((Point Current, (int X, int Y) Direction) Pos, int Points, List<Point> Path)>();
		queue.Enqueue(((start, (1, 0)), 0, new List<Point> { start }));

		var minPoints = int.MaxValue;
		var bestPaths = new List<List<Point>>();
		var minScores = new DefaultDictionary<(Point Current, (int X, int Y) Direction), int>(int.MaxValue);

		while (queue.Count != 0)
		{
			var (pos, points, path) = queue.Dequeue();
			if (points > minPoints)
				continue;

			if (pos.Current == end)
			{
				if (points == minPoints)
				{
					bestPaths.Add(path);
				}
				else if (points < minPoints)
				{
					bestPaths = new List<List<Point>> { path };
					minPoints = points;
				}
			}

			if (grid[new Point(pos.Current.X + pos.Direction.X, pos.Current.Y + pos.Direction.Y)] != "#")
				EnqueueIfBetter((
					(new Point(pos.Current.X + pos.Direction.X, pos.Current.Y + pos.Direction.Y), pos.Direction),
					points + 1,
					new List<Point>(path)
						{ new(pos.Current.X + pos.Direction.X, pos.Current.Y + pos.Direction.Y) }));

			if (pos.Direction.X != 0)
			{
				EnqueueIfBetter(((pos.Current, (0, -1)), points + 1000, path));
				EnqueueIfBetter(((pos.Current, (0, 1)), points + 1000, path));
			}
			else
			{
				EnqueueIfBetter(((pos.Current, (-1, 0)), points + 1000, path));
				EnqueueIfBetter(((pos.Current, (1, 0)), points + 1000, path));
			}
		}

		void EnqueueIfBetter(((Point Current, (int X, int Y) Direction) Pos, int Points, List<Point> Path) candidate)
		{
			if (minScores[candidate.Pos] >= candidate.Points)
			{
				minScores[candidate.Pos] = candidate.Points;
				queue.Enqueue(candidate);
			}
		}

		return bestPaths.SelectMany(x => x).Distinct().Count();
	}

	private class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : new() where TKey : notnull
	{
		private TValue DefaultValue { get; }

		public DefaultDictionary(TValue defaultValue)
		{
			DefaultValue = defaultValue;
		}

		public new TValue this[TKey key]
		{
			get
			{
				if (TryGetValue(key, out var val))
					return val;

				val = DefaultValue;

				Add(key, val);
				return val;
			}
			set => base[key] = value;
		}
	}

	private static Dictionary<Point, string> ParseGrid(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, string>();
		var enumerable = input as string[] ?? input.ToArray();
		for (var y = 0; y < enumerable.Length; y++)
		{
			for (var x = 0; x < enumerable[y].Length; x++)
			{
				var c = enumerable[y][x];
				var p = new Point(x, y);
				grid.Add(p, c.ToString());
			}
		}

		return grid;
	}
}