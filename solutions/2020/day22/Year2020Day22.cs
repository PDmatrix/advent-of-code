using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._22;

[UsedImplicitly]
public class Year2020Day22 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (playerOne, playerTwo) = ParseInput(input);

		while (playerOne.Count != 0 && playerTwo.Count != 0)
		{
			var playerOnePlay = playerOne.Dequeue();
			var playerTwoPlay = playerTwo.Dequeue();
			if (playerOnePlay > playerTwoPlay)
			{
				playerOne.Enqueue(playerOnePlay);
				playerOne.Enqueue(playerTwoPlay);
			} else if (playerOnePlay < playerTwoPlay)
			{
				playerTwo.Enqueue(playerTwoPlay);
				playerTwo.Enqueue(playerOnePlay);
			}
		}

		var winner = playerOne.Count != 0 ? 1 : 2;
		
		return winner == 1 ? GetScore(playerOne) : GetScore(playerTwo);
	}

	public object Part2(IEnumerable<string> input)
	{
		var (playerOne, playerTwo) = ParseInput(input);

		var winner = Play(playerOne, playerTwo);
		
		return winner == 1 ? GetScore(playerOne) : GetScore(playerTwo);
	}

	private static int GetScore(Queue<int> player)
	{
		var answer = 0;
		var index = player.Count;
		while (player.Count != 0)
		{
			answer += index * player.Dequeue();
			index--;
		}

		return answer;
	}

	private static (Queue<int>, Queue<int>) ParseInput(IEnumerable<string> input)
	{
		var playerOne = new Queue<int>();
		var playerTwo = new Queue<int>();

		var currentPlayer = 1;
		foreach (var line in input)
		{
			if (line.StartsWith("Player"))
				continue;
			if (string.IsNullOrEmpty(line))
			{
				currentPlayer++;
				continue;
			}

			if (currentPlayer == 1)
				playerOne.Enqueue(int.Parse(line));
			else
				playerTwo.Enqueue(int.Parse(line));
		}

		return (playerOne, playerTwo);
	}

	private static int Play(Queue<int> playerOne, Queue<int> playerTwo)
	{
		var playerOneStates = new HashSet<int>();
		var playerTwoStates = new HashSet<int>();
		while (playerOne.Count != 0 && playerTwo.Count != 0)
		{
			var playerOneHash = GetHash(playerOne.ToArray());
			var playerTwoHash = GetHash(playerTwo.ToArray());
			if (playerOneStates.Contains(playerOneHash) && playerTwoStates.Contains(playerTwoHash))
				return 1;
			
			playerOneStates.Add(playerOneHash);
			playerTwoStates.Add(playerTwoHash);

			var playerOnePlay = playerOne.Dequeue();
			var playerTwoPlay = playerTwo.Dequeue();
			int winner;
			if (playerOnePlay <= playerOne.Count && playerTwoPlay <= playerTwo.Count)
			{
				var newPlayerOne = new Queue<int>(playerOne.ToArray().Take(playerOnePlay));
				var newPlayerTwo = new Queue<int>(playerTwo.ToArray().Take(playerTwoPlay));
				winner = Play(newPlayerOne, newPlayerTwo);
			}
			else
			{
				winner = playerOnePlay > playerTwoPlay ? 1 : 2;
			}
			
			if (winner == 1)
			{
				playerOne.Enqueue(playerOnePlay);
				playerOne.Enqueue(playerTwoPlay);
			} else if (winner == 2)
			{
				playerTwo.Enqueue(playerTwoPlay);
				playerTwo.Enqueue(playerOnePlay);
			}
		}

		return playerOne.Count != 0 ? 1 : 2;
	}
	
	private static int GetHash(int[] playerOne)
	{
		unchecked
		{
			var hash = 19;
			foreach (var element in playerOne)
				hash = hash * 31 + element.GetHashCode();
			
			return hash;
		}
	}

}