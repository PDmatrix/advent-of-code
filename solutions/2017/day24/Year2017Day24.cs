using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2017._24;

[UsedImplicitly]
public class Year2017Day24 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var parts = input.Select(x =>
        {
            var splitted = x.Split('/').Select(int.Parse).ToArray();
            return (splitted.First(), splitted.Last());
        });
        var result = Build(0, new int[0], parts, 0).Max(x => x.strength);
        return result.ToString();
    }

    public object Part2(IEnumerable<string> input)
    {
        var parts = input.Select(x =>
        {
            var splitted = x.Split('/').Select(int.Parse).ToArray();
            return (splitted.First(), splitted.Last());
        });
            
        var result = Build(0, new int[0], parts, 0)
            .OrderByDescending(x => x.used.Count())
            .ThenByDescending(x => x.strength)
            .First()
            .strength;
            
        return result.ToString();
    }

    private static IEnumerable<(IEnumerable<int> used, int strength)> Build(
        int cur,
        IEnumerable<int> used,
        IEnumerable<(int a, int b)> available,
        int strength)
    {
        var usedArr = used.ToArray();
        var availableArr = available.ToArray();
        foreach (var (a, b) in availableArr)
        {
            if (a == cur || b == cur)
            {
                foreach (var res in Build(a == cur ? b : a, usedArr.Concat(new[] {a, b}),
                             availableArr.Where(x => !(x.a == a && x.b == b)), strength + a + b))
                {
                    yield return res;
                }
            }
        }

        yield return (usedArr, strength);
    }
}