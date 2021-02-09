using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._14
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day14 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var key = input.First();
			var grid = GetGrid(key);
			var res = 0;
			for (var x = 0; x < 128; x++)
			{
				for (var y = 0; y < 128; y++)
				{
					if (grid[x, y] == '#')
						res++;
				}
			}

			return res.ToString();
		}

		public object Part2(IEnumerable<string> input)
		{
			var key = input.First();
			var grid = GetGrid(key);
			var regions = 0;
			for (var x = 0; x < 128; x++)
			{
				for (var y = 0; y < 128; y++)
				{
					if(grid[x, y] != '#') continue;

					regions++;
					CleanRegion(grid, x, y);
				}
			}
			
			return regions.ToString();
		}

		private static char[,] GetGrid(string key)
		{
			var grid = new char[128, 128];
			for (var i = 0; i < 128; i++)
			{
				var keyToHash = $"{key}-{i}";
				var hexString = Hash(keyToHash);
				var binaryString = string.Join(string.Empty,
					hexString.Select(
						c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
					)
				);

				for (var j = 0; j < 128; j++)
				{
					grid[i, j] = binaryString[j] == '1' ? '#' : '.';
				}
			}

			return grid;
		}

		private static void CleanRegion(char[,] grid, int x, int y)
		{
			if(grid[x, y] != '#') 
				return;
			
			grid[x, y] = '.';
			
			if(y + 1 < 128) CleanRegion(grid, x, y + 1);
			if(y > 0) CleanRegion(grid, x, y - 1);
			if(x + 1 < 128) CleanRegion(grid, x + 1, y);
			if(x > 0) CleanRegion(grid, x - 1, y);
		}
		
		private static IEnumerable<int> ReverseSection(IList<int> list, int position, int length)
		{
			var newList = new List<int>(list);
			for (var i = 0; i < length; i++)
			{
				newList[(i + position) % newList.Count] =
					list[(position + length - i - 1) % newList.Count];
			}

			return newList;
		}

		private static string Hash(string input)
		{
			var list = Enumerable.Range(0, 256).ToArray();
			var lengths = input.Select(r => (int) r).Concat(new[] {17, 31, 73, 47, 23}).ToArray();
			var currentPosition = 0;
			var skipSize = 0;
			for (var i = 1; i <= 64; i++)
			{
				foreach (var length in lengths)
				{
					list = ReverseSection(list, currentPosition, length).ToArray();
					currentPosition += length + skipSize;
					skipSize++;
				}
			}

			return string.Join(string.Empty,
				list
					.Select((num, idx) => new {num, idx})
					.GroupBy(r => r.idx / 16)
					.Select(r => r.Aggregate(0, (acc, el) => acc ^ el.num))
					.Select(r => r.ToString("X").PadLeft(2, '0'))
			);
		}
	}
}