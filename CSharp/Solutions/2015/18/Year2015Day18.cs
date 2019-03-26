using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._18
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day18 : ISolution
	{
		// ReSharper disable once UnusedMember.Global
		public static int Year = 2015;
		// ReSharper disable once UnusedMember.Global
		public static int Day = 18;
				
		public string Part1(IEnumerable<string> lines)
		{
			var state = GetState(lines);
			for (var i = 0; i < 100; i++)
			{
				state = Step(state);
			}
			
			return state.Select(r => r.Count(x => x)).Sum().ToString();
		}

		private static List<List<bool>> Step(List<List<bool>> state)
		{
			var newState = new List<List<bool>>();
			for (var rowIdx = 0; rowIdx < state.Count; rowIdx++)
			{
				var newRow = new List<bool>();
				for (var colIdx = 0; colIdx < state[rowIdx].Count; colIdx++)
				{
					var neighborsCount = GetNeighborsCount(rowIdx, colIdx, state);
					if (state[rowIdx][colIdx])
					{
						newRow.Add(neighborsCount == 2 || neighborsCount == 3);
					}
					else
					{
						newRow.Add(neighborsCount == 3);
					}
				}
				newState.Add(newRow);
			}

			return newState;
		}

		private static int GetNeighborsCount(int rowIdx, int colIdx, List<List<bool>> state)
		{
			var res = 0;
			for (var i = rowIdx - 1; i <= rowIdx + 1; i++)
			{
				for (var j = colIdx - 1; j <= colIdx + 1; j++)
				{
					if(i == rowIdx && j == colIdx)
						continue;
					
					res += GetNeighbor(i, j, state);
				}
			}

			return res;
		}

		private static int GetNeighbor(int row, int col, IReadOnlyList<List<bool>> state)
		{
			try
			{
				return state[row][col] ? 1 : 0;
			}
			catch
			{
				return 0;
			}
		}

		private static List<List<bool>> GetState(IEnumerable<string> lines)
		{
			var state = new List<List<bool>>();
			foreach (var line in lines)
			{
				var row = new List<bool>();
				foreach (var light in line)
				{
					row.Add(light == '#');
				}
				state.Add(row);
			}

			return state;
		}

		private static void Stuck(IReadOnlyList<List<bool>> state)
		{
			state[0][0] = true;
			state[0][99] = true;
			state[99][0] = true;
			state[99][99] = true;
		}
		
		public string Part2(IEnumerable<string> lines)
		{
			var state = GetState(lines);
			Stuck(state);
			for (var i = 0; i < 100; i++)
			{
				state = Step(state);
				Stuck(state);
			}
			
			return state.Select(r => r.Count(x => x)).Sum().ToString();
		}
	}
}