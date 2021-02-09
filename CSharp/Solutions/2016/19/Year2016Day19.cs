using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._19
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day19 : ISolution
    {
        public object Part1(IEnumerable<string> input)
        {
            var prev = true;
            var a = Enumerable.Range(1, int.Parse(input.First())).ToList();
            var last = a.Last();
            while (a.Count > 1) 
            {
                a = a.Where((_, i) => (i + 1) % 2 != (prev ? 0 : 1)).ToList();
                prev = last != a.Last();
                last = a.Last();
            }

            return a.First().ToString();
        }

        public object Part2(IEnumerable<string> input)
        {
            var n = int.Parse(input.First());
            var elf = 1;
            for (var i = 1; i < n; i++)
            {
                elf = elf % i + 1;
                if (elf > (i + 1) / 2)
                {
                    elf++;
                }
            }

            return elf.ToString();
        }
    }
}