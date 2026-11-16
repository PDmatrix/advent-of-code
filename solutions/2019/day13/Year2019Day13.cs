using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2019._13;

[UsedImplicitly]
public class Year2019Day13 : ISolution
{
    public object Part1(IEnumerable<string> input)
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

        var grid = new Dictionary<Point, long>();
        while (!intcodeComputer.IsHalted())
        {
            var x = intcodeComputer.Run(0);
            var y = intcodeComputer.Run(0);
            var tile = intcodeComputer.Run(0);
            grid.Add(new Point((int)x, (int)y), tile);
        }
        return grid.Count(x => x.Value == 2);
    }
    private class IntcodeComputer
    {
        public IntcodeState InternalState { get; set; }
        
        public IntcodeComputer(IntcodeState state)
        {
            InternalState = state;
        }

        public long Run(long input)
        {
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
                        InternalState.Memory[GetValue(InternalState.Index + 1, instruction[2], 'w')] = input;
                        InternalState.Index += 2;
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

            return -2;
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
        var program = input.First().Split(',').Select(long.Parse).ToArray();
        long index = -1;
        var programMemory = program.ToDictionary(_ => ++index, x => x);
        programMemory[0] = 2;
        var intcodeComputer = new IntcodeComputer(new IntcodeState
        {
            BaseOffset = 0,
            Index = 0,
            Memory = programMemory
        });

        var grid = new Dictionary<Point, long>();
        long score = 0;
        while (!intcodeComputer.IsHalted())
        {
            var x = intcodeComputer.Run(0);
            if (x == -1)
            {
                intcodeComputer.Run(0);
                score = intcodeComputer.Run(0);
                continue;
            }
            if (x == -2)
                break;
            var y = intcodeComputer.Run(0);
            var tile = intcodeComputer.Run(0);
            var point = new Point((int)x, (int)y);
            if (grid.ContainsKey(point))
            {
                grid[point] = tile;
                break;
            }

            grid.Add(new Point((int)x, (int)y), tile);
        }

        var ballPoint = new Point(0, 0);
        var paddlePoint = new Point(0, 0);
        while (!intcodeComputer.IsHalted())
        {
            var x = intcodeComputer.Run(0);
            if (x == -1)
            {
                intcodeComputer.Run(0);
                score = intcodeComputer.Run(0);
                continue;
            }
            if (x == -2)
                break;
            var y = intcodeComputer.Run(0);
            var tile = intcodeComputer.Run(0);
            var point = new Point((int)x, (int)y);
            switch (tile)
            {
                case 4:
                    ballPoint = point;
                    break;
                case 3:
                    paddlePoint = point;
                    break;
            }

            if (grid.ContainsKey(point))
            {
                grid[point] = tile;
                break;
            }

            grid.Add(point, tile);
        }

        while (!intcodeComputer.IsHalted())
        {
            var needToPrint = false;
            var joystick = 0;
            if (paddlePoint.X > ballPoint.X)
                joystick = -1;
            if (paddlePoint.X < ballPoint.X)
                joystick = 1;
            var x = intcodeComputer.Run(joystick);
            if (x == -1)
            {
                intcodeComputer.Run(joystick);
                score = intcodeComputer.Run(joystick);
                needToPrint = true;
                continue;
            }

            if (x == -2)
                break;
            var y = intcodeComputer.Run(joystick);
            var tile = intcodeComputer.Run(joystick);
            var point = new Point((int)x, (int)y);
            switch (tile)
            {
                case 4:
                    ballPoint = point;
                    needToPrint = true;
                    break;
                case 3:
                    paddlePoint = point;
                    break;
            }

            grid[point] = tile;
            
            /*
            // uncomment if you want to see the game in action :)
            if (needToPrint)
            {
                ShowScreen(grid, score);
                Thread.Sleep(24);
                Console.Clear();
            }
        */
        }

        return score;
    }

    private static void ShowScreen(Dictionary<Point, long> grid, long score)
    {
        var minX = 0; //grid.Min(x => x.Key.X);
        var maxX = 39;//grid.Max(x => x.Key.X);
        var minY = 0;//grid.Min(x => x.Key.Y);
        var maxY = 19;//grid.Max(x => x.Key.Y);
        var sb = new StringBuilder();
        sb.AppendLine();
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                var tile = grid.GetValueOrDefault(new Point(x, y), 0);
                var tileStr = tile switch
                {
                    0 => " ",
                    1 => "█",
                    2 => ".",
                    3 => "_",
                    4 => "o",
                    _ => throw new Exception("Invalid tile")
                };
                sb.Append(tileStr);
            }

            sb.AppendLine();
        }

        sb.AppendLine(string.Concat(Enumerable.Repeat("-", 17)) + score.ToString("D6") + string.Concat(Enumerable.Repeat("-", 17)));

        Console.Write(sb.ToString());
    }
}