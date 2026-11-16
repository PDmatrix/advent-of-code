using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._2;

[UsedImplicitly]
public class Year2023Day02 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var games = ParseInput(input);
		var bag = new Dictionary<string, int>
		{
			{ "red", 12 },
			{ "green", 13 },
			{ "blue", 14 }
		};

		var id = 1;
		var answer = 0;
		foreach (var game in games)
		{
			var gamePossible = true;
			foreach (var g in game)
			{
				foreach (var (k, v) in g)
				{
					if (bag[k] >= v)
						continue;
					
					gamePossible = false;
					break;
				}
				
				if (!gamePossible)
					break;
			}
			
			if (gamePossible)
				answer += id;
			id++;
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var games = ParseInput(input);

		var answer = 0;
		foreach (var game in games)
		{
			var minimum = new Dictionary<string, int>
			{
				{ "red", 0 },
				{ "green", 0 },
				{ "blue", 0 }
			};

			foreach (var g in game)
			{
				foreach (var (k, v) in g)
				{
					minimum[k] = Math.Max(minimum[k], v);
				}
			}
			
			answer += minimum.Values.Aggregate(1, (current, value) => current * value);
		}
		
		return answer;
	}

	private static List<List<Dictionary<string, int>>> ParseInput(IEnumerable<string> input)
	{
		// Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
		var games = new List<List<Dictionary<string, int>>>();
		foreach (var line in input)
		{
			var gameList = new List<Dictionary<string, int>>();
			var allGames = line.Split(": ")[1];
			var gamesSplit = allGames.Split("; ");
			foreach (var game in gamesSplit)
			{
				var d = new Dictionary<string, int>();
				var colors = game.Split(", ");
				foreach (var color in colors)
				{
					d[color.Split()[1]] = int.Parse(color.Split()[0]);
				}
				
				gameList.Add(d);
			}
			
			games.Add(gameList);
		}

		return games;
	}
}