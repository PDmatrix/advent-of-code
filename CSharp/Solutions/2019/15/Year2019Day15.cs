using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2019._15;

[UsedImplicitly]
public class Year2019Day15 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var intcodeComputer = ConstructIntcodeComputer(input);

        var startPosition = new Position(0, 0);
        var queue = new Queue<(Position position, int steps, IntcodeComputer computer)>();
        var visited = new HashSet<Position>();
        queue.Enqueue((startPosition, 0, intcodeComputer));
        var directions = new List<long> { 1, 2, 3, 4 };
        while (true)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current.position))
                continue;

            visited.Add(current.position);
            var steps = current.steps + 1;
            foreach (var direction in directions)
            {
                var newPos = GetNewPosition(direction, current.position);
                var newComputer = current.computer.Clone();
                var output = newComputer.Run(direction);
                switch (output)
                {
                    case 2:
                        return steps;
                    case 0:
                        visited.Add(newPos);
                        break;
                    default:
                        queue.Enqueue((newPos, steps, newComputer));
                        break;
                }
            }
        }
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
        var intcodeComputer = ConstructIntcodeComputer(input);

        var startPosition = new Position(0, 0);
        var queue = new Queue<(Position position, int steps, IntcodeComputer computer)>();
        var visited = new HashSet<Position>();
        queue.Enqueue((startPosition, 0, intcodeComputer));
        var directions = new List<long> { 1, 2, 3, 4 };
        var grid = new Dictionary<Position, bool>();
        var oxygenPosition = new Position(-1, -1);
        while (queue.Count != 0)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current.position))
                continue;

            visited.Add(current.position);
            var steps = current.steps + 1;
            foreach (var direction in directions)
            {
                var newPos = GetNewPosition(direction, current.position);
                var newComputer = current.computer.Clone();
                var output = newComputer.Run(direction);
                switch (output)
                {
                    case 2:
                        oxygenPosition = newPos;
                        grid[newPos] = true;
                        break;
                    case 0:
                        visited.Add(newPos);
                        grid[newPos] = false;
                        break;
                    default:
                        queue.Enqueue((newPos, steps, newComputer));
                        grid[newPos] = true;
                        break;
                }
            }
        }
        
        var maxMinutes = GetMaxMinutes(oxygenPosition, grid, directions);

        return maxMinutes;
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

    private static int GetMaxMinutes(Position oxygenPosition, Dictionary<Position, bool> grid, List<long> directions)
    {
        var maxMinutes = 0;
        var queue = new Queue<(Position position, int minutes)>();
        queue.Enqueue((oxygenPosition, 0));
        var visited = new HashSet<Position>();
        while (queue.Count != 0)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current.position))
                continue;

            visited.Add(current.position);
            if (!grid.ContainsKey(current.position) || !grid[current.position])
                continue;

            maxMinutes = Math.Max(maxMinutes, current.minutes);
            foreach (var newPostion in directions.Select(direction => GetNewPosition(direction, current.position)))
            {
                queue.Enqueue((newPostion, current.minutes + 1));
            }
        }

        return maxMinutes;
    }

    private static Position GetNewPosition(long direction, Position currentPosition)
    {
        return direction switch
        {
            1 => new Position(currentPosition.X, currentPosition.Y - 1),
            2 => new Position(currentPosition.X, currentPosition.Y + 1),
            3 => new Position(currentPosition.X - 1, currentPosition.Y),
            4 => new Position(currentPosition.X + 1, currentPosition.Y),
            _ => throw new Exception("Unexpected direction")
        };
    }
}