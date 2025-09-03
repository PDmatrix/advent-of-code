using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._11;

[UsedImplicitly]
public class Year2023Day11 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var grid = FillColumn(FillRow(ParseInput(input)));
		
		var galaxies = grid.Where(x => x.Value == "#").Select(x => x.Key).ToList();
		var answer = 0;
		for (var i = 0; i < galaxies.Count; i++)
		{
			for (var j = i + 1; j < galaxies.Count; j++)
			{
				answer += GetManhattanDistance(galaxies[i], galaxies[j]);
			}
		}

		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var original = IncrementGalaxies(FillColumn(FillRow(ParseInput(input))));
		var grid = IncrementGalaxies(FillColumn(FillRow(ParseInput(input), 1), 1));

		var map = new Dictionary<(string a, string b), long>();
		var originalGalaxies = original.Where(x => x.Value != ".").Select(x => x.Key).ToList();
		for (var i = 0; i < originalGalaxies.Count; i++)
		{
			for (var j = i + 1; j < originalGalaxies.Count; j++)
			{
				map.Add((original[originalGalaxies[i]], original[originalGalaxies[j]]), GetManhattanDistance(originalGalaxies[i], originalGalaxies[j]));
			}
		}
		
		var galaxies = grid.Where(x => x.Value != ".").Select(x => x.Key).ToList();
		for (var i = 0; i < galaxies.Count; i++)
		{
			for (var j = i + 1; j < galaxies.Count; j++)
			{
				var distance = GetManhattanDistance(galaxies[i], galaxies[j]);
				var difference = (distance - map[(grid[galaxies[i]], grid[galaxies[j]])]) * 999999;
				map[(grid[galaxies[i]], grid[galaxies[j]])] += difference;
			}
		}

		return map.Values.Sum();
	}
	
	private static int GetManhattanDistance(Point a, Point b)
	{
		return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
	}

	private static Dictionary<Point, string> IncrementGalaxies(Dictionary<Point, string> grid)
	{
		var inc = 0;
		foreach (var point in grid.Keys.Where(point => grid[point] == "#"))
			grid[point] = inc++.ToString();
		
		return grid;
	}
	
	private static Dictionary<Point, string> ParseInput(IEnumerable<string> input)
	{
		var grid = new Dictionary<Point, string>();
		var y = 0;
		foreach (var line in input)
		{
			for (var x = 0; x < line.Length; x++)
				grid[new Point(x, y)] = line[x].ToString();

			y++;
		}

		return grid;
	}
	
	private static Dictionary<Point, string> FillRow(Dictionary<Point, string> grid, int times = 0)
	{
		var newGrid = new Dictionary<Point, string>();
		var maxX = grid.Keys.Max(x => x.X);
		var maxY = grid.Keys.Max(x => x.Y);
		var innerY = 0;
		
		for (var y = 0; y <= maxY; y++)
		{
			var isEmpty = true;
			for (var x = 0; x <= maxX; x++)
			{
				if (grid.GetValueOrDefault(new Point(x, y), ".") == "#")
					isEmpty = false;
				
				newGrid[new Point(x, y + innerY)] = grid.GetValueOrDefault(new Point(x, y), ".");
			}

			if (!isEmpty) 
				continue;

			for (var i = 0; i < times; i++)
			{
				innerY++;
				for (var x = 0; x <= maxX; x++)
					newGrid[new Point(x, y + innerY)] = ".";
			}
		}
		
		return newGrid;
	}

	private static Dictionary<Point, string> FillColumn(Dictionary<Point, string> grid, int times = 0)
	{
		var newGrid = new Dictionary<Point, string>();
		var maxX = grid.Keys.Max(x => x.X);
		var maxY = grid.Keys.Max(x => x.Y);
		var innerX = 0;

		for (var x = 0; x <= maxX; x++)
		{
			var isEmpty = true;
			for (var y = 0; y <= maxY; y++)
			{
				if (grid.GetValueOrDefault(new Point(x, y), ".") == "#")
					isEmpty = false;
				
				newGrid[new Point(x + innerX, y)] = grid.GetValueOrDefault(new Point(x, y), ".");
			}

			if (!isEmpty) 
				continue;

			for (var i = 0; i < times; i++)
			{
				innerX++;
				for (var y = 0; y <= maxY; y++)
					newGrid[new Point(x + innerX, y)] = ".";
			}
		}
		
		return newGrid;
	}
}