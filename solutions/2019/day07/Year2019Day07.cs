using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._7;

[UsedImplicitly]
public class Year2019Day07 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var program = input.First().Split(',').Select(int.Parse).ToArray();
        var list = new List<int> { 0, 1, 2, 3, 4 };
        var permutations = GetPermutations(list, list.Count);
        var maxOutput = -1;
        foreach (var permutation in permutations)
        {
            var enumerable = permutation as int[] ?? permutation.ToArray();
            var amplifiers = new List<(int[] state, int pos, int[] input)>
            {
                ((int[])program.Clone(), 0, new[] { enumerable[0], 0 }),
            };

            for (var i = 0; i < list.Count - 1; i++)
            {
                var (state, pos, ot) = RunProgram(amplifiers[i].state, amplifiers[i].input, amplifiers[i].pos);
                amplifiers[i] = (state, pos, amplifiers[i].input);
                amplifiers.Add(((int[])program.Clone(), 0, new[] { enumerable[i + 1], ot }));
            }

            var (_, _, output) = RunProgram(amplifiers.Last().state, amplifiers.Last().input, amplifiers.Last().pos);

            maxOutput = Math.Max(maxOutput, output);
        }

        return maxOutput;
    }

    private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new[] { t });

        var enumerable = list as T[] ?? list.ToArray();
        return GetPermutations(enumerable, length - 1)
            .SelectMany(t => enumerable.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new[] { t2 }));
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

    private static (int[] state, int pos, int output) RunProgram(int[] a, int[] input, int pos = 0)
    {
        var output = -1;
        var inputIndex = 0;
        for (var i = pos; i < a.Length;)
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
                    a[a[i + 1]] = input[inputIndex];
                    inputIndex++;
                    i += 2;
                    break;
                case 4:
                    //Console.WriteLine(GetValue(a, i + 1, instruction[2]));
                    output = GetValue(a, i + 1, instruction[2]);
                    i += 2;
                    return (a, i, output);
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

        return (a, -1, output);
    }


    public object Part2(IEnumerable<string> input)
    {
        var program = input.First().Split(',').Select(int.Parse).ToArray();
        var list = new List<int> { 5, 6, 7, 8, 9 };
        var permutations = GetPermutations(list, list.Count);
        var maxOutput = -1;
        foreach (var permutation in permutations)
        {
            var enumerable = permutation as int[] ?? permutation.ToArray();
            var amplifiers = new List<(int[] state, int pos, int[] input)>
            {
                ((int[])program.Clone(), 0, new[] { enumerable[0], 0 }),
            };
            for (var i = 0; i < list.Count - 1; i++)
            {
                var (state, pos, output) = RunProgram(amplifiers[i].state, amplifiers[i].input, amplifiers[i].pos);
                amplifiers[i] = (state, pos, amplifiers[i].input);
                amplifiers.Add(((int[])program.Clone(), 0, new[] { enumerable[i + 1], output }));
            }

            var index = 4;
            var haltCount = 0;
            while (haltCount < 5)
            {
                var trueIndex = index % 5;
                var (state, pos, output) = RunProgram(amplifiers[trueIndex].state, amplifiers[trueIndex].input,
                    amplifiers[trueIndex].pos);
                amplifiers[trueIndex] = (state, pos, amplifiers[trueIndex].input);
                var nextIndex = (index + 1) % 5;
                amplifiers[nextIndex] = (amplifiers[nextIndex].state, amplifiers[nextIndex].pos, new[] { output });
                maxOutput = Math.Max(maxOutput, output);
                if (pos == -1)
                    haltCount++;
                index++;
            }
        }

        return maxOutput;
    }
}
/*
3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5

get input, put in 26 pos
get 26 pos - 4, put in 26 pos
get input, put in 27 pos
get 27 pos * 2, put in 27 pos
get 27 pos + 26 pos, put in 27 pos
output 27 pos
get 28 pos - 1, put in 28 pos
if 28 pos != 0, i = 6

pos[26] = input;
pos[26] = pos[26] - 4
start:
pos[27] = input;
pos[27] = pos[27] * 2
pos[27] = pos[26] + pos[27]
return pos[27]
pos[28] = pos[28] - 1
if (pos[28] != 0) goto start;
*/