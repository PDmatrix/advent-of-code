using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._5;

[UsedImplicitly]
public class Year2019Day05 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var a = input.First().Split(',').Select(int.Parse).ToArray();

        return RunProgram(a, 1);
    }
	
    private static IEnumerable<int> DigitArr2(int n)
    {
        var result = new int[5];
        for (var i = result.Length - 1; i >= 0; i--)
        {
            result[i] = n % 10;
            n /= 10;
        }

        return result;
    }

    private static int GetValue(IList<int> a, int i, int mode)
    {
        return (mode == 0 ? a[a[i]] : a[i]);
    }

    private static int RunProgram(IList<int> a, int input)
    {
        for (var i = 0; i < a.Count;)
        {
            if (a[i] == 99)
                break;

            var instruction = DigitArr2(a[i]).ToArray();

            switch (instruction[4])
            {
                case 1:
                    a[a[i + 3]] = GetValue(a, i + 1, instruction[2]) +
                                  GetValue(a, i + 2, instruction[1]);
                    i += 4;
                    break;
                case 2:
                    a[a[i + 3]] = GetValue(a, i + 1, instruction[2]) *
                                  GetValue(a, i + 2, instruction[1]);
                    i += 4;
                    break;
                case 3:
                    a[a[i + 1]] = input;
                    i += 2;
                    break;
                case 4:
                    Console.WriteLine(GetValue(a, i + 1, instruction[2]));
                    i += 2;
                    break;
                case 5:
                    i = GetValue(a, i + 1, instruction[2]) != 0
                        ? GetValue(a, i + 2, instruction[1])
                        : i + 3;
                    break;
                case 6:
                    i = GetValue(a, i + 1, instruction[2]) == 0
                        ? GetValue(a, i + 2, instruction[1])
                        : i + 3;
                    break;
                case 7:
                    a[a[i + 3]] = GetValue(a, i + 1, instruction[2]) < GetValue(a, i + 2, instruction[1])
                        ? 1
                        : 0;
                    i += 4;
                    break;
                case 8:
                    a[a[i + 3]] = GetValue(a, i + 1, instruction[2]) == GetValue(a, i + 2, instruction[1])
                        ? 1
                        : 0;
                    i += 4;
                    break;
            }
        }

        return a[0];
    }

    public object Part2(IEnumerable<string> input)
    {
        var a = input.First().Split(',').Select(int.Parse).ToArray();

        return RunProgram(a, 5);
    }
}