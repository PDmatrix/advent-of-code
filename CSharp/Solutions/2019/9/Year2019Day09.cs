using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._9;

[UsedImplicitly]
public class Year2019Day09 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        //input = new[] { "3,9,4,9,4,10,99" };
        var program = input.First().Split(',').Select(long.Parse).ToArray();
        long index = -1;
        var programDict = program.ToDictionary(x =>
        {
            index++;
            return index;
        }, x => x);
        
        var a = RunProgram(programDict, 1);
        return 1;
    }
    
    private static IEnumerable<long> DigitArr2(long n)
    {
        var result = new long[5];
        for (var i = result.Length - 1; i >= 0; i--)
        {
            result[i] = n % 10;
            n /= 10;
        }

        return result;
    }

    private static long GetValue(Dictionary<long, long> state, long i, long mode, long baseOffset, char type = 'r')
    {
        if (type == 'w')
        {
            return mode switch
            {
                0 => state.GetValueOrDefault(i, 0),
                1 => state.GetValueOrDefault(i, 0),
                2 => state.GetValueOrDefault(i, 0) + baseOffset,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }
        
        return mode switch
        {
            0 => state.GetValueOrDefault(state.GetValueOrDefault(i, 0), 0),
            1 => state.GetValueOrDefault(i, 0),
            2 => state.GetValueOrDefault(state.GetValueOrDefault(i, 0) + baseOffset, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    private static long RunProgram(Dictionary<long, long> state, long input)
    {
        long index = 0;
        long baseOffset = 0;
        while (state.GetValueOrDefault(index, 0) != 99)
        {
            var instruction = DigitArr2(state.GetValueOrDefault(index, 0)).ToArray();

            switch (instruction[4])
            {
                case 1:
                    state[GetValue(state, index + 3, instruction[0], baseOffset, 'w')] = GetValue(state, index + 1, instruction[2], baseOffset) +
                                                                             GetValue(state, index + 2, instruction[1], baseOffset);
                    index += 4;
                    break;
                case 2:
                    state[GetValue(state, index + 3, instruction[0], baseOffset, 'w')] = GetValue(state, index + 1, instruction[2], baseOffset) *
                                                                   GetValue(state, index + 2, instruction[1], baseOffset);
                    index += 4;
                    break;
                case 3:
                    state[GetValue(state, index + 1, instruction[2], baseOffset, 'w')] = input;
                    index += 2;
                    break;
                case 4:
                    Console.WriteLine(GetValue(state, index + 1, instruction[2], baseOffset));
                    index += 2;
                    break;
                case 5:
                    index = GetValue(state, index + 1, instruction[2], baseOffset) != 0
                        ? GetValue(state, index + 2, instruction[1], baseOffset)
                        : index + 3;
                    break;
                case 6:
                    index = GetValue(state, index + 1, instruction[2], baseOffset) == 0
                        ? GetValue(state, index + 2, instruction[1], baseOffset)
                        : index + 3;
                    break;
                case 7:
                    state[GetValue(state, index + 3, instruction[0], baseOffset, 'w')] = GetValue(state, index + 1, instruction[2], baseOffset) <
                                                                   GetValue(state, index + 2, instruction[1], baseOffset)
                        ? 1
                        : 0;
                    index += 4;
                    break;
                case 8:
                    state[GetValue(state, index + 3, instruction[0], baseOffset, 'w')] = GetValue(state, index + 1, instruction[2], baseOffset) ==
                                                                   GetValue(state, index + 2, instruction[1], baseOffset)
                        ? 1
                        : 0;
                    index += 4;
                    break;
                case 9:
                    baseOffset += GetValue(state, index + 1, instruction[2], baseOffset);
                    index += 2;
                    break;
            }
        }

        return state[0];
    }


    public object Part2(IEnumerable<string> input)
    {
        var program = input.First().Split(',').Select(long.Parse).ToArray();
        long index = -1;
        var programDict = program.ToDictionary(x =>
        {
            index++;
            return index;
        }, x => x);
        
        var a = RunProgram(programDict, 2);
        return 2;
    }
}