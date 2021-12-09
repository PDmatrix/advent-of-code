using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2016._7;

[UsedImplicitly]
public class Year2016Day07 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var counter = 0;
        foreach (var ip in input)
        {
            var hypernets = Regex.Matches(ip, @"\[.+?\]").Select(r => r.Value.Trim('[',']')).ToArray();
            var newIp = hypernets.Aggregate(ip, (current, match) => current.Replace("["+match+"]", " "));
            var isHypernetsWithoutAbba = hypernets.Select(HasAbba).All(r => r == false);
            var isAbba = newIp.Split().Select(HasAbba).Any(r => r);
            if (isHypernetsWithoutAbba && isAbba)
                counter++;
        }
        return counter.ToString();
    }

    private static bool HasAbba(string seq)
    {
        for (var i = 0; i < seq.Length - 3; i++)
        {
            if(seq[i] == seq[i + 1])
                continue;
            var pair = new string(new []{seq[i], seq[i + 1]});
            if (pair == new string(new []{seq[i + 3], seq[i + 2]}))
                return true;
        }

        return false;
    }
        
    private static IEnumerable<string> GetAbas(string seq)
    {
        for (var i = 0; i < seq.Length - 2; i++)
        {
            if(seq[i] == seq[i + 1])
                continue;

            if (seq[i] == seq[i + 2])
                yield return new string(new[] {seq[i], seq[i + 1], seq[i + 2]});
        }
    }

    public object Part2(IEnumerable<string> input)
    {
        var counter = 0;
        foreach (var ip in input)
        {
            var hypernets = Regex.Matches(ip, @"\[.+?\]").Select(r => r.Value.Trim('[',']')).ToArray();
            var newIp = hypernets.Aggregate(ip, (current, match) => current.Replace("["+match+"]", " "));
            var abaList = newIp.Split().Aggregate(new List<string>(), (current, seq) => current.Union(GetAbas(seq)).ToList());
            var reversedAbaList = abaList.Select(r => string.Concat(r[1], r[0], r[1]));
            if (hypernets.Select(r => reversedAbaList.Select(r.Contains).Any(x => x)).Any(r => r))
                counter++;
        }
        return counter.ToString();
    }
}