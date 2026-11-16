using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._22;

[UsedImplicitly]
public class Year2023Day22 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var simulation = Simulate(input);
		return simulation.Settled.Count(brick => simulation.Supports[brick.Id].All(above => simulation.SupportedBy[above].Count > 1));
	}

	public object Part2(IEnumerable<string> input)
	{
		var simulation = Simulate(input);
		return simulation.Settled.Sum(brick => CountFallingBricks(brick.Id, simulation.Supports, simulation.SupportedBy));
	}

	private static List<Brick> ParseInput(IEnumerable<string> input)
	{
		return input
			.Select((line, index) =>
			{
				var parts = line.Split('~');
				return new Brick(index, Point3D.Parse(parts[0]), Point3D.Parse(parts[1]));
			})
			.ToList();
	}

	private static Simulation Simulate(IEnumerable<string> input)
	{
		var bricks = ParseInput(input)
			.OrderBy(brick => brick.MinZ)
			.ThenBy(brick => brick.Id)
			.ToList();

		var settled = new List<Brick>(bricks.Count);
		var supports = bricks.ToDictionary(brick => brick.Id, _ => new HashSet<int>());
		var supportedBy = bricks.ToDictionary(brick => brick.Id, _ => new HashSet<int>());

		foreach (var brick in bricks)
		{
			var supportLevel = 0;
			foreach (var other in settled.Where(other => other.OverlapsXY(brick)))
			{
				if (other.MaxZ > supportLevel)
				{
					supportLevel = other.MaxZ;
				}
			}

			var fallen = brick.MoveToMinZ(supportLevel + 1);
			var directSupporters = settled
				.Where(other => other.MaxZ == supportLevel && other.OverlapsXY(fallen))
				.ToList();

			foreach (var supporter in directSupporters)
			{
				supports[supporter.Id].Add(fallen.Id);
				supportedBy[fallen.Id].Add(supporter.Id);
			}

			settled.Add(fallen);
		}

		return new Simulation(settled, supports, supportedBy);
	}

	private static int CountFallingBricks(
		int removedBrickId,
		Dictionary<int, HashSet<int>> supports,
		Dictionary<int, HashSet<int>> supportedBy)
	{
		var remainingSupports = supportedBy.ToDictionary(pair => pair.Key, pair => pair.Value.Count);
		var falling = new HashSet<int> { removedBrickId };
		var queue = new Queue<int>();
		queue.Enqueue(removedBrickId);

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			foreach (var above in supports[current])
			{
				remainingSupports[above]--;
				if (remainingSupports[above] == 0 && falling.Add(above))
				{
					queue.Enqueue(above);
				}
			}
		}

		return falling.Count - 1;
	}

	private record Simulation(
		List<Brick> Settled,
		Dictionary<int, HashSet<int>> Supports,
		Dictionary<int, HashSet<int>> SupportedBy);

	private record Brick
	{
		public Brick(int id, Point3D a, Point3D b)
		{
			Id = id;
			MinX = a.X < b.X ? a.X : b.X;
			MaxX = a.X > b.X ? a.X : b.X;
			MinY = a.Y < b.Y ? a.Y : b.Y;
			MaxY = a.Y > b.Y ? a.Y : b.Y;
			MinZ = a.Z < b.Z ? a.Z : b.Z;
			MaxZ = a.Z > b.Z ? a.Z : b.Z;
		}

		public int Id { get; }
		public int MinX { get; }
		public int MaxX { get; }
		public int MinY { get; }
		public int MaxY { get; }
		public int MinZ { get; }
		public int MaxZ { get; }

		public bool OverlapsXY(Brick other)
		{
			return MinX <= other.MaxX
			       && MaxX >= other.MinX
			       && MinY <= other.MaxY
			       && MaxY >= other.MinY;
		}

		public Brick MoveToMinZ(int newMinZ)
		{
			var height = MaxZ - MinZ;
			return new Brick(Id, new Point3D(MinX, MinY, newMinZ), new Point3D(MaxX, MaxY, newMinZ + height));
		}
	}

	private record Point3D(int X, int Y, int Z)
	{
		public static Point3D Parse(string s)
		{
			var parts = s.Split(',').Select(int.Parse).ToArray();
			return new Point3D(parts[0], parts[1], parts[2]);
		}
	}
}
