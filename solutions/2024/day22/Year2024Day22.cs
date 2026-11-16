using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._22;

[UsedImplicitly]
public class Year2024Day22 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        long answer = 0;
        foreach (var line in input)
        {
            var secret = long.Parse(line);
            for (var i = 0; i < 2000; i++)
                secret = CalculateNext(secret);

            answer += secret;
        }

        return answer;
    }

    public object Part2(IEnumerable<string> input)
    {
        var monkeySecrets = new Dictionary<int, List<long>>();
        var monkey = 0;
        foreach (var line in input)
        {
            var secret = long.Parse(line);
            monkeySecrets[monkey] = new List<long> { secret };
            for (var i = 0; i < 2000; i++)
            {
                secret = CalculateNext(secret);
                monkeySecrets[monkey].Add(secret);
            }

            monkey++;
        }

        monkey = 0;
        var sequences = new Dictionary<int, Dictionary<string, long>>();
        foreach (var (_, secrets) in monkeySecrets)
        {
            sequences[monkey] = new Dictionary<string, long>();
            var sb = new StringBuilder();
            for (var i = 1; i < secrets.Count - 3; i++)
            {
                sb.Clear();
                var prev = secrets[i - 1] % 10;
                sb.Append(secrets[i] % 10 - prev);
                sb.Append(',');
                sb.Append(secrets[i + 1] % 10 - secrets[i] % 10);
                sb.Append(',');
                sb.Append(secrets[i + 2] % 10 - secrets[i + 1] % 10);
                sb.Append(',');
                sb.Append(secrets[i + 3] % 10 - secrets[i + 2] % 10);

                var s = sb.ToString();
                if (sequences[monkey].ContainsKey(s))
                    continue;

                sequences[monkey][sb.ToString()] = secrets[i + 3] % 10;
            }

            monkey++;
        }

        var uniqSequences = new HashSet<string>();
        foreach (var (_, v) in sequences)
        {
            foreach (var key in v)
                uniqSequences.Add(key.Key);
        }

        var max = long.MinValue;
        foreach (var uniqSequence in uniqSequences)
        {
            var seqWithValues = sequences.Where(x => x.Value.ContainsKey(uniqSequence));
            max = Math.Max(max, seqWithValues.Sum(x => x.Value[uniqSequence]));
        }

        return max;
    }

    private static long CalculateNext(long secret)
    {
        secret = Prune(Mix(secret, secret * 64));
        secret = Prune(Mix(secret, secret / 32));
        secret = Prune(Mix(secret, secret * 2048));

        return secret;
    }

    private static long Mix(long a, long b) => a ^ b;

    private static long Prune(long a) => a % 16777216;
}