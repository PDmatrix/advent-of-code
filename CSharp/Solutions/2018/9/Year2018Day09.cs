using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._9
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day09 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var groups = Regex.Match(input.First(), @"(\d+) players; last marble is worth (\d+) points").Groups;
			var players = int.Parse(groups[1].Value);
			var lastMarble = int.Parse(groups[2].Value);

			return GetWinningScore(players, lastMarble).ToString();
		}

		private static long GetWinningScore(int players, long lastMarble)
		{
			var node = new Node(0);
			var list = new DoublyCircularLinkedList();

			list.InsertAtBeginning(node);
			var current = (player: 0, node);

			var playerList = new Dictionary<int, long>();
			for (var i = 1; i <= players; i++)
				playerList[i] = 0;

			for (var i = 1; i <= lastMarble; i++)
			{
				if (i % 23 == 0)
				{
					current = (current.player % players + 1, current.node);
					playerList[current.player] += i;
					var prev = current.node;
					for (var j = 0; j < 7; j++)
						prev = prev!.Prev;

					playerList[current.player] += prev!.Data;
					current = (current.player, prev.Next)!;
					list.Remove(prev);
					continue;
				}

				var nd = new Node(i);
				list.InsertAfter(current.node.Next!, nd);
				current = (current.player % players + 1, nd);
			}

			return playerList.Values.Max();
		}

		public object Part2(IEnumerable<string> input)
		{
			var groups = Regex.Match(input.First(), @"(\d+) players; last marble is worth (\d+) points").Groups;
			var players = int.Parse(groups[1].Value);
			long lastMarble = int.Parse(groups[2].Value) * 100;

			return GetWinningScore(players, lastMarble).ToString();
		}
	}
}