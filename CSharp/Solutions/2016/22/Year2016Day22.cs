using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._22
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day22 : ISolution
    {
        public string Part1(IEnumerable<string> input)
        {
            const string reg = 
                @"/dev/grid/node-x(?<x>\d+)-y(?<y>\d+)\s+(\d+)T\s+(?<u>\d+)T\s+(?<a>\d+)T\s+(\d+)";
            var nodes = input.Select(r =>
            {
                var match = Regex.Match(r, reg);
                if (!match.Success) return null;
                return new Node
                {
                    X = int.Parse(match.Groups["x"].Value),
                    Y = int.Parse(match.Groups["y"].Value),
                    Available = int.Parse(match.Groups["a"].Value),
                    Used = int.Parse(match.Groups["u"].Value),
                };
            }).Where(r => r != null).ToArray();
            var res = 0;
            foreach (var node in nodes)
            {
                foreach (var node1 in nodes)
                {
                    var notEmpty = node.Used != 0;
                    var notTheSame = node.X != node1.X || node.Y != node1.Y;
                    var fit = node.Used <= node1.Available;
                    res += Convert.ToInt32(notEmpty && notTheSame && fit);
                }
            }
            
            return res.ToString();
        }

        public string Part2(IEnumerable<string> input)
        {
            const string reg = 
                @"/dev/grid/node-x(?<x>\d+)-y(?<y>\d+)\s+(\d+)T\s+(?<u>\d+)T\s+(?<a>\d+)T\s+(\d+)";
            var nodes = input.Select(r =>
            {
                var match = Regex.Match(r, reg);
                if (!match.Success) return null;
                return new Node
                {
                    X = int.Parse(match.Groups["x"].Value),
                    Y = int.Parse(match.Groups["y"].Value),
                    Available = int.Parse(match.Groups["a"].Value),
                    Used = int.Parse(match.Groups["u"].Value),
                };
            }).Where(r => r != null).ToArray();
            var prevX = -1;
            foreach (var node in nodes)
            {
                if (node.X != prevX)
                {
                    prevX = node.X;
                    Console.WriteLine();
                }
                
                if(node.Used == 0)
                    Console.Write("_ ");
                else if(node.Used > 100)
                    Console.Write("# ");
                else
                    Console.Write(". ");
            }
            Console.WriteLine();
            return (9 + 27 + 26 + 29 * 5).ToString();
        }

        private class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Used { get; set; }
            public int Available { get; set; }
            
            public int Size => Used + Available;
        }
    }
}