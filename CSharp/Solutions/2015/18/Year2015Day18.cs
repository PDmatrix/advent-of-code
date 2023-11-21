using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2015._18;

[UsedImplicitly]
public class Year2015Day18 : ISolution
{
	public object Part1(IEnumerable<string> lines)
	{
		var state = GetState(lines);
		for (var i = 0; i < 100; i++)
			state = Step(state);
			
		return state.Select(r => r.Count(x => x)).Sum().ToString();
	}

	private static List<List<bool>> Step(IReadOnlyList<List<bool>> state)
	{
		var newState = new List<List<bool>>();
		for (var rowIdx = 0; rowIdx < state.Count; rowIdx++)
		{
			var newRow = new List<bool>();
			for (var colIdx = 0; colIdx < state[rowIdx].Count; colIdx++)
			{
				var neighborsCount = GetNeighborsCount(rowIdx, colIdx, state);
				if (state[rowIdx][colIdx])
					newRow.Add(neighborsCount is 2 or 3);
				else
					newRow.Add(neighborsCount == 3);
			}
			newState.Add(newRow);
		}

		return newState;
	}

	private static int GetNeighborsCount(int rowIdx, int colIdx, IReadOnlyList<List<bool>> state)
	{
		var res = 0;
		for (var i = rowIdx - 1; i <= rowIdx + 1; i++)
		{
			for (var j = colIdx - 1; j <= colIdx + 1; j++)
			{
				if (i == rowIdx && j == colIdx)
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
		return lines.Select(line => line.Select(light => light == '#').ToList()).ToList();
	}

	private static void Stuck(IReadOnlyList<List<bool>> state)
	{
		state[0][0] = true;
		state[0][99] = true;
		state[99][0] = true;
		state[99][99] = true;
	}
		
	public object Part2(IEnumerable<string> lines)
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