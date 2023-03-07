using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._20;

[UsedImplicitly]
public class Year2021Day20 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var algorithm = input.First();

        var grid = ParseInput(input.Skip(2));

        return Enhance(grid, algorithm, 2);
    }

    public object Part2(IEnumerable<string> input)
    {
        var algorithm = input.First();

        var grid = ParseInput(input.Skip(2));

        return Enhance(grid, algorithm, 50);
    }

    private static int Enhance(Dictionary<Point, char> grid, string algorithm, int count)
    {
        var defaultVal = '.';
        for (var steps = 0; steps < count; steps++)
        {
            var minX = grid.Min(x => x.Key.X);
            var maxX = grid.Max(x => x.Key.X);
            var minY = grid.Min(x => x.Key.Y);
            var maxY = grid.Max(x => x.Key.Y);

            var newGrid = grid.ToDictionary(x => x.Key, x => x.Value);

            for (var y = minY - 1; y <= maxY + 1; y++)
            {
                for (var x = minX - 1; x <= maxX + 1; x++)
                {
                    var newPoint = new Point(x, y);
                    newGrid[newPoint] = GetVal(grid, newPoint, algorithm, defaultVal);
                }
            }

            grid = newGrid.ToDictionary(x => x.Key, x => x.Value);
            defaultVal = defaultVal == '.' ? '#' : '.';
        }

        return grid.Count(x => x.Value == '#');
    }

    private static Dictionary<Point, char> ParseInput(IEnumerable<string> input)
    {
        var grid = new Dictionary<Point, char>();
        var yGrid = 0;
        foreach (var line in input)
        {
            var x = 0;
            foreach (var c in line)
            {
                grid.Add(new Point(x, yGrid), c);
                x++;
            }

            yGrid++;
        }

        return grid;
    }

    private static char GetVal(Dictionary<Point, char> grid, Point point, string algorithm, char defaultVal)
    {
        var diff = new List<(int x, int y)>
        {
            (1, 1), (0, 1), (-1, 1),
            (1, 0), (0, 0), (-1, 0),
            (1, -1), (0, -1), (-1, -1),
        };

        var sb = new StringBuilder();
        foreach (var (dx, dy) in diff)
        {
            var newPoint = new Point(point.X - dx, point.Y - dy);
            sb.Append(grid.GetValueOrDefault(newPoint, defaultVal) == '.' ? '0' : '1');
        }

        return algorithm[Convert.ToInt32(sb.ToString(), 2)];
    }
}