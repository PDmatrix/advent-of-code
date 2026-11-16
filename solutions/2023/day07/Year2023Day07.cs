using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._7;

[UsedImplicitly]
public class Year2023Day07 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var sets = ParseInput(input);

		var orderedSets = sets.OrderBy(x => x.Hand.Type)
			.ThenByDescending(x => x.Hand.Representation, new HandComparer())
			.ToList();
		
		return orderedSets.Select((t, i) => t.Bid * (orderedSets.Count - i)).Sum();
	}

	public object Part2(IEnumerable<string> input)
	{
		var sets = ParseInput(input, true);

		var orderedSets = sets.OrderBy(x => x.Hand.Type)
			.ThenByDescending(x => x.Hand.Representation, new HandComparer(true))
			.ToList();
		
		return orderedSets.Select((t, i) => t.Bid * (orderedSets.Count - i)).Sum();
	}

	private class HandComparer : IComparer<string>
	{
		public HandComparer(bool withJoker = false)
		{
			if (withJoker)
				Orders['J'] = 1;
		}
		
		private static readonly Dictionary<char, int> Orders = new()
		{
			{ '2', 2 },
			{ '3', 3 },
			{ '4', 4 },
			{ '5', 5 },
			{ '6', 6 },
			{ '7', 7 },
			{ '8', 8 },
			{ '9', 9 },
			{ 'T', 10 },
			{ 'J', 11 },
			{ 'Q', 12 },
			{ 'K', 13 },
			{ 'A', 14 },
		};
		
		public int Compare(string? x, string? y)
		{
			for (var i = 0; i < x.Length; i++)
			{
				var xVal = Orders.GetValueOrDefault(x[i], -1);
				var yVal = Orders.GetValueOrDefault(y[i], -1);

				if (xVal != yVal)
					return xVal.CompareTo(yVal); // Higher card wins
			}
			
			return 0;
		}
	}
	
	private static List<Set> ParseInput(IEnumerable<string> input, bool withJoker = false)
	{
		var sets = new List<Set>();
		foreach (var line in input)
		{
			var splitted = line.Split(' ');
			var hand = Hand.Parse(splitted[0], withJoker);
			var bid = int.Parse(splitted[1]);
			sets.Add(new Set(hand, bid));
		}
		
		return sets;
	}

	private record struct Set(Hand Hand, int Bid);
	
	private record struct Hand(string Representation, HandType Type)
	{
		public static Hand Parse(string representation, bool withJoker = false)
		{
			var type = withJoker ? GetHandTypeWithJoker(representation) : GetHandType(representation);
			return new Hand(representation, type);
		}
	}

	private static HandType GetHandType(string hand)
	{
		var c = ToCounter(hand);
		
		if (c.ContainsValue(5))
			return HandType.FiveOfAKind;
		if (c.ContainsValue(4))
			return HandType.FourOfAKind;
		if (c.ContainsValue(3) && c.ContainsValue(2))
			return HandType.FullHouse;
		if (c.ContainsValue(3))
			return HandType.ThreeOfAKind;
		if (c.Count(x => x.Value == 2) == 2)
			return HandType.TwoPair;
		if (c.ContainsValue(2))
			return HandType.OnePair;
		
		return HandType.HighCard;
	}
	
	private static HandType GetHandTypeWithJoker(string hand)
	{
		var c = ToCounter(hand);
		
		var jokerCount = c.GetValueOrDefault('J', 0);
		if (jokerCount == 0)
			return GetHandType(hand);

		c.Remove('J');
		if (c.Count(x => x.Value == 2) == 2) // Two pairs with a joker
			return HandType.FullHouse;

		if (c.Values.Count == 0)
			return HandType.FiveOfAKind;
		
		var maxCount = c.Values.Max();
		var withJokerCount = maxCount + jokerCount;
		
		return withJokerCount switch
		{
			5 => HandType.FiveOfAKind,
			4 => HandType.FourOfAKind,
			3 when c.Count(x => x.Value == 2) == 2 => HandType.FullHouse,
			3 => HandType.ThreeOfAKind,
			2 => HandType.OnePair,
			_ => HandType.HighCard
		};
	}
	
	// https://stackoverflow.com/a/77061284
	private static Dictionary<TSource, int> ToCounter<TSource>(
		IEnumerable<TSource> source,
		IEqualityComparer<TSource>? comparer = null) where TSource : notnull
	{
		ArgumentNullException.ThrowIfNull(source);

		Dictionary<TSource, int> dictionary = new(comparer);
		foreach (var item in source)
		{
			CollectionsMarshal.GetValueRefOrAddDefault(dictionary, item, out _)++;
		}
		return dictionary;
	}
	
	private enum HandType
	{
		FiveOfAKind,
		FourOfAKind,
		FullHouse,
		ThreeOfAKind,
		TwoPair,
		OnePair,
		HighCard
	}
}