using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._18;

[UsedImplicitly]
public class Year2023Day18 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		return GetArea(ParseInput(input));
	}

	private static object GetArea(List<Instruction> instructions)
	{
		var vertices = new List<(double X, double Y)>
		{
			(0, 0)
		};
		
		var current = (X: 0, Y: 0);
		foreach (var instruction in instructions)
		{
			switch (instruction.Direction)
			{
				case Direction.Up:
					current = (current.X, current.Y + instruction.Distance);
					break;
				case Direction.Down:
					current = (current.X, current.Y - instruction.Distance);
					break;
				case Direction.Left:
					current = (current.X - instruction.Distance, current.Y);
					break;
				case Direction.Right:
					current = (current.X + instruction.Distance, current.Y);
					break;
			};
			
			vertices.Add(current);
		}
		
		var area = PolygonArea(vertices);
		var perimeter = instructions.Sum(instruction => instruction.Distance);

		return area + perimeter / 2 + 1;
	}

	public object Part2(IEnumerable<string> input)
	{
		var instructions = ParseInputPart2(input);
		
		return GetArea(instructions);
	}
	
	private static double PolygonArea(IReadOnlyList<(double X, double Y)> polygon)
	{
		if (polygon == null) throw new ArgumentNullException(nameof(polygon));
		int n = polygon.Count;
		if (n == 0) return 0.0;

		var b = polygon[n - 1];
		double area = 0.0;

		for (int i = 0; i < n; i++)
		{
			var a = b;
			b = polygon[i];
			area += a.Y * b.X - a.X * b.Y;
		}

		return area / 2.0;
	}
	
	private static List<Instruction> ParseInput(IEnumerable<string> input)
	{
		var instructions = new List<Instruction>();
		foreach (var line in input)
		{
			var parts = line.Split(' ');
			var direction = parts[0] switch
			{
				"U" => Direction.Up,
				"D" => Direction.Down,
				"L" => Direction.Left,
				"R" => Direction.Right,
			};
			var distance = int.Parse(parts[1]);
			instructions.Add(new Instruction(direction, distance));
		}

		return instructions;
	}
	
	private static List<Instruction> ParseInputPart2(IEnumerable<string> input)
	{
		var instructions = new List<Instruction>();
		foreach (var line in input)
		{
			var parts = line.Split(' ');
			var hex = parts[2];
			var direction = hex[^2] switch
			{
				'3' => Direction.Up,
				'1' => Direction.Down,
				'2' => Direction.Left,
				'0' => Direction.Right,
			};
			var distance = int.Parse(hex[2..^2], NumberStyles.HexNumber);
			instructions.Add(new Instruction(direction, distance));
		}

		return instructions;
	}

	
	private record struct Instruction(Direction Direction, int Distance);
	
	private enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}
}