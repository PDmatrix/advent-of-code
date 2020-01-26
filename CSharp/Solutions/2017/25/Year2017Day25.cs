using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._25
{
    // ReSharper disable once UnusedMember.Global
    public class Year2017Day25 : ISolution
    {
        public string Part1(IEnumerable<string> input)
        {
            var beginRegex = 
                new Regex(@"Begin in state (?<begin>\w)\.");
            
            var checksumCheckStepsRegex = 
                new Regex(@"Perform a diagnostic checksum after (?<checksum>\d+) steps\.");

            var state = beginRegex.Match(input.ElementAt(0)).Groups["begin"].Value;
            var checksumCheckSteps = int.Parse(checksumCheckStepsRegex.Match(input.ElementAt(1)).Groups["checksum"].Value);

            var stateDict = new Dictionary<string, ((int, string, string), (int, string, string))>();
            
            var states = input.Skip(3).ToArray();
            var stateRegex = new Regex(@"In state (?<state>[A-Z]):");
            var writeRegex = new Regex(@"- Write the value (?<value>\d)\.");
            var moveRegex = new Regex(@"- Move one slot to the (?<move>\w+)\.");
            var continueRegex = new Regex(@"- Continue with state (?<continueState>[A-Z])\.");
            for (var i = 0; i < states.Length; i += 10)
            {
                var currentState = stateRegex.Match(states[i]).Groups["state"].Value;
                var ifValue0 =
                (
                    int.Parse(writeRegex.Match(states[i + 2]).Groups["value"].Value),
                    moveRegex.Match(states[i + 3]).Groups["move"].Value,
                    continueRegex.Match(states[i + 4]).Groups["continueState"].Value
                );
                
                var ifValue1 =
                (
                    int.Parse(writeRegex.Match(states[i + 6]).Groups["value"].Value),
                    moveRegex.Match(states[i + 7]).Groups["move"].Value,
                    continueRegex.Match(states[i + 8]).Groups["continueState"].Value
                );

                stateDict[currentState] = (ifValue0, ifValue1);
            }
            
            var tape = new Dictionary<int, int>();
            var currentIdx = 0;
            for (var i = 0; i < checksumCheckSteps; i++)
            {
                var (ifValueZero, ifValueNotZero) = stateDict[state];
                var currentValue = tape.GetValueOrDefault(currentIdx, 0);

                var (write, dir, next) 
                    = currentValue == 0 ? ifValueZero : ifValueNotZero;

                tape[currentIdx] = write;
                currentIdx += dir == "right" ? 1 : -1;
                state = next;
            }

            return tape.Sum(x => x.Value).ToString();
        }

        public string Part2(IEnumerable<string> input)
        {
            return "Congratulations!";
        }
    }
}