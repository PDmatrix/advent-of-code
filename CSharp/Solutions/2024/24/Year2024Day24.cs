using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._24;

[UsedImplicitly]
public class Year2024Day24 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var (gates, registers) = ParseInput(input);
        return Run(gates, registers);
    }

    public object Part2(IEnumerable<string> input)
    {
        var (gates, registers) = ParseInput(input);

        var nxz = gates.Where(g => g.C[0] == 'z' && g.C != "z45" && g.Op != Operation.XOR).ToList();
        var xnz = gates.Where(g => g.A[0] != 'x' && g.A[0] != 'y' &&
                                   g.B[0] != 'x' && g.B[0] != 'y' &&
                                   g.C[0] != 'z' && g.Op == Operation.XOR).ToList();

        foreach (var i in xnz)
        {
            var b = nxz.First(n => n.C == FirstZThatUsesC(gates, i.C));
            (i.C, b.C) = (b.C, i.C);
        }

        var falseCarry = BitOperations
            .TrailingZeroCount(
                GetWiresAsLong(registers, 'x') + GetWiresAsLong(registers, 'y') ^ Run(gates, registers))
            .ToString();

        return string.Join(",", nxz.Concat(xnz)
            .Concat(gates.Where(g => g.A.EndsWith(falseCarry) && g.B.EndsWith(falseCarry)))
            .Select(g => g.C)
            .OrderBy(c => c));
    }

    private enum Operation
    {
        AND,
        OR,
        XOR
    }

    private class Gate
    {
        public string A { get; }
        public string B { get; }
        public Operation Op { get; }
        public string C { get; set; }

        public Gate(string a, string b, Operation op, string c)
        {
            A = a;
            B = b;
            Op = op;
            C = c;
        }
    }

    private static string FirstZThatUsesC(List<Gate> gates, string c)
    {
        var x = gates.Where(g => g.A == c || g.B == c).ToList();
        var match = x.FirstOrDefault(g => g.C.StartsWith('z'));
        if (match != null)
        {
            return "z" + (int.Parse(match.C[1..]) - 1).ToString("D2");
        }

        return x.Select(g => FirstZThatUsesC(gates, g.C)).FirstOrDefault(res => res != null);
    }

    private static long Run(List<Gate> gates, Dictionary<string, int> registers)
    {
        var exclude = new HashSet<Gate>();

        while (exclude.Count != gates.Count)
        {
            var available = gates.Where(a => !exclude.Contains(a) &&
                                             gates.All(b => !(a.A == b.C || a.B == b.C) || exclude.Contains(b)))
                .ToList();

            foreach (var gate in available)
            {
                var v1 = registers.GetValueOrDefault(gate.A, 0);
                var v2 = registers.GetValueOrDefault(gate.B, 0);
                registers[gate.C] = gate.Op switch
                {
                    Operation.AND => v1 & v2,
                    Operation.OR => v1 | v2,
                    Operation.XOR => v1 ^ v2,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            exclude.UnionWith(available);
        }

        return GetWiresAsLong(registers, 'z');
    }

    private static long GetWiresAsLong(Dictionary<string, int> registers, char type)
    {
        return Convert.ToInt64(
            string.Join("", registers
                .Where(r => r.Key.StartsWith(type))
                .OrderBy(r => r.Key)
                .Select(r => r.Value)
                .Select(v => v.ToString())
                .Reverse()
                .ToArray()),
            2);
    }

    private static (List<Gate>, Dictionary<string, int>) ParseInput(IEnumerable<string> input)
    {
        var wires = new Dictionary<string, int>();
        var instructions = new List<Gate>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            var split = line.Split(" ");
            if (split.Length == 2)
            {
                wires[split[0].TrimEnd(':')] = int.Parse(split[1]);
                continue;
            }

            instructions.Add(new Gate(split[0], split[2], Enum.Parse<Operation>(split[1]), split[4]));
        }

        return (instructions, wires);
    }
}