using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._1
{
	// ReSharper disable once UnusedMember.Global
	public class Year2016Day01 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var instructions = input.First().Split(",").Select(r => r.Trim());
			var location = new {X = 0, Y = 0};
			// Directions: North - 0, East - 1, South - 2, West - 3
			var direction = 0;
			foreach (var instruction in instructions)
			{
				int steps;
				(direction, steps) = GetDirectionAndSteps(direction, instruction);
				if (direction == 0)
					location = new {location.X, Y = location.Y + steps};
				if (direction == 1)
					location = new {X = location.X + steps, location.Y};
				if (direction == 2)
					location = new {location.X, Y = location.Y - steps};
				if (direction == 3)
					location = new { X = location.X - steps, location.Y };
			}

			return (Math.Abs(location.X) + Math.Abs(location.Y)).ToString();
		}
		
		public string Part2(IEnumerable<string> input)
		{
			var instructions = input.First().Split(",").Select(r => r.Trim());
			var location = new {X = 0, Y = 0};
			var visited = new HashSet<object>{location};
			// Directions: North - 0, East - 1, South - 2, West - 3
			var direction = 0;
			foreach (var instruction in instructions)
			{
				int steps;
				(direction, steps) = GetDirectionAndSteps(direction, instruction);
				for (var i = 0; i < steps; i++)
				{
					if (direction == 0)
						location = new {location.X, Y = location.Y + 1};
					if (direction == 1)
						location = new {X = location.X + 1, location.Y};
					if (direction == 2)
						location = new {location.X, Y = location.Y - 1};
					if (direction == 3)
						location = new { X = location.X - 1, location.Y };
					if(visited.Contains(location))
						return (Math.Abs(location.X) + Math.Abs(location.Y)).ToString();
					visited.Add(location);
				}
			}
			throw new Exception("No solution");
		}

		private static (int, int) GetDirectionAndSteps(int previousDirection, string instruction)
		{
			var turn = instruction[0] == 'R' ? 1 : -1;
			var steps = int.Parse(instruction.Remove(0, 1));
			var newDirection = previousDirection + turn;
			newDirection = newDirection == -1 ? 3 : newDirection == 4 ? 0 : newDirection;
			return (newDirection, steps);
		}
	}
}