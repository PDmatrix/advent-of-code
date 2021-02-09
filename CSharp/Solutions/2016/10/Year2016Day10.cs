using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._10
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day10 : ISolution
    {
        public object Part1(IEnumerable<string> input)
        {
            var bots = new Dictionary<int, Action<int>>();
            var givePattern = new Regex(@"bot (?<source>\d+) gives low to (?<low>(bot|output)) (?<lowval>\d+) and high to (?<high>(bot|output)) (?<highval>\d+)");
            var valuePattern = new Regex(@"value (?<value>\d+) goes to bot (?<bot>\d+)");
            var result = 0;
            foreach (var instruction in input.OrderBy(r => r))
            {
                var valueMatch = valuePattern.Match(instruction);
                if (valueMatch.Success)
                {
                    bots[int.Parse(valueMatch.Groups["bot"].Value)]
                        (int.Parse(valueMatch.Groups["value"].Value));
                }

                var giveMatch = givePattern.Match(instruction);
                if (!giveMatch.Success) 
                    continue;
                
                var values = new List<int>();
                var botNumber = int.Parse(giveMatch.Groups["source"].Value);
                bots[botNumber] = value =>
                {
                    values.Add(value);
                    if (values.Count != 2)
                        return;

                    if (values.Min() == 17 && values.Max() == 61) result = botNumber;
                    if (giveMatch.Groups["low"].Value == "bot")
                        bots[int.Parse(giveMatch.Groups["lowval"].Value)](values.Min());

                    if (giveMatch.Groups["high"].Value == "bot")
                        bots[int.Parse(giveMatch.Groups["highval"].Value)](values.Max());
                };
            }
            
            return result.ToString();
        }

        public object Part2(IEnumerable<string> input)
        {
            var bots = new Dictionary<int, Action<int>>();
            var outputs = new int[32];
            var givePattern = new Regex(@"bot (?<source>\d+) gives low to (?<low>(bot|output)) (?<lowval>\d+) and high to (?<high>(bot|output)) (?<highval>\d+)");
            var valuePattern = new Regex(@"value (?<value>\d+) goes to bot (?<bot>\d+)");
            foreach (var instruction in input.OrderBy(r => r))
            {
                var valueMatch = valuePattern.Match(instruction);
                if (valueMatch.Success)
                {
                    bots[int.Parse(valueMatch.Groups["bot"].Value)]
                        (int.Parse(valueMatch.Groups["value"].Value));
                }

                var giveMatch = givePattern.Match(instruction);
                if (!giveMatch.Success) 
                    continue;
                
                var values = new List<int>();
                var botNumber = int.Parse(giveMatch.Groups["source"].Value);
                bots[botNumber] = value =>
                {
                    values.Add(value);
                    if (values.Count != 2)
                        return;

                    if (giveMatch.Groups["low"].Value == "bot")
                        bots[int.Parse(giveMatch.Groups["lowval"].Value)](values.Min());
                    else
                        outputs[int.Parse(giveMatch.Groups["lowval"].Value)] = values.Min();

                    if (giveMatch.Groups["high"].Value == "bot")
                        bots[int.Parse(giveMatch.Groups["highval"].Value)](values.Max());
                    else
                        outputs[int.Parse(giveMatch.Groups["highval"].Value)] = values.Max();
                };
            }

            return (outputs[0] * outputs[1] * outputs[2]).ToString();
        }
    }
}