using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._4;

[UsedImplicitly]
public class Year2023Day04 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var cards = ParseInput(input);
		var answer = 0;
		foreach (var (l, r) in cards)
		{
			var points = 0;
			foreach (var i in r)
			{
				if (!l.Contains(i))
					continue;

				if (points == 0)
					points = 1;
				else
					points *= 2;
			}
			
			answer += points;
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var cards = ParseInput(input);
		var processedCards = new DefaultDictionary<int, int>();
		for (var i = 0; i < cards.Count; i++)
		{
			processedCards[i] += 1;
			
			var (winning, current) = cards[i];
			var c = current.Count(cur => winning.Contains(cur));
			for (var j = i + 1; j <= c + i; j++)
			{
				processedCards[j] += processedCards[i];
			}
		}
		
		return processedCards.Values.Sum();
	}

	private static List<(HashSet<int>, List<int>)> ParseInput(IEnumerable<string> input)
	{
		var l = new List<(HashSet<int>, List<int>)>(); 
		foreach (var line in input)
		{
			var parts = line.Split(": ");
			var card = parts[1].Split(" | ");
			var first = card[0].Replace("  ", " ").Trim().Split().Select(int.Parse).ToHashSet();
			var second = card[1].Replace("  ", " ").Trim().Split().Select(int.Parse).ToList();
			l.Add((first, second));
		}

		return l;
	}
	
	private class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : new() where TKey : notnull
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

}