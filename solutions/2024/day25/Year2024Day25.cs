using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._25;

[UsedImplicitly]
public class Year2024Day25 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var data = ParseInput(input);

        var keys = new List<List<int>>();
        var locks = new List<List<int>>();
        foreach (var d in data)
        {
            var l = new List<int>();
            
            for (var x = 0; x <= d.Max(c => c.Key.X); x++)
                l.Add(d.Count(c => c.Key.X == x && c.Value == "#") - 1);
            
            if (d.Where(x => x.Key.Y == 0).All(x => x.Value == "#"))
                locks.Add(l);
            else
                keys.Add(l);
        }

        var max = data.First().Max(x => x.Key.Y) - 1;
        var answer = 0;
        foreach (var key in keys)
        {
            foreach (var lo in locks)
            {
                var sumList = key.Zip(lo, (x,y) => x + y).ToList();
                if (sumList.TrueForAll(x => x <= max))
                {
                    answer++;
                }
            }
        }
        
        return answer;
    }

    private static List<Dictionary<Point, string>> ParseInput(IEnumerable<string> input)
    {
        var data = new List<Dictionary<Point, string>>();
        var d = new Dictionary<Point, string>();
        var y = 0;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                data.Add(d);
                y = 0;
                d = new Dictionary<Point, string>();
                continue;
            }

            var x = 0;
            foreach (var l in line)
            {
                d[new Point(x, y)] = l.ToString();
                x++;
            }

            y++;
        }
        data.Add(d);

        return data;
    }
    public object Part2(IEnumerable<string> input)
    {
        return "Congratulations!";
    }
}