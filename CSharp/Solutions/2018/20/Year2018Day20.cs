using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2018._20
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day20 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var paths = input.First()[1..^1];
			var (x, y) = (5000, 5000);

			var positions = new Stack<(int x, int y)>();
			var (prevX, prevY) = (x, y);
			var distances = new Dictionary<(int x, int y), int>();
			var directions = new Dictionary<char, (int x, int y)>
			{
				['N'] = (0, -1),
				['E'] = (1, 0),
				['S'] = (0, 1),
				['W'] = (-1, 0)
			};
			foreach (var c in paths)
			{
				switch (c)
				{
					case '(':
						positions.Push((x, y));
						break;
					case ')':
						(x, y) = positions.Pop();
						break;
					case '|':
						(x, y) = positions.First();
						break;
					default:
					{
						var (dx, dy) = directions[c];
						x += dx;
						y += dy;

						if (distances.ContainsKey((x, y)))
							distances[(x, y)] = Math.Min(distances[(x, y)], distances[(prevX, prevY)] + 1);
						else
							distances[(x, y)] = distances.GetValueOrDefault((prevX, prevY), 0) + 1;
						break;
					}
				}

				(prevX, prevY) = (x, y);
			}
			
			return distances.Values.Max().ToString();
		}
		
		public string Part2(IEnumerable<string> input)
		{
			var paths = input.First()[1..^1];
			var (x, y) = (5000, 5000);

			var positions = new Stack<(int x, int y)>();
			var (prevX, prevY) = (x, y);
			var distances = new Dictionary<(int x, int y), int>();
			var directions = new Dictionary<char, (int x, int y)>
			{
				['N'] = (0, -1),
				['E'] = (1, 0),
				['S'] = (0, 1),
				['W'] = (-1, 0)
			};
			foreach (var c in paths)
			{
				switch (c)
				{
					case '(':
						positions.Push((x, y));
						break;
					case ')':
						(x, y) = positions.Pop();
						break;
					case '|':
						(x, y) = positions.First();
						break;
					default:
					{
						var (dx, dy) = directions[c];
						x += dx;
						y += dy;

						if (distances.ContainsKey((x, y)))
							distances[(x, y)] = Math.Min(distances[(x, y)], distances[(prevX, prevY)] + 1);
						else
							distances[(x, y)] = distances.GetValueOrDefault((prevX, prevY), 0) + 1;
						break;
					}
				}

				(prevX, prevY) = (x, y);
			}

			return distances.Values.Count(dist => dist >= 1000).ToString();
		}
	}
}