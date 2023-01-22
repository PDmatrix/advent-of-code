using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2020._12;

[UsedImplicitly]
public class Year2020Day12 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		/*
		input = new[]
		{
			"F10",
			"N3",
			"F7",
			"R90",
			"F11",
		};
		*/
		var current = new Point(0, 0);
		var currentAngle = 0;
		var movementInstructions = new HashSet<char>
		{
			'N', 'E', 'W', 'S'
		};
		foreach (var line in input)
		{
			var instruction = line[0];
			var units = int.Parse(string.Join(string.Empty, line.Skip(1)));
			if (movementInstructions.Contains(instruction))
			{
				current = instruction switch
				{
					'N' => current with { Y = current.Y + units },
					'S' => current with { Y = current.Y - units },
					'W' => current with { X = current.X - units },
					'E' => current with { X = current.X + units },
					_ => throw new Exception("invalid instruction")
				};
			} else if (instruction == 'F')
			{
				current = currentAngle switch
				{
					0 => current with { X = current.X + units },
					90 => current with { Y = current.Y + units },
					180 => current with { X = current.X - units },
					270 => current with { Y = current.Y - units },
					_ => throw new Exception("invalid angle")
				};
			}
			else
			{
				currentAngle = instruction switch
				{
					'L' => currentAngle + units,
					'R' => currentAngle - units
				};
				if (currentAngle >= 360)
					currentAngle -= 360;
				
				if (currentAngle < 0)
					currentAngle += 360;
			}
		}
		
		return Math.Abs(current.X) + Math.Abs(current.Y);
	}

	public object Part2(IEnumerable<string> input)
	{
		var currentShip = new Point(0, 0);
		var currentWaypoint = new Point(10, 1);
		var movementInstructions = new HashSet<char>
		{
			'N', 'E', 'W', 'S'
		};
		foreach (var line in input)
		{
			var instruction = line[0];
			var units = int.Parse(string.Join(string.Empty, line.Skip(1)));
			if (movementInstructions.Contains(instruction))
			{
				currentWaypoint = instruction switch
				{
					'N' => currentWaypoint with { Y = currentWaypoint.Y + units },
					'S' => currentWaypoint with { Y = currentWaypoint.Y - units },
					'W' => currentWaypoint with { X = currentWaypoint.X - units },
					'E' => currentWaypoint with { X = currentWaypoint.X + units },
					_ => throw new Exception("invalid instruction")
				};
			} else if (instruction == 'F')
			{
				currentShip = new Point(currentShip.X + units * currentWaypoint.X,
					currentShip.Y + units * currentWaypoint.Y);
			}
			else
			{
				if (instruction == 'R')
				{
					for (var angle = 0; angle < units; angle += 90)
						currentWaypoint = currentWaypoint with { X = currentWaypoint.Y, Y = -1 * currentWaypoint.X };
				}
				else
				{
					for (var angle = 0; angle < units; angle += 90)
						currentWaypoint = currentWaypoint with { X = -1 * currentWaypoint.Y, Y = currentWaypoint.X };
				}
			}
		}
		
		return Math.Abs(currentShip.X) + Math.Abs(currentShip.Y);
	}
}