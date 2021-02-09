using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._3
{
	// ReSharper disable once UnusedMember.Global
	public class Year2016Day03 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var triangleSides = GetTriangleSides(input);
			var validTriangles = triangleSides.Count(IsTriangleValid);
			return validTriangles.ToString();
		}
		
		public object Part2(IEnumerable<string> input)
		{
			var triangleSides = GetVerticalTriangleSides(input);
			var validTriangles = triangleSides.Count(IsTriangleValid);
			return validTriangles.ToString();
		}

		private static bool IsTriangleValid(IReadOnlyList<int> triangleSide)
		{
			return triangleSide[0] + triangleSide[1] > triangleSide[2]
			       && triangleSide[1] + triangleSide[2] > triangleSide[0]
			       && triangleSide[0] + triangleSide[2] > triangleSide[1];
		}

		

		private static IEnumerable<IReadOnlyList<int>> GetTriangleSides(IEnumerable<string> input)
		{
			return input.Select(r => r.Split().Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray());
		}
		
		private static IEnumerable<IReadOnlyList<int>> GetVerticalTriangleSides(IEnumerable<string> input)
		{
			var horizontalSides = input.Select(r => r.Split().Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray()).ToArray();
			for (var i = 0; i <= horizontalSides.Length - 3; i += 3)
			{
				for (var j = 0; j < 3; j++)
				{
					yield return new[]
					{
						horizontalSides[i][j], 
						horizontalSides[i + 1][j], 
						horizontalSides[i + 2][j]
					};
				}
			}
		}
	}
}