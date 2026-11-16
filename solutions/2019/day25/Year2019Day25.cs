using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._25;

[UsedImplicitly]
public class Year2019Day25 : ISolution
{
    /*
     * Manual game, pass command through console
     * Items required to pass checks on my input:
     * - astronaut ice cream
     * - dark matter
     * - weather machine
     * - easter egg
     * Map looks like that, where * is starting point, and . is pressure sensitive floor:
     * ### ###     ###         ###   ###
     * # > < >     < >         < >   <*#
     * ### #v#     #v#         #v#   ###
     *               
     * ### #^# ### #^# ### ### #^#   ###
     * # > < # # > < # # # #.# # >   < #
     * ### ### ### #v# #v# #v# #v#   #v#
     *
     *             #^# #^# #^# #^#   #^#
     *             # > < > < # # #   # #
     *             ### ### ### #v#   ###
     *
     *                         #^#
     *                         # #
     *                         ###
     */
    public object Part1(IEnumerable<string> input)
    {
        var computer = ConstructIntcodeComputer(input);
        var sb = new StringBuilder();
        
        while (!computer.IsHalted())
        {
            try
            {
                var output = computer.Run();
                if (output == -99)
                {
                    Console.WriteLine(sb);
                    sb.Clear();
                    var inp = Console.ReadLine().Select(x => (long)x);
                    foreach (var i in inp)
                        computer.Run(i);
                    sb.Append((char)computer.Run(10));
                }
                else
                    sb.Append((char)output);
            }
            catch (Exception e)
            {
                break;
            }
        }
        Console.WriteLine(sb);

        return 1;
    }

    private class IntcodeComputer
    {
        public IntcodeState InternalState { get; set; }
        private readonly Dictionary<long, long> _originalMemory;

        public IntcodeComputer(IntcodeState state)
        {
            InternalState = state;
            _originalMemory = state.Memory.ToDictionary(x => x.Key, x => x.Value);
        }

        public IntcodeComputer Clone()
        {
            return new IntcodeComputer(new IntcodeState
            {
                BaseOffset = InternalState.BaseOffset,
                Index = InternalState.Index,
                Memory = InternalState.Memory.ToDictionary(x => x.Key, x => x.Value)
            });
        }

        public void Reset()
        {
            InternalState.BaseOffset = 0;
            InternalState.Index = 0;
            InternalState.Memory = _originalMemory.ToDictionary(x => x.Key, x => x.Value);
        }

        public long Run(long input = -1)
        {
            var inputCount = 0;
            while (InternalState.Memory.GetValueOrDefault(InternalState.Index, 0) != 99)
            {
                var instruction = DigitArr2(InternalState.Memory.GetValueOrDefault(InternalState.Index, 0)).ToArray();

                switch (instruction[4])
                {
                    case 1:
                        InternalState.Memory[GetValue(InternalState.Index + 3, instruction[0], 'w')] =
                            GetValue(InternalState.Index + 1, instruction[2]) +
                            GetValue(InternalState.Index + 2, instruction[1]);
                        InternalState.Index += 4;
                        break;
                    case 2:
                        InternalState.Memory[GetValue(InternalState.Index + 3, instruction[0], 'w')] =
                            GetValue(InternalState.Index + 1, instruction[2]) *
                            GetValue(InternalState.Index + 2, instruction[1]);
                        InternalState.Index += 4;
                        break;
                    case 3:
                        if (input == -1 || inputCount > 0)
                            return -99;
                        InternalState.Memory[GetValue(InternalState.Index + 1, instruction[2], 'w')] = input;
                        InternalState.Index += 2;
                        inputCount++;
                        break;
                    case 4:
                        var output = GetValue(InternalState.Index + 1, instruction[2]);
                        InternalState.Index += 2;
                        return output;
                    case 5:
                        InternalState.Index = GetValue(InternalState.Index + 1, instruction[2]) != 0
                            ? GetValue(InternalState.Index + 2, instruction[1])
                            : InternalState.Index + 3;
                        break;
                    case 6:
                        InternalState.Index = GetValue(InternalState.Index + 1, instruction[2]) == 0
                            ? GetValue(InternalState.Index + 2, instruction[1])
                            : InternalState.Index + 3;
                        break;
                    case 7:
                        InternalState.Memory[GetValue(InternalState.Index + 3, instruction[0], 'w')] =
                            GetValue(InternalState.Index + 1, instruction[2]) <
                            GetValue(InternalState.Index + 2, instruction[1])
                                ? 1
                                : 0;
                        InternalState.Index += 4;
                        break;
                    case 8:
                        InternalState.Memory[GetValue(InternalState.Index + 3, instruction[0], 'w')] =
                            GetValue(InternalState.Index + 1, instruction[2]) ==
                            GetValue(InternalState.Index + 2, instruction[1])
                                ? 1
                                : 0;
                        InternalState.Index += 4;
                        break;
                    case 9:
                        InternalState.BaseOffset += GetValue(InternalState.Index + 1, instruction[2]);
                        InternalState.Index += 2;
                        break;
                }
            }

            return -1;
        }

        public bool IsHalted()
        {
            return InternalState.Memory[InternalState.Index] == 99;
        }

        private long GetValue(long index, long mode, char type = 'r')
        {
            if (type == 'w')
            {
                return mode switch
                {
                    0 => InternalState.Memory.GetValueOrDefault(index, 0),
                    1 => InternalState.Memory.GetValueOrDefault(index, 0),
                    2 => InternalState.Memory.GetValueOrDefault(index, 0) + InternalState.BaseOffset,
                    _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
                };
            }

            return mode switch
            {
                0 => InternalState.Memory.GetValueOrDefault(InternalState.Memory.GetValueOrDefault(index, 0), 0),
                1 => InternalState.Memory.GetValueOrDefault(index, 0),
                2 => InternalState.Memory.GetValueOrDefault(
                    InternalState.Memory.GetValueOrDefault(index, 0) + InternalState.BaseOffset, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }
    }

    private class IntcodeState
    {
        public long BaseOffset { get; set; }
        public long Index { get; set; }
        public Dictionary<long, long> Memory { get; set; }
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

    private static IntcodeComputer ConstructIntcodeComputer(IEnumerable<string> input)
    {
        var program = input.First().Split(',').Select(long.Parse).ToArray();
        long index = -1;
        var programMemory = program.ToDictionary(_ => ++index, x => x);
        var intcodeComputer = new IntcodeComputer(new IntcodeState
        {
            BaseOffset = 0,
            Index = 0,
            Memory = programMemory
        });
        return intcodeComputer;
    }

    public object Part2(IEnumerable<string> input)
    {
        return "Congratulations!";
    }
}