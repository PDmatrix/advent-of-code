using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._18
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day18 : ISolution
    {
        public object Part1(IEnumerable<string> input)
        {
            return GetSafeTiles(input.First(), 40).ToString();
        }

        public object Part2(IEnumerable<string> input)
        {
            return GetSafeTiles(input.First(), 400000).ToString();
        }

        private static int GetSafeTiles(string initialRow, int rows)
        {
            var cur = $".{initialRow}.";
            var ans = 0;
            var trapConfig = new List<string> {"^^.", ".^^", "^..", "..^"};
            ans += cur.Count(c => c == '.') - 2;
            for (var row = 1; row < rows; row++)
            {
                var sb = new StringBuilder();
                for (var i = 1; i < cur.Length - 1; i++)
                    sb.Append(trapConfig.Contains(string.Concat(cur[i - 1], cur[i], cur[i + 1])) ? "^" : ".");

                var newPart = sb.ToString(); 
                ans += newPart.Count(c => c == '.');
                cur = $".{newPart}.";
            }

            return ans;
        }
    }
}