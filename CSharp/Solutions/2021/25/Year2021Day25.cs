using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._25;

[UsedImplicitly]
public class Year2021Day25 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var grid = new Dictionary<Point, char>();
        var y = 0;
        foreach (var line in input)
        {
            var x = 0;
            foreach (var critter in line)
            {
                grid[new Point(x, y)] = critter;
                x++;
            }

            y++;
        }

        var maxX = grid.Max(x => x.Key.X);
        var maxY = grid.Max(x => x.Key.Y);

        var steps = 0;
        var count = 1;
        while (count != 0)
        {
            steps += 1;
            count = 0;
            var temp = grid.ToDictionary(x => x.Key, x => x.Value);
            for (var y1 = 0; y1 <= maxY; y1++)
            {
                for (var x1 = 0; x1 <= maxX; x1++)
                {
                    var oldPoint = new Point(x1, y1);
                    if (grid[oldPoint] != '>')
                        continue;
                    var newPoint = new Point((x1 + 1) % (maxX + 1), y1);
                    if (grid[newPoint] == '.')
                    {
                        temp[newPoint] = '>';
                        temp[oldPoint] = '.';
                        count++;
                    }
                }
            }
            
            grid = temp.ToDictionary(x => x.Key, x => x.Value);
            for (var y1 = 0; y1 <= maxY; y1++)
            {
                for (var x1 = 0; x1 <= maxX; x1++)
                {
                    var oldPoint = new Point(x1, y1);
                    if (grid[oldPoint] != 'v')
                        continue;
                    var newPoint = new Point(x1, (y1 + 1) % (maxY + 1));
                    if (grid[newPoint] == '.')
                    {
                        temp[newPoint] = 'v';
                        temp[oldPoint] = '.';
                        count++;
                    }
                }
            }

            grid = temp.ToDictionary(x => x.Key, x => x.Value);
        }
        
        return steps;
    }
    
    public object Part2(IEnumerable<string> input)
    {
        return "Congratulations!";
    }
}