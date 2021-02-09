using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._23
{
    // ReSharper disable once UnusedMember.Global
    public class Year2017Day23 : ISolution
    {
        public object Part1(IEnumerable<string> input)
        {
            var regex = new Regex(@"(?<op>set|mul|sub|jnz) (?<f>.+?) (?<s>.+)", RegexOptions.Compiled);
            
            var registers = new Dictionary<char, int>
            {
                ['a'] = 0,
                ['b'] = 0,
                ['c'] = 0,
                ['d'] = 0,
                ['e'] = 0,
                ['f'] = 0,
                ['g'] = 0,
                ['h'] = 0
            };

            var instructions = input.ToArray();
            
            var currentInstruction = 0;
            var instructionCount = instructions.Length;
            var mulCount = 0;
            while (currentInstruction < instructionCount)
            {
                var groups = regex.Match(instructions[currentInstruction]).Groups;

                var instruction = groups["op"].Value;
                var f = groups["f"].Value;
                var s = groups["s"].Value;
                
                switch (instruction)
                {
                    case "set":
                        registers[f.GetPinnableReference()] = GetValue(registers, s);
                        break;
                    case "sub":
                        registers[f.GetPinnableReference()] -= GetValue(registers, s);
                        break;
                    case "mul":
                        mulCount++;
                        registers[f.GetPinnableReference()] *= GetValue(registers, s);
                        break;
                    case "jnz":
                        if (GetValue(registers, f) != 0)
                        {
                            currentInstruction += GetValue(registers, s);
                            continue;
                        }
                        break;
                }
                
                currentInstruction += 1;
            }
            
            return mulCount.ToString();
        }

        public object Part2(IEnumerable<string> input)
        {
            var result = 0;
            // register b = 79
            for (var i = 107900; i <= 124900; i += 17)
            {
                if (!IsPrime(i))
                    result++;
            }

            return result.ToString();
        }

        private static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int) Math.Floor(Math.Sqrt(number));

            for (var i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

        private static int GetValue(IDictionary<char, int> registers, string value)
            => int.TryParse(value, out var result) 
                ? result 
                : registers[value.GetPinnableReference()];
    }
}