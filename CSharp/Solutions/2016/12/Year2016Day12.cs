using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._12
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day12 : ISolution
    {
        public string Part1(IEnumerable<string> input)
        {
            var reg = new Dictionary<string, int>
            {
                ["a"] = 0,
                ["b"] = 0,
                ["c"] = 0,
                ["d"] = 0
            };
            var idx = 0;
            var commands = input as string[] ?? input.ToArray();
            while (idx < commands.Length)
            {
                var command = commands[idx];
                if (command.StartsWith("cpy"))
                {
                    var splitted = command.Split();
                    var firstValue = splitted[1];
                    var secondValue = splitted[2];
                    if (int.TryParse(firstValue, out var res))
                    {
                        reg[secondValue] = res;
                    }
                    else
                    {
                        reg[secondValue] = reg[firstValue];
                    }
                }

                if (command.StartsWith("inc"))
                {
                    var regName = command.Split()[1];
                    reg[regName] += 1;
                }
                
                if (command.StartsWith("dec"))
                {
                    var regName = command.Split()[1];
                    reg[regName] -= 1;
                }

                if (command.StartsWith("jnz"))
                {
                    var splitted = command.Split();
                    var firstValue = splitted[1];
                    var secondValue = splitted[2];
                    var fValue = int.TryParse(firstValue, out var res) ? res : reg[firstValue];
                    var jumpValue = int.TryParse(secondValue, out var res2) ? res2 : reg[secondValue];
                    if (fValue != 0)
                    {
                        idx += jumpValue;
                        continue;
                    }
                }

                idx += 1;
            }
            return reg["a"].ToString();
        }

        public string Part2(IEnumerable<string> input)
        {
            var reg = new Dictionary<string, int>
            {
                ["a"] = 0,
                ["b"] = 0,
                ["c"] = 1,
                ["d"] = 0
            };
            
            var idx = 0;
            var commands = input as string[] ?? input.ToArray();
            while (idx < commands.Length)
            {
                var command = commands[idx];
                if (command.StartsWith("cpy"))
                {
                    var splitted = command.Split();
                    var firstValue = splitted[1];
                    var secondValue = splitted[2];
                    if (int.TryParse(firstValue, out var res))
                    {
                        reg[secondValue] = res;
                    }
                    else
                    {
                        reg[secondValue] = reg[firstValue];
                    }
                }

                if (command.StartsWith("inc"))
                {
                    var regName = command.Split()[1];
                    reg[regName] += 1;
                }
                
                if (command.StartsWith("dec"))
                {
                    var regName = command.Split()[1];
                    reg[regName] -= 1;
                }

                if (command.StartsWith("jnz"))
                {
                    var splitted = command.Split();
                    var firstValue = splitted[1];
                    var secondValue = splitted[2];
                    var fValue = int.TryParse(firstValue, out var res) ? res : reg[firstValue];
                    var jumpValue = int.TryParse(secondValue, out var res2) ? res2 : reg[secondValue];
                    if (fValue != 0)
                    {
                        idx += jumpValue;
                        continue;
                    }
                }

                idx += 1;
            }
            return reg["a"].ToString();
        }
    }
}