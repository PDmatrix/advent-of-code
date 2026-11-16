using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._24;

[UsedImplicitly]
public class Year2023Day24 : ISolution
{
	// this puzzle was pretty hard for me (math; bad) so code is AI generated (shame!)
	public object Part1(IEnumerable<string> input)
	{
		const long min = 200000000000000;
		const long max = 400000000000000;

		var hailstones = input.Where(line => !string.IsNullOrWhiteSpace(line))
			.Select(line => line.Split([',', '@'], StringSplitOptions.TrimEntries)
				.Select(BigInteger.Parse).ToArray())
			.Select(values => (X: values[0], Y: values[1], Vx: values[3], Vy: values[4]))
			.ToArray();

		var intersections = 0;
		for (var i = 0; i < hailstones.Length; i++)
		{
			for (var j = i + 1; j < hailstones.Length; j++)
			{
				var a = hailstones[i];
				var b = hailstones[j];
				var determinant = a.Vx * b.Vy - a.Vy * b.Vx;

				// Parallel or coincident lines have no unique crossing point.
				if (determinant.IsZero)
					continue;

				var dx = b.X - a.X;
				var dy = b.Y - a.Y;
				// Each hailstone reaches the crossing at its own time, t / determinant.
				var tA = dx * b.Vy - dy * b.Vx;
				var tB = dx * a.Vy - dy * a.Vx;
				if (determinant.Sign < 0)
				{
					determinant = -determinant;
					tA = -tA;
					tB = -tB;
				}

				if (tA.Sign < 0 || tB.Sign < 0)
					continue;

				// Compare scaled coordinates without division or floating-point rounding.
				var x = a.X * determinant + a.Vx * tA;
				var y = a.Y * determinant + a.Vy * tA;
				if (x >= min * determinant && x <= max * determinant &&
				    y >= min * determinant && y <= max * determinant)
					intersections++;
			}
		}

		return intersections;
	}

	public object Part2(IEnumerable<string> input)
	{
		var hailstones = input.Where(line => !string.IsNullOrWhiteSpace(line))
			.Select(line => line.Split([',', '@'], StringSplitOptions.TrimEntries)
				.Select(BigInteger.Parse).ToArray())
			.ToArray();
		if (hailstones.Length < 3)
			throw new InvalidOperationException("At least three hailstones are required.");

		// A collision implies (rockPosition - p) x (rockVelocity - v) = 0.
		// Subtract two hailstones' equations to cancel rockPosition x rockVelocity:
		// r x (vB - vA) + (pB - pA) x u = pB x vB - pA x vA.
		// The six unknowns are [rx, ry, rz, ux, uy, uz].
		var equations = new List<BigInteger[]>();
		var a = hailstones[0];
		foreach (var b in hailstones.Skip(1))
		{
			for (var axisA = 0; axisA < 2; axisA++)
			{
				for (var axisB = axisA + 1; axisB < 3; axisB++)
				{
					var row = new BigInteger[7];
					row[axisA] = b[axisB + 3] - a[axisB + 3];
					row[axisB] = a[axisA + 3] - b[axisA + 3];
					row[axisA + 3] = a[axisB] - b[axisB];
					row[axisB + 3] = b[axisA] - a[axisA];
					row[6] = b[axisA] * b[axisB + 3] - b[axisB] * b[axisA + 3]
					         - a[axisA] * a[axisB + 3] + a[axisB] * a[axisA + 3];
					equations.Add(row);
				}
			}
		}

		var rock = SolveLinearSystem(equations);
		foreach (var hailstone in hailstones)
		{
			if (!CollidesInFuture(rock, hailstone))
				throw new InvalidOperationException("The rock does not collide with every hailstone in the future.");
		}

		return rock[0] + rock[1] + rock[2];
	}

	private static BigInteger[] SolveLinearSystem(List<BigInteger[]> rows)
	{
		const int unknowns = 6;
		for (var column = 0; column < unknowns; column++)
		{
			// Use all hailstones so parallel/degenerate early pairs do not prevent a solution.
			var pivot = rows.FindIndex(column, row => !row[column].IsZero);
			if (pivot < 0)
				throw new InvalidOperationException("The hailstones do not determine a unique rock trajectory.");
			(rows[column], rows[pivot]) = (rows[pivot], rows[column]);

			for (var i = column + 1; i < rows.Count; i++)
			{
				if (rows[i][column].IsZero)
					continue;

				// Fraction-free elimination; reduce common factors to keep integers small.
				var gcd = BigInteger.GreatestCommonDivisor(rows[column][column], rows[i][column]);
				var multiplier = rows[column][column] / gcd;
				var factor = rows[i][column] / gcd;
				var rowGcd = BigInteger.Zero;
				for (var j = column; j <= unknowns; j++)
				{
					rows[i][j] = rows[i][j] * multiplier - rows[column][j] * factor;
					rowGcd = BigInteger.GreatestCommonDivisor(rowGcd, rows[i][j]);
				}

				if (rowGcd > BigInteger.One)
					for (var j = column + 1; j <= unknowns; j++)
						rows[i][j] /= rowGcd;
			}
		}

		if (rows.Skip(unknowns).Any(row => !row[unknowns].IsZero))
			throw new InvalidOperationException("The hailstone trajectories are inconsistent.");

		var solution = new BigInteger[unknowns];
		for (var i = unknowns - 1; i >= 0; i--)
		{
			var value = rows[i][unknowns];
			for (var j = i + 1; j < unknowns; j++)
				value -= rows[i][j] * solution[j];

			solution[i] = BigInteger.DivRem(value, rows[i][i], out var remainder);
			if (!remainder.IsZero)
				throw new InvalidOperationException("The rock must have integer position and velocity.");
		}

		return solution;
	}

	private static bool CollidesInFuture(BigInteger[] rock, BigInteger[] hailstone)
	{
		var axis = -1;
		for (var i = 0; i < 3; i++)
			if (rock[i + 3] != hailstone[i + 3])
			{
				axis = i;
				break;
			}

		if (axis < 0)
			return Enumerable.Range(0, 3).All(i => rock[i] == hailstone[i]);

		// Time is distance / relativeVelocity; it need not be an integer.
		var distance = hailstone[axis] - rock[axis];
		var relativeVelocity = rock[axis + 3] - hailstone[axis + 3];
		if (!distance.IsZero && distance.Sign != relativeVelocity.Sign)
			return false;

		return Enumerable.Range(0, 3).All(i =>
			(hailstone[i] - rock[i]) * relativeVelocity ==
			distance * (rock[i + 3] - hailstone[i + 3]));
	}
}
