using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._12
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day12 : ISolution
    {
        public string Part1(IEnumerable<string> input)
        {
            var registers = new Dictionary<string, int>
            {
                ["a"] = 0,
                ["b"] = 0,
                ["c"] = 0,
                ["d"] = 0
            };
            return ComputeValue(input, registers);
        }

        

        

        public string Part2(IEnumerable<string> input)
        {
            var registers = new Dictionary<string, int>
            {
                ["a"] = 0,
                ["b"] = 0,
                ["c"] = 1,
                ["d"] = 0
            };
            return ComputeValue(input, registers);
        }
        
        private static int ParseValue(string regOrValue, IReadOnlyDictionary<string, int> registers)
        {
            return int.TryParse(regOrValue, out var res) ? res : registers[regOrValue];
        }
        
        private static string ComputeValue(IEnumerable<string> input, Dictionary<string, int> registers)
        {
            var idx = 0;
            var commands = input as string[] ?? input.ToArray();
            const string commandPattern =
                @"(?<command>cpy|inc|dec|jnz) (?<first>[\d|\w]+) ?(?<second>.*)";
            while (idx < commands.Length)
            {
                var match = Regex.Match(commands[idx], commandPattern);
                var command = match.Groups["command"].Value;
                switch (command)
                {
                    case "cpy":
                        registers[match.Groups["second"].Value] =
                            ParseValue(match.Groups["first"].Value, registers);
                        break;

                    case "inc":
                        registers[match.Groups["first"].Value] += 1;
                        break;

                    case "dec":
                        registers[match.Groups["first"].Value] -= 1;
                        break;

                    case "jnz":
                        if (ParseValue(match.Groups["first"].Value, registers) != 0)
                        {
                            idx += ParseValue(match.Groups["second"].Value, registers);
                            continue;
                        }

                        break;

                    default:
                        throw new Exception("Invalid input!");
                }

                idx += 1;
            }

            return registers["a"].ToString();
        }
    }
}