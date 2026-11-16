using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._16;

[UsedImplicitly]
public class Year2019Day16 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var inputSignal = input.First();
        var inputLength = inputSignal.Length;
        for (int phase = 0; phase < 100; phase++)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < inputLength; i++)
            {
                var pattern = GetPattern(i, inputLength).ToArray();
                var newNum = 0;
                for (int j = 0; j < inputLength; j++)
                {
                    var num = int.Parse(inputSignal[j].ToString());
                    newNum += num * pattern[j];
                }
                sb.Append(newNum.ToString().Last());
            }

            inputSignal = sb.ToString();
        }
        
        return inputSignal[..8];
    }

    private IEnumerable<int> GetPattern(int repeats, int inputLength)
    {
        var basePattern = new List<int> { 0, 1, 0, -1 };
        var patternWithRequiredLength = new List<int>();
        for (var i = 0; i <= inputLength; i++)
        {
            patternWithRequiredLength.Add(basePattern[i % basePattern.Count]);
        }
        
        if (repeats == 0)
            return patternWithRequiredLength.Skip(1);

        var patternWithRepeated = new List<int>();
        for (var i = 0; i <= inputLength; i++)
        {
            for (var j = 0; j <= repeats; j++)
            {
                patternWithRepeated.Add(basePattern[i % basePattern.Count]);
                if (patternWithRepeated.Count > inputLength)
                    break;
            }
            if (patternWithRepeated.Count > inputLength)
                break;
        }
        
        return patternWithRepeated.Skip(1);
    }

    public object Part2(IEnumerable<string> input)
    {
        var inputSignal = new List<int>(input.First().Length * 10000);
        var originalSignal = input.First().Select(x => x.ToString()).Select(int.Parse).ToArray();
        
        for (var i = 0; i < 10000; i++)
            inputSignal.AddRange(originalSignal);

        var skip = int.Parse(string.Join(string.Empty, inputSignal.Take(7).ToArray()));
        inputSignal = inputSignal.Skip(skip).ToList();
        var inputLength = inputSignal.Count;
        for (int phase = 0; phase < 100; phase++)
        {
            var cuSum = 0;
            for (var i = inputLength - 1; i >= 0; i--)
            {
                cuSum += inputSignal[i];
                inputSignal[i] = cuSum % 10;
            }
        }
        
        return string.Join(string.Empty, inputSignal.Take(8));
    }
}