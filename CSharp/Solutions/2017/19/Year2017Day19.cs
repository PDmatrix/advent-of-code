using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._19
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day19 : ISolution
	{
		private enum Direction : byte
		{
			Right,
			Down,
			Left,
			Up
		}
		
		public string Part1(IEnumerable<string> input)
		{
			var grid = input.Select(x => x.Select(w => w).ToArray()).ToArray();

			var current = (x: 0, y: Array.IndexOf(grid[0], '|'));
			var direction = Direction.Down;
			var result = "";
			while (true)
			{
				var value = grid[current.x][current.y];
				if (value == ' ')
					break;
				
				if (value == '+')
				{
					var (x, y) = GetNextPosition(current, direction);
					if (grid[x][y] == ' ')
					{
						direction = GetNewDirection(current, grid, direction);
					}
				}

				if (char.IsLetter(value))
				{
					result += value;
				}

				current = GetNextPosition(current, direction);
			}
			
			return result;
		}

		private static (int x, int y) GetNextPosition((int x, int y) current, Direction direction)
		{
			return direction switch
			{
				Direction.Down => (current.x + 1, current.y),
				Direction.Left => (current.x, current.y - 1),
				Direction.Right => (current.x, current.y + 1),
				Direction.Up => (current.x - 1, current.y),
				_ => throw new Exception()
			};
		}
		
		private static Direction GetNewDirection((int x, int y) current, IReadOnlyList<char[]> grid, Direction direction)
		{
			var (x, y) = current;
			if (grid[x + 1][y] != ' ' && direction != Direction.Up) return Direction.Down;
			if (grid[x][y + 1] != ' ' && direction != Direction.Left) return Direction.Right;
			if (grid[x - 1][y] != ' ' && direction != Direction.Down) return Direction.Up;
			if (grid[x][y - 1] != ' ' && direction != Direction.Right) return Direction.Left;
			
			throw new Exception();
		}

		public string Part2(IEnumerable<string> input)
		{
			var grid = input.Select(x => x.Select(w => w).ToArray()).ToArray();

			var current = (x: 0, y: Array.IndexOf(grid[0], '|'));
			var direction = Direction.Down;
			var steps = 0;
			while (true)
			{
				var value = grid[current.x][current.y];
				if (value == ' ')
					break;
				
				if (value == '+')
				{
					var (x, y) = GetNextPosition(current, direction);
					if (grid[x][y] == ' ')
					{
						direction = GetNewDirection(current, grid, direction);
					}
				}
				
				current = GetNextPosition(current, direction);
				steps++;
			}

			return steps.ToString();
		}
	}
}