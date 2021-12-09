using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2017._20;

[UsedImplicitly]
public class Year2017Day20 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var particles = ParseInput(input).ToList();
		for (var i = 0; i < 1000; i++)
		{
			particles.ForEach(x => x.Tick());
		}

		var minDistanceFromCenter = particles.Min(x => Math.Abs(x.DistanceFromCenter));
			
		return particles.Select((particle, index) => new {particle, index})
			.First(x => x.particle.DistanceFromCenter == minDistanceFromCenter).index.ToString();
	}

	public object Part2(IEnumerable<string> input)
	{
		var particles = ParseInput(input).ToList();
		for (var i = 0; i < 1000; i++)
		{
			var set = new HashSet<(long x, long y, long z)>();
			var delete = new List<(long x, long y, long z)>();
			particles.ForEach(x =>
			{
				x.Tick();
				var position = (x.PositionX, x.PositionY, x.PositionZ);
				if (set.Contains(position))
				{
					delete.Add(position);
				}
				else
				{
					set.Add(position);
				}
			});
				
			particles.RemoveAll(x => delete.Contains((x.PositionX, x.PositionY, x.PositionZ)));
		}

		return particles.Count.ToString();
	}

	private static IEnumerable<Particle> ParseInput(IEnumerable<string> input)
	{
		var regex = new Regex(@"p=<(?<px>-?\d+),(?<py>-?\d+),(?<pz>-?\d+)>, v=<(?<vx>-?\d+),(?<vy>-?\d+),(?<vz>-?\d+)>, a=<(?<ax>-?\d+),(?<ay>-?\d+),(?<az>-?\d+)>", RegexOptions.Compiled);

		foreach (var particle in input)
		{
			var groups = regex.Match(particle).Groups;
			yield return new Particle
			{
				PositionX = int.Parse(groups["px"].Value),
				PositionY = int.Parse(groups["py"].Value),
				PositionZ = int.Parse(groups["pz"].Value),
				VelocityX = int.Parse(groups["vx"].Value),
				VelocityY = int.Parse(groups["vy"].Value),
				VelocityZ = int.Parse(groups["vz"].Value),
				AccelerationX = int.Parse(groups["ax"].Value),
				AccelerationY = int.Parse(groups["ay"].Value),
				AccelerationZ = int.Parse(groups["az"].Value),
			};
		}
	}

	private class Particle
	{
		public long PositionX { get; set; }
		public long PositionY { get; set; }
		public long PositionZ { get; set; }

		public long VelocityX { get; set; }
		public long VelocityY { get; set; }
		public long VelocityZ { get; set; }
			
		public long AccelerationX { get; set; }
		public long AccelerationY { get; set; }
		public long AccelerationZ { get; set; }
			
		public long DistanceFromCenter { get; set; }

		public void Tick()
		{
			VelocityX += AccelerationX;
			VelocityY += AccelerationY;
			VelocityZ += AccelerationZ;

			PositionX += VelocityX;
			PositionY += VelocityY;
			PositionZ += VelocityZ;

			DistanceFromCenter = Math.Abs(PositionX) + Math.Abs(PositionY) + Math.Abs(PositionZ);
		}
	}
}