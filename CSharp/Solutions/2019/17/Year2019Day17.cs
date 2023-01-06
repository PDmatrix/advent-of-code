using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2019._17;

[UsedImplicitly]
public class Year2019Day17 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var computer = ConstructIntcodeComputer(input);
        var outputs = new List<long>();
        while (!computer.IsHalted())
        {
            var output = computer.Run(0);
            if (output == -1)
                break;

            outputs.Add(output);
        }

        var grid = new Dictionary<Position, char>();
        var currentPosition = new Position(0, 0);
        foreach (var output in outputs)
        {

            var elem = (char)output;
            if (elem == '\n')
            {
                currentPosition = new Position(0, currentPosition.Y + 1);
                continue;
            }

            grid.Add(currentPosition, elem);
            currentPosition = new Position(currentPosition.X + 1, currentPosition.Y);
        }

        var intersectedPosition = new List<Position>();

        foreach (var pos in grid.Keys)
        {
            if (grid[pos] != '#')
                continue;

            var positions = new List<Position>
            {
                new(pos.X, pos.Y + 1),
                new(pos.X, pos.Y - 1),
                new(pos.X + 1, pos.Y),
                new(pos.X - 1, pos.Y),
            };
            var found = true;
            foreach (var requiredPosition in positions)
            {
                if (!grid.ContainsKey(requiredPosition))
                {
                    found = false;
                    break;
                }

                if (grid[requiredPosition] != '#')
                {
                    found = false;
                    break;
                }
            }

            if (found)
                intersectedPosition.Add(pos);
        }


        return intersectedPosition.Select(x => x.X * x.Y).Sum();
    }

    private static void ShowScreen(Dictionary<Position, char> grid)
    {
        var minX = grid.Min(x => x.Key.X);
        var maxX = grid.Max(x => x.Key.X);
        var minY = grid.Min(x => x.Key.Y);
        var maxY = grid.Max(x => x.Key.Y);
        var sb = new StringBuilder();
        sb.AppendLine();
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                var tile = grid.GetValueOrDefault(new Position(x, y), '.');
                sb.Append(tile);
            }

            sb.AppendLine();
        }

        Console.Write(sb.ToString());
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

    private class IntcodeComputer
    {
        public IntcodeState InternalState { get; set; }

        public IntcodeComputer(IntcodeState state)
        {
            InternalState = state;
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

    public object Part2(IEnumerable<string> input)
    {
        var computer = ConstructIntcodeComputer(input);

        computer.InternalState.Memory[0] = 2;
        /*
        L,12 L,8 R,12 L,10 L,8 L,12 R,12 L,12 L,8 R,12 R,12 L,8 L,10 L,12 L,8, R,12 L,12
        L,8 R,12 R,12 L,8 L,10 L,10 L,8 L,12 R,12 R,12 L,8 L,10 L,10 L,8 L,12 R,12

        XYZ CYXZ XYZ ZYC XYZ XYZ ZYC CYXZ ZYC CYXZ 

        A B A C A A C B C B

        A = L,12 L,8 R,12
        B = L,10 L,8 L,12 R,12
        C = R,12 L,8 L,10
        */
        var inputs = "A,B,A,C,A,A,C,B,C,B\n|L,12,L,8,R,12\n|L,10,L,8,L,12,R,12\n|R,12,L,8,L,10\n|n\n".Split('|');
        foreach (var inputValue in inputs)
        {
            InputValues(computer, inputValue.Select(x => (long)x));
        }

        while (!computer.IsHalted())
        {
            var output = computer.Run();
            if (output > 255)
                return output;
        }

        return 2;
    }

    private static void InputValues(IntcodeComputer computer, IEnumerable<long> inputs)
    {
        long output = 0;
        while (output != -99)
        {
            output = computer.Run();
            if (output == -99)
            {
                foreach (var i in inputs)
                {
                    computer.Run(i);
                }
            }
        }
    }
}