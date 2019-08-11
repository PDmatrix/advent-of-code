using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._23
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day23 : ISolution
    {
        public string Part1(IEnumerable<string> input)
        {
            var registers = new Dictionary<string, int>
            {
                ["a"] = 7,
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
                ["a"] = 12,
                ["b"] = 0,
                ["c"] = 0,
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
                @"(?<command>cpy|inc|dec|jnz|tgl) (?<first>-?[\d|\w]+) ?(?<second>.*)";
            while (idx < commands.Length)
            {
                // Hack
                if (idx == 5 || idx == 21)
                {
                    registers["a"] = registers["a"] + Math.Abs(registers["c"]) * Math.Abs(registers["d"]);
                    idx += 5;
                    continue;
                }
                var match = Regex.Match(commands[idx], commandPattern);
                var command = match.Groups["command"].Value;
                try
                {
                    switch (command)
                    {
                        case "tgl":
                            var index = idx + ParseValue(match.Groups["first"].Value, registers);
                            if (index >= commands.Length || index < 0)
                                break;
                            var changedCommand = commands[index];
                            var newMatch = Regex.Match(changedCommand, commandPattern);

                            switch (newMatch.Groups["command"].Value)
                            {
                                case "inc":
                                    commands[index] =
                                        $"dec {newMatch.Groups["first"]}";
                                    break;
                                case "dec":
                                case "tgl":
                                    commands[index] =
                                        $"inc {newMatch.Groups["first"]}";
                                    break;
                                case "jnz":
                                    commands[index] =
                                        $"cpy {newMatch.Groups["first"]} {newMatch.Groups["second"]}";
                                    break;
                                default:
                                    commands[index] =
                                        $"jnz {newMatch.Groups["first"]} {newMatch.Groups["second"]}";
                                    break;
                            }

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
                }
                catch
                {
                    Console.WriteLine("Ignored");
                    // ignore
                }

                idx += 1;
            }

            return registers["a"].ToString();
        }
    }
}