using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._25
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day25 : ISolution
    {
        public object Part1(IEnumerable<string> input)
        {
            var idx = 0;
            var computed = false;
            var enumerable = input as string[] ?? input.ToArray();
            while (!computed)
            {
                var registers = new Dictionary<string, int>
                {
                    ["a"] = idx,
                    ["b"] = 0,
                    ["c"] = 0,
                    ["d"] = 0
                };
                computed = ComputeValue(enumerable, registers);
                idx++;
            }

            return (idx - 1).ToString();
        }

        public object Part2(IEnumerable<string> input)
        {
            return "Congratulations!";
        }
        
        private static int ParseValue(string regOrValue, IReadOnlyDictionary<string, int> registers)
        {
            return int.TryParse(regOrValue, out var res) ? res : registers[regOrValue];
        }
        
        private static bool ComputeValue(IEnumerable<string> input, Dictionary<string, int> registers)
        {
            var idx = 0;
            var last = 1;
            var times = 0;
            var commands = input as string[] ?? input.ToArray();
            const string commandPattern =
                @"(?<command>cpy|inc|dec|jnz|out) (?<first>-?[\d|\w]+) ?(?<second>.*)";
            while (idx < commands.Length)
            {
                if (times >= 100000)
                    return true;
                var match = Regex.Match(commands[idx], commandPattern);
                var command = match.Groups["command"].Value;
                switch (command)
                {
                    case "out":
                        var parsed = ParseValue(match.Groups["first"].Value, registers);
                        if (last == parsed)
                            return false;
                        last = parsed;
                        break;
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
                times++;
            }

            return false;
        }
    }
}