using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._6
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day06 : ISolution
    {
        public object Part1(IEnumerable<string> input)
        {
            char MostCommonCharacter(string r) => 
                r.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
            
            var errorCorrectedMessage = 
                Transpose(input.ToArray()).Select(MostCommonCharacter);
            return string.Join("", errorCorrectedMessage);
        }

        public object Part2(IEnumerable<string> input)
        {
            char LeastCommonCharacter(string r) => 
                r.GroupBy(x => x).OrderBy(x => x.Count()).First().Key;
            
            var errorCorrectedMessage = 
                Transpose(input.ToArray()).Select(LeastCommonCharacter);
            return string.Join("", errorCorrectedMessage);
        }

        private static IEnumerable<string> Transpose(IReadOnlyList<string> array)
        {
            var messageLength = array.First().Length;
            for (var i = 0; i < messageLength; i++)
            {
                yield return array.Aggregate("", (current, t) => current + t[i]);
            }
        }
    }
}