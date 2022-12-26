using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2019._11;

[UsedImplicitly]
public class Year2019Day11 : ISolution
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

        var robot = new Point(500, 500);
        var grid = new Dictionary<Point, long>
        {
            [robot] = 0
        };

        var directions = new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left };
        var currentDirectionIndex = 0;
        while (!intcodeComputer.IsHalted())
        {
            var color = intcodeComputer.Run(grid.GetValueOrDefault(robot, 0));
            var direction = intcodeComputer.Run(grid.GetValueOrDefault(robot, 0));
            grid[robot] = color;
            currentDirectionIndex += direction == 0 ? -1 : 1;
            if (currentDirectionIndex >= directions.Length)
                currentDirectionIndex -= directions.Length;
            else if (currentDirectionIndex < 0)
                currentDirectionIndex += directions.Length;
            robot = GetNextPosition(robot, directions[currentDirectionIndex]);
        }

        return grid.Count;
    }
    
    private enum Direction : byte
    {
        Right,
        Down,
        Left,
        Up
    }
    
    private static Point GetNextPosition(Point current, Direction direction)
    {
        return direction switch
        {
            Direction.Down => current with { Y = current.Y + 1 },
            Direction.Left => current with { X = current.X - 1 },
            Direction.Right => current with { X = current.X + 1 },
            Direction.Up => current with { Y = current.Y - 1 },
            _ => throw new Exception()
        };
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
                        //Console.WriteLine(output);
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
        var program = input.First().Split(',').Select(long.Parse).ToArray();
        long index = -1;
        var programMemory = program.ToDictionary(_ => ++index, x => x);
        var intcodeComputer = new IntcodeComputer(new IntcodeState
        {
            BaseOffset = 0,
            Index = 0,
            Memory = programMemory
        });

        var robot = new Point(500, 500);
        var grid = new Dictionary<Point, long>
        {
            [robot] = 1
        };

        var directions = new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left };
        var currentDirectionIndex = 0;
        while (!intcodeComputer.IsHalted())
        {
            var color = intcodeComputer.Run(grid.GetValueOrDefault(robot, 0));
            if (color == -1)
                break;
            var direction = intcodeComputer.Run(grid.GetValueOrDefault(robot, 0));
            grid[robot] = color;
            currentDirectionIndex += direction == 0 ? -1 : 1;
            if (currentDirectionIndex >= directions.Length)
                currentDirectionIndex -= directions.Length;
            else if (currentDirectionIndex < 0)
                currentDirectionIndex += directions.Length;
            robot = GetNextPosition(robot, directions[currentDirectionIndex]);
        }

        var minX = grid.Min(x => x.Key.X);
        var maxX = grid.Max(x => x.Key.X);
        var minY = grid.Min(x => x.Key.Y);
        var maxY = grid.Max(x => x.Key.Y);

        var sb = new StringBuilder();
        sb.AppendLine();
        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var color = grid.GetValueOrDefault(new Point(x, y), 0);
                sb.Append(color == 0 ? " " : "#");
            }

            sb.AppendLine();
        }
        return sb.ToString();
    }
}