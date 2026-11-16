using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2021._17;

[UsedImplicitly]
public class Year2021Day17 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var regex = new Regex(@"target area: x=(?<fx>-?\d+)\.\.(?<sx>-?\d+), y=(?<fy>-?\d+)\.\.(?<sy>-?\d+)");
		var match = regex.Match(input.First());
		var to = new Point(int.Parse(match.Groups["sx"].Value), int.Parse(match.Groups["fy"].Value));

		return FactorialSum(Math.Abs(to.Y) - 1);
	}

	public object Part2(IEnumerable<string> input)
	{
		var regex = new Regex(@"target area: x=(?<fx>-?\d+)\.\.(?<sx>-?\d+), y=(?<fy>-?\d+)\.\.(?<sy>-?\d+)");
		var match = regex.Match(input.First());
		var from = new Point(int.Parse(match.Groups["fx"].Value), int.Parse(match.Groups["sy"].Value));
		var to = new Point(int.Parse(match.Groups["sx"].Value), int.Parse(match.Groups["fy"].Value));

		var hitCount = 0;
		bool IsHit(Point speed)
		{
			var position = new Point(0, 0);
			var maxHeight = 0;

			while (position.X < to.X && position.Y > to.Y)
			{
				position.X += speed.X;
				position.Y += speed.Y;
				speed = speed with
				{
					X = speed.X > 0 ? speed.X - 1 : 0,
					Y = speed.Y - 1,
				};

				maxHeight = Math.Max(maxHeight, position.Y);

				if (position.X >= from.X && position.Y <= from.Y && position.X <= to.X && position.Y >= to.Y)
					return true;
			}

			return false;
		}

		var minXVelocity = (int)Math.Ceiling((Math.Sqrt(1 + from.X * 8) - 1) / 2);
		for (var xVelocity = minXVelocity; xVelocity <= to.X; xVelocity++)
		for (var yVelocity = to.Y; yVelocity <= -to.Y; yVelocity++)
		{
			if (IsHit(new Point(xVelocity, yVelocity)))
				hitCount++;
		}

		return hitCount;
	}


	private static readonly Dictionary<int, int> Cache = new()
	{
		[0] = 0, [1] = 1
	};
	private static int FactorialSum(int n)
	{
		if (Cache.TryGetValue(n, out var value))
			return value;

		var sum = n + FactorialSum(n - 1);
		Cache[n] = sum;
		
		return Cache[n];
	}
}