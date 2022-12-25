using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2019._10;

[UsedImplicitly]
public class Year2019Day10 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var enumerable = input as string[] ?? input.ToArray();
        var grid = CreateGrid(enumerable);
        
        return GetBaseAsteroid(grid).Value.Count;
    }

    private static List<Point> CreateGrid(string[] enumerable)
    {
        var grid = new List<Point>();
        for (var y = 0; y < enumerable.Length; y++)
        {
            for (var x = 0; x < enumerable[y].Length; x++)
            {
                if (enumerable[y][x] != '#') continue;
                grid.Add(new Point(x, y));
            }
        }

        return grid;
    }


    private static double Distance(Point a1, Point a2)
    {
        return Math.Sqrt(Math.Pow(a1.X - a2.X, 2) + Math.Pow(a1.Y - a2.Y, 2));
    }
    private static bool IsVisible(Point a1, Point a2, List<Point> asteroids)
    {
        if (a1 == a2)
            return false;
        
        foreach (var asteroid in asteroids)
        {
            if (a1 == asteroid || a2 == asteroid)
                continue;

            if (Math.Abs(Distance(a1, asteroid) + Distance(asteroid, a2) - Distance(a1, a2)) < 0.000001)
                return false;
        }

        return true;
    }
    
    private static double ConvertRadiansToDegrees(double radians)
    {
        return 180 / Math.PI * radians;
    }

    private static double GetAngle(Point a, Point b)
    {
        var angle = ConvertRadiansToDegrees(Math.Atan2(b.Y - a.Y, b.X - a.X)) - 90;
        if (angle < 0)
            angle += 360;
        
        return angle;
    }
    
    
    public object Part2(IEnumerable<string> input)
    {
        var enumerable = input as string[] ?? input.ToArray();
        var grid = CreateGrid(enumerable);

        var baseAsteroid = GetBaseAsteroid(grid).Key;
        var angles = new List<(double dist, Point ast)>();   
        foreach (var point in grid)
        {
            if (point == baseAsteroid)
                continue;
            
            if (!IsVisible(baseAsteroid, point, grid))
                continue;
            var angle = GetAngle(point, baseAsteroid);
            angles.Add((angle, point));
        }

        angles = angles
            .OrderBy(x => x.dist)
            .ToList();
        
        var index = 0;
        foreach (var angle in angles)
        {
            if (index == 199)
            {
                return angle.ast.X * 100 + angle.ast.Y;
            }
            index++;
        }

        return 2;
    }

    private static KeyValuePair<Point, List<Point>> GetBaseAsteroid(List<Point> grid)
    {
        var dict = new Dictionary<Point, List<Point>>();
        for (var i = 0; i < grid.Count; i++)
        {
            for (var j = i + 1; j < grid.Count; j++)
            {
                if (!IsVisible(grid[i], grid[j], grid)) continue;
                if (!dict.ContainsKey(grid[i]))
                    dict.Add(grid[i], new List<Point> { grid[j] });
                else
                    dict[grid[i]].Add(grid[j]);

                if (!dict.ContainsKey(grid[j]))
                    dict.Add(grid[j], new List<Point> { grid[i] });
                else
                    dict[grid[j]].Add(grid[i]);
            }
        }


        var baseAsteroidPair = dict.MaxBy(x => x.Value.Count);
        return baseAsteroidPair;
    }
}