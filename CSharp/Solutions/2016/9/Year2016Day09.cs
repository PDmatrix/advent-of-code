using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._9
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day09 : ISolution
    {
        public string Part1(IEnumerable<string> input)
        {
            var compressed = input.First();
            return Compute(compressed, false).ToString();
        }

        public string Part2(IEnumerable<string> input)
        {
            var compressed = input.First();
            return Compute(compressed, true).ToString();
        }

        private static long Compute(string str, bool recursive)
        {
            long length = str.Length;

            for (var i = 0; i < str.Length; i++) {
                if (str[i] != '(') continue;
                var match = Regex.Match(str.Substring(i), @"\((\d+)x(\d+)\)").Groups;
                var matchLength = int.Parse(match[1].Value);
                var times = long.Parse(match[2].Value);
                var start = i + match[0].Length;
                var matchStr = str.Substring(start, matchLength);
                var decompressedLength = recursive ? Compute(matchStr, true) : matchStr.Length;
                length += decompressedLength * times - matchStr.Length - match[0].Length;
                i = start + matchStr.Length - 1;
            }

            return length;
        }
    }
}