using System;
using System.Collections.Generic;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._24;

[UsedImplicitly]
public class Year2021Day24 : ISolution
{
    private static readonly int[] Div = { 1, 1, 1, 1, 1, 26, 1, 26, 26, 1, 26, 26, 26, 26 };
    private static readonly int[] Check = { 12, 11, 13, 11, 14, -10, 11, -9, -3, 13, -5, -10, -4, -5 };
    private static readonly int[] Offset = { 4, 11, 5, 11, 14, 7, 11, 4, 6, 5, 9, 12, 14, 14 };
    private static readonly HashSet<(int depth, long z)> BadStates = new();

    public object Part1(IEnumerable<string> input)
    {
        return GenerateModelNumber(0, 0, 0, new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }) ??
               throw new InvalidOperationException();
    }


    private long? GenerateModelNumber(int depth, long modelNumber, long z, int[] digits)
    {
        if (BadStates.Contains((depth, z)) || depth == 14)
            return null;

        modelNumber *= 10;
        var originalZ = z;

        foreach (var t in digits)
        {
            z = originalZ;
            var w = t;
            var x = z;
            x %= 26;
            z /= Div[depth];
            x += Check[depth];
            x = x == w ? 1 : 0;
            x = x == 0 ? 1 : 0;
            long y = 25;
            y *= x;
            y += 1;
            z *= y;
            y = w;
            y += Offset[depth];
            y *= x;
            z += y;

            if (z == 0 && depth == 13)
                return modelNumber + t;

            var ret = GenerateModelNumber(depth + 1, modelNumber + t, z, digits);
            if (ret != null)
                return ret;
        }

        BadStates.Add((depth, originalZ));

        return null;
    }

    public object Part2(IEnumerable<string> input)
    {
        return GenerateModelNumber(0, 0, 0, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }) ??
               throw new InvalidOperationException();
    }
}