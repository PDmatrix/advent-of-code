using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._7;

[UsedImplicitly]
public class Year2024Day07 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var dict = new Dictionary<long, List<long>>();

		foreach (var line in input)
		{
			var parts = line.Split(": ");
			var key = long.Parse(parts[0]);
			dict[key] = parts[1].Split(" ").Select(long.Parse).ToList();
		}

		long answer = 0;
		foreach (var kv in dict)
		{
			var queue = new Queue<(long, int)>();
			queue.Enqueue((0, 0));
			
			while (queue.Count > 0)
			{
				var (sum, idx) = queue.Dequeue();
				
				if (idx == kv.Value.Count)
				{
					if (sum == kv.Key)
					{
						answer += sum;
						break;
					}

					continue;
				}
				
				queue.Enqueue((sum + kv.Value[idx], idx + 1));
				queue.Enqueue((sum == 0 ? kv.Value[idx] : sum * kv.Value[idx], idx + 1));
			}
		}
		
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var dict = new Dictionary<long, List<long>>();

		foreach (var line in input)
		{
			var parts = line.Split(": ");
			var key = long.Parse(parts[0]);
			dict[key] = parts[1].Split(" ").Select(long.Parse).ToList();
		}

		long answer = 0;
		foreach (var kv in dict)
		{
			var queue = new Queue<(long, int)>();
			queue.Enqueue((0, 0));
			
			while (queue.Count > 0)
			{
				var (sum, idx) = queue.Dequeue();
				
				if (idx == kv.Value.Count)
				{
					if (sum == kv.Key)
					{
						answer += sum;
						break;
					}

					continue;
				}
				
				queue.Enqueue((sum + kv.Value[idx], idx + 1));
				queue.Enqueue((sum == 0 ? kv.Value[idx] : sum * kv.Value[idx], idx + 1));
				queue.Enqueue((sum == 0 ? kv.Value[idx] : long.Parse(sum.ToString() + kv.Value[idx].ToString()), idx + 1));
			}
		}
		
		return answer;
	}
}