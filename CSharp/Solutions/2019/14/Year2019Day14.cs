using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._14;

[UsedImplicitly]
public class Year2019Day14 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var reactions = ParseInput(input);

        return OreRequired(reactions);
    }

    private static long OreRequired(Dictionary<string, Reaction> reactions, long fuel = 1)
    {
        var chemNeeded = new DefaultDictionary<string, long>
        {
            ["FUEL"] = fuel
        };
        var chemHave = new DefaultDictionary<string, long>();
        long ore = 0;
        while (chemNeeded.Count != 0)
        {
            var item = chemNeeded.Keys.ToList()[0];
            if (chemNeeded[item] <= chemHave[item])
            {
                chemHave[item] -= chemNeeded[item];
                chemNeeded.Remove(item);
                continue;
            }

            var numNeeded = chemNeeded[item] - chemHave[item];
            chemHave.Remove(item);
            chemNeeded.Remove(item);
            var numProduced = reactions[item].Out;

            long numReactions = 0;
            if (numNeeded / numProduced == Math.Ceiling((double)numNeeded / numProduced))
                numReactions = numNeeded / numProduced;
            else
                numReactions = (numNeeded / numProduced) + 1;

            chemHave[item] += (numReactions * numProduced) - numNeeded;
            foreach (var chem in reactions[item].In)
            {
                if (chem.Key == "ORE")
                    ore += reactions[item].In[chem.Key] * numReactions;
                else
                    chemNeeded[chem.Key] += reactions[item].In[chem.Key] * numReactions;
            }
        }

        return ore;
    }

    private static Dictionary<string, Reaction> ParseInput(IEnumerable<string> input)
    {
        var reactions = new Dictionary<string, Reaction>();
        foreach (var line in input)
        {
            var splitted = line.Split(" => ");
            var a = splitted.Last().Split(" ");
            var inputs = new Dictionary<string, long>();
            foreach (var reaction in splitted.First().Split(", "))
            {
                var splt = reaction.Split(" ");
                inputs.Add(splt[1], long.Parse(splt[0]));
            }

            reactions.Add(a[1], new Reaction(long.Parse(a[0]), inputs));
        }

        return reactions;
    }

    public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : new() where TKey : notnull
    {
        public new TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var val)) 
                    return val;
                val = new TValue();
                Add(key, val);
                return val;
            }
            set => base[key] = value;
        }
    }

    public record Reaction(long Out, Dictionary<string, long> In);

    public object Part2(IEnumerable<string> input)
    {
        var reactions = ParseInput(input);
        var low = (long) 1e12 / OreRequired(reactions);
        var high = 10 * low;

        while (OreRequired(reactions, high) < 1e12)
        {
            low = high;
            high = 10 * low;
        }

        long mid = 0;
        while (low < high - 1)
        {
            mid = (low + high) / 2;
            var ore = OreRequired(reactions, mid);
            if (ore < 1e12)
                low = mid;
            else if (ore > 1e12)
                high = mid;
            else
                break;
        }

        return mid - 1;
    }
}