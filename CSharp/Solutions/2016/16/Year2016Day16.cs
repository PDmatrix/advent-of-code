using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2016._16;

[UsedImplicitly]
public class Year2016Day16 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        return CalculateDragonChecksum(input.First(), 272);
    }

    public object Part2(IEnumerable<string> input)
    {
        return CalculateDragonChecksum(input.First(), 35651584);
    }
        
    private static string CalculateDragonChecksum(string initialState, int length)
    {
        var current = new StringBuilder(initialState);
        while (current.Length <= length)
        {
            var b = string.Join("", current.ToString().Reverse().Select(r => r == '1' ? '0' : '1'));
            current.Append("0");
            current.Append(b);
        }

        return CalculateChecksum(string.Join("", current.ToString().Take(length)));
    }
        
    private static string CalculateChecksum(string value)
    {
        while (true)
        {
            if (value.Length % 2 == 1) 
                return value;
                
            var sb = new StringBuilder();
            for (var i = 0; i < value.Length; i += 2)
            {
                sb.Append(value[i] == value[i + 1] ? "1" : "0");
            }
            value = sb.ToString();
        }
    }
}