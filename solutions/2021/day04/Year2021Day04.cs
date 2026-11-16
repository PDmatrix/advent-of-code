using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2021._4;

[UsedImplicitly]
public class Year2021Day04 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var numbers = input.First().Split(',').Select(int.Parse).ToList();
		
		var allBoards = ParseInput(input.Skip(2));
		var boardsWithUnmarkedNumbers = GetBoardsWithUnmarkedNumbers(allBoards);

		foreach (var number in numbers)
		{
			foreach (var kv in boardsWithUnmarkedNumbers)
			{
				foreach (var hs in kv.Value)
				{
					hs.Remove(number);
					if (hs.Count != 0) continue;
					var all = kv.Value.SelectMany(x => x.Select(c => c)).ToHashSet();
					return number * allBoards[kv.Key].Where(x => all.Contains(x.Value)).Sum(x => x.Value);
				}
			}
		}

		throw new Exception("No answer found");
	}

	public object Part2(IEnumerable<string> input)
	{
		var numbers = input.First().Split(',').Select(int.Parse).ToList();
		var allBoards = ParseInput(input.Skip(2));

		var boardsWithUnmarkedNumbers = GetBoardsWithUnmarkedNumbers(allBoards);

		var winningBoards = new HashSet<int>();
		foreach (var number in numbers)
		{
			foreach (var kv in boardsWithUnmarkedNumbers.Where(x => !winningBoards.Contains(x.Key)))
			{
				foreach (var hs in kv.Value)
				{
					hs.Remove(number);
					if (hs.Count != 0) continue;
					winningBoards.Add(kv.Key);
					if (winningBoards.Count != allBoards.Count) continue;
					
					var all = kv.Value.SelectMany(x => x.Select(c => c)).ToHashSet();
					return number * allBoards[kv.Key].Where(x => all.Contains(x.Value)).Sum(x => x.Value);
				}
			}
		}

		throw new Exception("No answer found");
	}

	private static Dictionary<int, List<HashSet<int>>> GetBoardsWithUnmarkedNumbers(List<Dictionary<Position, int>> allBoards)
	{
		var boardsWithUnmarkedNumbers = new Dictionary<int, List<HashSet<int>>>();
		for (var i = 0; i < allBoards.Count; i++)
		{
			var req = new List<HashSet<int>>();
			for (var j = 0; j < 5; j++)
			{
				var hs = allBoards[i].Where(c => c.Key.X == j).Select(c => c.Value).ToHashSet();
				req.Add(hs);
			}

			for (var j = 0; j < 5; j++)
			{
				var hs = allBoards[i].Where(c => c.Key.Y == j).Select(c => c.Value).ToHashSet();
				req.Add(hs);
			}

			boardsWithUnmarkedNumbers[i] = req;
		}

		return boardsWithUnmarkedNumbers;
	}

	private static List<Dictionary<Position, int>> ParseInput(IEnumerable<string> input)
	{
		var allBoards = new List<Dictionary<Position, int>>();
		var currentDict = new Dictionary<Position, int>();
		var y = 0;
		foreach (var line in input)
		{
			if (string.IsNullOrEmpty(line))
			{
				allBoards.Add(currentDict);
				currentDict = new Dictionary<Position, int>();
				y = 0;
				continue;
			}

			var x = 0;
			foreach (var splitted in line.Split())
			{
				if (string.IsNullOrEmpty(splitted))
					continue;

				currentDict.Add(new Position(x, y), int.Parse(splitted));
				x++;
			}

			y++;
		}

		allBoards.Add(currentDict);
		return allBoards;
	}
}