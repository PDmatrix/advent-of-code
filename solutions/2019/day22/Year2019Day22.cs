using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2019._22;

[UsedImplicitly]
public class Year2019Day22 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var deck = Enumerable.Range(0, 10007).ToList();
        foreach (var line in input)
        {
            if (line.StartsWith("deal with increment"))
            {
                var regex = new Regex(@"deal with increment (?<inc>\d+)");
                var match = regex.Match(line);
                DealWithInc(ref deck, int.Parse(match.Groups["inc"].Value));
            } else if (line.StartsWith("cut"))
            {
                var regex = new Regex(@"cut (?<cut>-?\d+)");
                var match = regex.Match(line);
                Cut(ref deck, int.Parse(match.Groups["cut"].Value));
            }
            else
            {
                Deal(ref deck);
            }
        }



        return deck.FindIndex(x => x == 2019);
    }

    private static void Deal(ref List<int> deck)
    {
        deck.Reverse();
    }
    
    private static void DealWithInc(ref List<int> deck, int inc)
    {
        var newList = new List<int>(deck);
        var index = 0;
        var deckIndex = 0;
        while (index < deck.Count)
        {
            newList[deckIndex % deck.Count] = deck[index];
            deckIndex += inc;
            index++;
        }

        deck = newList;
    }

    
    private static void Cut(ref List<int> deck, int cutIndex)
    {
        if (cutIndex > 0)
        {
            var cutted = deck.Take(cutIndex).ToList();
            var rest = deck.Skip(cutIndex).ToList();
            deck = new List<int>(cutted.Count + rest.Count);
            deck.AddRange(rest);
            deck.AddRange(cutted);
        }
        else
        {
            var len = deck.Count + cutIndex;
            var cutted = deck.Take(len).ToList();
            var rest = deck.Skip(len).ToList();
            deck = new List<int>(cutted.Count + rest.Count);
            deck.AddRange(rest);
            deck.AddRange(cutted);
        }
    }

    public object Part2(IEnumerable<string> input)
    {
        var cards = new BigInteger(119315717514047);
        var repeats = new BigInteger(101741582076661);

        var incrementMul = BigInteger.One;
        var offsetDiff = BigInteger.Zero;
        
        foreach (var line in input)
        {
            if (line.StartsWith("deal with increment"))
            {
                var regex = new Regex(@"deal with increment (?<inc>\d+)");
                var match = regex.Match(line);
                var q = long.Parse(match.Groups["inc"].Value);
                incrementMul = BigInteger.Multiply(incrementMul,
                    BigInteger.ModPow(q, BigInteger.Subtract(cards, new BigInteger(2)), cards));
                incrementMul = BigInteger.Remainder(incrementMul, cards);
                if (incrementMul.Sign == -1)
                    incrementMul = BigInteger.Add(incrementMul, cards);
            } else if (line.StartsWith("cut"))
            {
                var regex = new Regex(@"cut (?<cut>-?\d+)");
                var match = regex.Match(line);
                var q = long.Parse(match.Groups["cut"].Value);
                offsetDiff = BigInteger.Add(offsetDiff, BigInteger.Multiply(q, incrementMul));
                offsetDiff = BigInteger.Remainder(offsetDiff, cards);
                if (offsetDiff.Sign == -1)
                    offsetDiff = BigInteger.Add(offsetDiff, cards);

            }
            else
            {
                incrementMul = BigInteger.Multiply(new BigInteger(-1), incrementMul);
                incrementMul = BigInteger.Remainder(incrementMul, cards);
                if (incrementMul.Sign == -1)
                    incrementMul = BigInteger.Add(incrementMul, cards);
                offsetDiff = BigInteger.Add(offsetDiff, incrementMul);
                offsetDiff = BigInteger.Remainder(offsetDiff, cards);
                if (offsetDiff.Sign == -1)
                    offsetDiff = BigInteger.Add(offsetDiff, cards);
            }
        }

        var increment = BigInteger.ModPow(incrementMul, repeats, cards);
        var offset = BigInteger.Multiply(offsetDiff,
            BigInteger.Multiply(BigInteger.Subtract(BigInteger.One, increment),
                BigInteger.ModPow(BigInteger.Remainder(BigInteger.Subtract(BigInteger.One, incrementMul), cards),
                    BigInteger.Subtract(cards, 2), cards)));
        offset = BigInteger.Remainder(offset, cards);
        
        return BigInteger.Remainder(BigInteger.Add(offset, BigInteger.Multiply(new BigInteger(2020), increment)), cards).ToString();
    }
}