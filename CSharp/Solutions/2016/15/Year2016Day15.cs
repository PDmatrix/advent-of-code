using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2016._15;

[UsedImplicitly]
public class Year2016Day15 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        return CalculateTime(input).ToString();
    }
        
    public object Part2(IEnumerable<string> input)
    {
        var enumerable = input.ToList();
        enumerable.Add("Disc #7 has 11 positions; at time=0, it is at position 0.");
        return CalculateTime(enumerable).ToString();
    }

    private static int CalculateTime(IEnumerable<string> arrangement)
    {
        var dict = new Dictionary<string, (int pos, int curPos)>();
        const string pattern = 
            @"Disc #(?<disc>\d+) has (?<pos>\d+) positions; at time=0, it is at position (?<curPos>\d+).";
        foreach (var disc in arrangement)
        {
            var match = Regex.Match(disc, pattern);
            dict[match.Groups["disc"].Value] = 
                (int.Parse(match.Groups["pos"].Value), int.Parse(match.Groups["curPos"].Value));
        }

        var time = 0;
        while (dict.Select(r => GetPosition(int.Parse(r.Key) + time, r.Value)).Any(r => r != 0))
            time++;

        return time;
    }

    private static int GetPosition(int ticks, (int max, int current) tuple)
    {
        var (max, current) = tuple;
        return (ticks + current) % max;
    }
}