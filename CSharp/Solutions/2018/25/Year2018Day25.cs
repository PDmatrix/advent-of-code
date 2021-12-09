using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2018._25;

[UsedImplicitly]
public class Year2018Day25 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var arr = input.Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();
		var n = arr.Length;
		var visited = new Dictionary<int, bool>();

		static int Dist(IList<int> a, IList<int> b)
			=> Math.Abs(a[0] - b[0]) + Math.Abs(a[1] - b[1]) + Math.Abs(a[2] - b[2]) + Math.Abs(a[3] - b[3]);

		void Dfs(int index)
		{
			visited.TryAdd(index, true);
			for (var i = 0; i < n; i++)
			{
				if (!visited.GetValueOrDefault(i, false) && Dist(arr[index], arr[i]) <= 3)
				{
					Dfs(i);
				}
			}
		}

		var cnt = 0;
		for (var i = 0; i < n; i++)
		{
			if (visited.GetValueOrDefault(i, false)) 
				continue;
			cnt++;
			Dfs(i);
		}

		return cnt.ToString();
	}

	public object Part2(IEnumerable<string> input)
	{
		return "Congratulations!";
	}
}