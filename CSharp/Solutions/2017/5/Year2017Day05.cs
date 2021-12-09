using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2017._5;

[UsedImplicitly]
public class Year2017Day05 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var jumpList = input.Select(int.Parse).ToArray();
        var idx = 0;
        var count = 0;
        while (idx < jumpList.Length)
        {
            var prevIdx = idx;
            idx += jumpList[idx];
            jumpList[prevIdx]++;
            count++;
        }
        return count.ToString();
    }

    public object Part2(IEnumerable<string> input)
    {
        var jumpList = input.Select(int.Parse).ToArray();
        var idx = 0;
        var count = 0;
        while (idx >= 0 && idx < jumpList.Length)
        {
            var prevIdx = idx;
            idx += jumpList[idx];
            jumpList[prevIdx] += jumpList[prevIdx] >= 3 ? -1 : 1;
            count++;
        }
        return count.ToString();
    }
}