using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2019._21;

[UsedImplicitly]
public class Year2019Day21 : ISolution
{
    /*
     * #####.#..########
     * #####.#..########
     */
    public object Part1(IEnumerable<string> input)
    {
        var intcodeComputer = ConstructIntcodeComputer(input);
        ExpectOutput(intcodeComputer, "Input instructions:\n");
        var instructions = "NOT A T\nOR T J\nNOT B T\nOR T J\nNOT C T\nOR T J\nAND D J\nWALK\n".Select(x => (long)x);
        foreach (var instruction in instructions)
        {
            intcodeComputer.Run(instruction);
        }
        
        ExpectOutput(intcodeComputer, "Walking...\n\n");
        return intcodeComputer.Run();
    }

    private void ExpectOutput(IntcodeComputer computer, string expected)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < expected.Length; i++)
        {
            var output = computer.Run();
            sb.Append((char)output);
        }

        if (sb.ToString() != expected)
            throw new Exception($"Expected '{expected}', but got '{sb}");
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
        var intcodeComputer = ConstructIntcodeComputer(input);
        ExpectOutput(intcodeComputer, "Input instructions:\n");
        var instructions = "NOT A T\nOR T J\nNOT B T\nOR T J\nNOT C T\nOR T J\nAND D J\nNOT H T\nNOT T T\nOR E T\nAND T J\nRUN\n".Select(x => (long)x);
        foreach (var instruction in instructions)
        {
            intcodeComputer.Run(instruction);
        }
        
        ExpectOutput(intcodeComputer, "Running...\n\n");
        return intcodeComputer.Run();
    }
}