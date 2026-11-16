using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._21;

[UsedImplicitly]
public class Year2023Day21 : ISolution
{
	// cudos to u/Old_Smoke_3382 https://www.reddit.com/r/adventofcode/comments/18nevo3/comment/keanapt/
	public object Part1(IEnumerable<string> input)
	{
		var list = input.ToList();
		var gridSize = list.Count == list[0].Length ? list.Count : throw new ArgumentOutOfRangeException();

		var start = Enumerable.Range(0, gridSize)
			.SelectMany(i => Enumerable.Range(0, gridSize)
				.Where(j => list[i][j] == 'S')
				.Select(j => new Coord(i, j)))
			.Single();

		var work = new HashSet<Coord> { start };
		for (var i = 0; i < 64; i++)
		{
			work = new HashSet<Coord>(work
				.SelectMany(it => new[] { Dir.N, Dir.S, Dir.E, Dir.W }.Select(dir => it.Move(dir)))
				.Where(dest => dest.X >= 0 && dest.Y >= 0 && dest.X < gridSize && dest.Y < gridSize && list[(int)dest.X][(int)dest.Y] != '#'));
		}

		return work.Count;
	}

	public object Part2(IEnumerable<string> input)
	{
		var list = input.ToList();
		var gridSize = list.Count == list[0].Length ? list.Count : throw new ArgumentOutOfRangeException();

		var start = Enumerable.Range(0, gridSize)
			.SelectMany(i => Enumerable.Range(0, gridSize)
				.Where(j => list[i][j] == 'S')
				.Select(j => new Coord(i, j)))
			.Single();

		var grids = 26501365 / gridSize;
		var rem = 26501365 % gridSize;

		// By inspection, the grid is square and there are no barriers on the direct horizontal / vertical path from S
		// So, we'd expect the result to be quadratic in (rem + n * gridSize) steps, i.e. (rem), (rem + gridSize), (rem + 2 * gridSize), ...
		// Use the code from Part 1 to calculate the first three values of this sequence, which is enough to solve for ax^2 + bx + c
		var sequence = new List<int>();
		var work = new HashSet<Coord> { start };
		var steps = 0;
		for (var n = 0; n < 3; n++)
		{
			for (; steps < n * gridSize + rem; steps++)
			{
				// Funky modulo arithmetic bc modulo of a negative number is negative, which isn't what we want here
				work = new HashSet<Coord>(work
					.SelectMany(it => new[] { Dir.N, Dir.S, Dir.E, Dir.W }.Select(dir => it.Move(dir)))
					.Where(dest => list[(int)((dest.X % 131) + 131) % 131][(int)((dest.Y % 131) + 131) % 131] != '#'));
			}

			sequence.Add(work.Count);
		}

		// Solve for the quadratic coefficients
		var c = sequence[0];
		var aPlusB = sequence[1] - c;
		var fourAPlusTwoB = sequence[2] - c;
		var twoA = fourAPlusTwoB - (2 * aPlusB);
		var a = twoA / 2;
		var b = aPlusB - a;

		long F(long n)
		{
			return a * (n * n) + b * n + c;
		}

		for (var i = 0; i < sequence.Count; i++)
		{
			Console.WriteLine($"{sequence[i]} : {F(i)}");
		}

		return F(grids);
	}

	public record Coord(long X, long Y)
	{
		public Coord Move(Dir dir, int dist = 1)
		{
			return dir switch
			{
				Dir.N => new Coord(this.X - dist, this.Y),
				Dir.S => new Coord(this.X + dist, this.Y),
				Dir.E => new Coord(this.X, this.Y + dist),
				Dir.W => new Coord(this.X, this.Y - dist),
			};
		}
	}
	
	public enum Dir
	{
		N, S, E, W
	}
}

