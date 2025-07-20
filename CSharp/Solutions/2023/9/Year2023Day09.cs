using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._9;

[UsedImplicitly]
public class Year2023Day09 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var history = GetInput(input);
		return history.Sum(Extrapolate);
	}

	public object Part2(IEnumerable<string> input)
	{
		var history = GetInput(input);

		return history.Sum(ExtrapolateBackwards);
	}

	private static int Extrapolate(List<int> nums)
	{
		var differences = new List<int>();
		for (var i = 0; i < nums.Count - 1; i++)
			differences.Add(nums[i + 1] - nums[i]);

		if (differences.All(difference => difference == 0))
			return nums.First();
		
		return Extrapolate(differences) + nums.Last();
	}
	
	private static int ExtrapolateBackwards(List<int> nums)
	{
		var differences = new List<int>();
		for (var i = 0; i < nums.Count - 1; i++)
			differences.Add(nums[i + 1] - nums[i]);

		if (differences.All(difference => difference == 0))
			return nums.First();
		
		return nums.First() - ExtrapolateBackwards(differences);
	}

	private static List<List<int>> GetInput(IEnumerable<string> input)
	{
		return input.Select(line => line.Split().Select(int.Parse).ToList()).ToList();
	}
}