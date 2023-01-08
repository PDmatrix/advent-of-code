using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2019._19;

[UsedImplicitly]
public class Year2019Day19 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var intcodeComputer = ConstructIntcodeComputer(input);
        var grid = new Dictionary<Position, long>();
        const int maxX = 49;
        const int maxY = 49;
        for (var y = 0; y <= maxY; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                intcodeComputer.Reset();
                var ot = intcodeComputer.Run(x);
                var isPulled = intcodeComputer.Run(y);
                grid.Add(new Position(x, y), isPulled);
            }
        }

        return grid.Count(x => x.Value == 1);
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

    private static bool Check(IntcodeComputer computer, Position position)
    {
        if (position.X < 0 || position.Y < 0)
            return false;
        
        computer.Reset();
        computer.Run(position.X);
        var output = computer.Run(position.Y);
        
        return output == 1;
    }

    private int GetFirst(IntcodeComputer computer, int y)
    {
        for (int i = 0; i < 99999; i++)
        {
            if (Check(computer, new Position(i, y)))
                return i;
        }

        return -1;
    }
    
    public object Part2(IEnumerable<string> input)
    {
        var intcodeComputer = ConstructIntcodeComputer(input);
        const int shipWide = 99;
        const int shipHeight = 99;
        for (int y = 100; y < 99999; y++)
        {
            var x = GetFirst(intcodeComputer, y);
            if (x == -1)
                continue;

            var checks = new List<bool>();
            checks.Add(Check(intcodeComputer, new Position(x, y - shipHeight)));
            checks.Add(Check(intcodeComputer, new Position(x + shipWide, y - shipHeight)));
            checks.Add(Check(intcodeComputer, new Position(x + shipWide, y)));
            if (checks.TrueForAll(check => check))
                return x * 10000 + (y - shipHeight);
        }

        return -1;
    }
}