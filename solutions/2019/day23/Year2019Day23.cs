using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._23;

[UsedImplicitly]
public class Year2019Day23 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var computers = Enumerable.Range(0, 50).Select(x => ConstructIntcodeComputer(input)).ToList();
        for (var i = 0; i < computers.Count; i++)
        {
            computers[i].Run(i);
        }
        var queue = new Queue<(long computer, long x, long y)>();
        while (true)
        {
            if (queue.Count > 0)
            {
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    if (current.computer == 255)
                        return current.y;
                    
                    computers[(int)current.computer].Run(current.x);
                    var output = computers[(int)current.computer].Run(current.y);
                    if (output == -99) 
                        continue;
                    
                    var x = computers[(int)current.computer].Run(-1);
                    var y = computers[(int)current.computer].Run(-1);
                    queue.Enqueue((output, x, y));
                }
            }
            else
            {
                foreach (var computer in computers)
                {
                    var output = computer.Run(-1);
                    if (output == -99) 
                        continue;
                    
                    var x = computer.Run(-1);
                    var y = computer.Run(-1);
                    queue.Enqueue((output, x, y));
                }
            }
        }
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
        
        public long Run(long input = -2)
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
                        if (input == -2 || inputCount > 0)
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
        var computers = Enumerable.Range(0, 50).Select(x => ConstructIntcodeComputer(input)).ToList();
        for (var i = 0; i < computers.Count; i++)
        {
            computers[i].Run(i);
        }
        var queue = new Queue<(long computer, long x, long y)>();
        (long computer, long x, long y) nat = (0, 0, 0);
        long lastNatY = 0;
        while (true)
        {
            var outputs = new List<long>(50);

            if (queue.Count > 0)
            {
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    if (current.computer == 255)
                    {
                        nat = (0, current.x, current.y);
                        continue;
                    }
                    computers[(int)current.computer].Run(current.x);
                    var output = computers[(int)current.computer].Run(current.y);
                    outputs.Add(output);
                    if (output == -99) 
                        continue;
                    
                    var x = computers[(int)current.computer].Run(-1);
                    var y = computers[(int)current.computer].Run(-1);
                    queue.Enqueue((output, x, y));
                }
            }
            else
            {
                foreach (var computer in computers)
                {
                    var output = computer.Run(-1);
                    outputs.Add(output);
                    if (output == -99) 
                        continue;
                    var x = computer.Run(-1);
                    var y = computer.Run(-1);
                    queue.Enqueue((output, x, y));
                }
            }

            if (!outputs.TrueForAll(x => x == -99)) 
                continue;
            
            queue.Enqueue(nat);
            if (nat.y == lastNatY)
                return nat.y;
            
            lastNatY = nat.y;
        }
    }
}