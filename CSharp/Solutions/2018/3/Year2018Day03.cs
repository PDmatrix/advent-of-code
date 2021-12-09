using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using CSharpx;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2018._3;

[UsedImplicitly]
public class Year2018Day03 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = new Dictionary<(int x, int y), int>();
		var claims = input as string[] ?? input.ToArray();

		foreach (var claim in claims)
		{
			var parsedClaim = ParseClaim(claim);
			for (var i = parsedClaim.X; i < parsedClaim.X + parsedClaim.XSize; i++)
			{
				for (var j = parsedClaim.Y; j < parsedClaim.Y + parsedClaim.YSize; j++)
				{
					var coords = (i, j);
					if (grid.ContainsKey(coords))
						grid[coords]++;
					else
						grid.Add(coords, 1);
				}
			}
		}
			
		return grid.Values.Count(x => x > 1).ToString();
	}

	public object Part2(IEnumerable<string> input)
	{
		var grid = new Dictionary<(int x, int y), int>();
		var claims = input as string[] ?? input.ToArray();

		foreach (var claim in claims)
		{
			var parsedClaim = ParseClaim(claim);
			for (var i = parsedClaim.X; i < parsedClaim.X + parsedClaim.XSize; i++)
			{
				for (var j = parsedClaim.Y; j < parsedClaim.Y + parsedClaim.YSize; j++)
				{
					var coords = (i, j);
					if (grid.ContainsKey(coords))
						grid[coords]++;
					else
						grid.Add(coords, 1);
				}
			}
		}

		foreach (var claim in claims)
		{
			var parsedClaim = ParseClaim(claim);
			var isOverlap = false;
			for (var i = parsedClaim.X; i < parsedClaim.X + parsedClaim.XSize; i++)
			{
				for (var j = parsedClaim.Y; j < parsedClaim.Y + parsedClaim.YSize; j++)
				{
					var coords = (i, j);
					if (grid[coords] > 1)
						isOverlap = true;
				}
			}

			if (!isOverlap)
				return parsedClaim.Id;
		}
			
		throw new Exception("No solutions found");
	}

	private static Claim ParseClaim(string claim)
	{
		const string regex = @"#(?<id>\d+) @ (?<x>\d+),(?<y>\d+): (?<xsize>\d+)x(?<ysize>\d+)";
			
		var groups = Regex.Match(claim, regex).Groups;

		return new Claim
		{
			Id = groups["id"].Value,
			X = int.Parse(groups["x"].Value),
			Y = int.Parse(groups["y"].Value),
			XSize = int.Parse(groups["xsize"].Value),
			YSize = int.Parse(groups["ysize"].Value)
		};
	}

	private class Claim
	{
		public string Id { get; set; } = null!;
		public int X { get; set; }
		public int Y { get; set; }
		public int XSize { get; set; }
		public int YSize { get; set; }
	}
}