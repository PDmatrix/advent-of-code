using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._23;

[UsedImplicitly]
public class Year2020Day23 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var cups = new LinkedList<int>();

		foreach (var cup in input.First())
		{
			var cupValue = int.Parse(cup.ToString());
			cups.AddLast(cupValue);
		}

		var currentCup = cups.First ?? throw new Exception("First is not found");

		for (int i = 0; i < 100; i++)
		{
			var removedCups = new List<int>();
			for (int j = 0; j < 3; j++)
			{
				var nextCup = currentCup.Next ?? currentCup.List?.First ?? throw new Exception();
				cups.Remove(nextCup);
				removedCups.Add(nextCup.Value);
			}

			var destinationValue = currentCup.Value - 1;
			LinkedListNode<int>? destination = null;
			while (true)
			{
				if (destinationValue <= 0)
				{
					var maxValue = cups.Max();
					destination = cups.Find(maxValue);
					break;
				}
				
				destination = cups.Find(destinationValue);
				if (destination != null)
					break;

				destinationValue--;
			}
			foreach (var removedCup in removedCups)
			{
				cups.AddAfter(destination, new LinkedListNode<int>(removedCup));
				destination = destination.Next;
			}

			currentCup = currentCup.Next ?? currentCup.List?.First ?? throw new Exception();
		}

		var sb = new StringBuilder();
		var node = cups.Find(1);
		while (true)
		{
			var next = node.Next ?? node.List?.First ?? throw new Exception();
			if (next.Value == 1)
				break;

			sb.Append(next.Value);
			node = next;
		}
		
		return sb;
	}

	public object Part2(IEnumerable<string> input)
	{
		var cups = new LinkedList<int>();

		var cupsDict = new Dictionary<int, LinkedListNode<int>>();
		foreach (var cup in input.First())
		{
			var cupValue = int.Parse(cup.ToString());
			cupsDict[cupValue] = cups.AddLast(cupValue);
		}

		for (var i = cups.Max() + 1; i <= 1000000; i++)
		{
			cupsDict[i] = cups.AddLast(i);
		}

		var currentCup = cups.First ?? throw new Exception("First is not found");

		for (int i = 0; i < 10000000; i++)
		{
			var removedCups = new List<int>(3);
			for (int j = 0; j < 3; j++)
			{
				var nextCup = currentCup.Next ?? currentCup.List?.First ?? throw new Exception();
				cups.Remove(nextCup);
				cupsDict.Remove(nextCup.Value);
				removedCups.Add(nextCup.Value);
			}

			var destinationValue = currentCup.Value - 1;
			LinkedListNode<int>? destination;
			while (true)
			{
				if (destinationValue <= 0)
				{
					var maxValue = cups.Max();
					destination = cupsDict[maxValue];
					break;
				}

				if (!cupsDict.ContainsKey(destinationValue))
				{
					destinationValue--;
					continue;
				}
				
				destination = cupsDict[destinationValue];
				break;
			}
			foreach (var removedCup in removedCups)
			{
				var addedNode = new LinkedListNode<int>(removedCup);
				cups.AddAfter(destination, addedNode);
				destination = destination.Next;
				cupsDict.Add(removedCup, addedNode);
			}

			currentCup = currentCup.Next ?? currentCup.List?.First ?? throw new Exception();
		}

		var node = cups.Find(1);

		return (long)node.Next.Value * node.Next.Next.Value;
	}
}