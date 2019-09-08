using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._3
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day03 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var value = int.Parse(input.First());
			var (level, minValue) = GetLevelAndMinValue(value);
			var middleValues = GetMiddleValues(minValue, level).ToArray();
			return (middleValues.Select(r => Math.Abs(r - value)).Min() + level - 1).ToString();
		}

		private static IEnumerable<int> GetMiddleValues(int minValue, int level)
		{
			var baseValue = minValue + level - 2;
			yield return baseValue;
			for (var i = 1; i < 4; i++)
			{
				yield return baseValue + 2 * (level - 1) * i;
			}
		}

		private static (int, int) GetLevelAndMinValue(int value)
		{
			var min = 2;
			var max = 9;
			var level = 2;
			while (!(min <= value && value <= max))
			{
				min = max + 1;
				max = min - 1 + level * 8;
				level++;
			}

			return (level, min);
		}

		public string Part2(IEnumerable<string> input)
		{
			var state = new Dictionary<(int, int), int>
			{
				[(0,0)] = 1,
			};
			var value = int.Parse(input.First());
			const string dir = "RULD";
			var distance = 1;
			var x = 0;
			var y = 0;
			var dirIndex = 0;
			(int, int) Move()
			{
				switch (dir[dirIndex])
				{
					case 'R':
						x++;
						break;
					case 'U':
						y++;
						break;
					case 'L':
						x--;
						break;
					case 'D':
						y--;
						break;
				}

				return (x, y);
			}

			while (true)
			{
				for (var i = 0; i < 2; i++)
				{
					for (var j = 0; j < distance; j++)
					{
						var nextPos = Move();
						var next = GetNextNumber(nextPos, state);
						if(next > value) return next.ToString();
						state[nextPos] = next;
					}

					dirIndex = ++dirIndex % dir.Length;
				}
				distance++;
			}
		}


		private static int GetNextNumber((int x, int y) tuple, IReadOnlyDictionary<(int, int), int> state)
		{
			var list = new List<List<int>>
			{
				new List<int> {1, 0},
				new List<int> {-1, 0},
				new List<int> {0, 1},
				new List<int> {0, -1},
				new List<int> {-1, 1},
				new List<int> {1, 1},
				new List<int> {1, -1},
				new List<int> {-1, -1},
			};
			var (x, y) = tuple;
			return list.Aggregate(0,
				(acc, item) => acc + state.GetValueOrDefault((item[0] + x, item[1] + y), 0));
		}
	}
}