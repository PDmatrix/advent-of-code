using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._1;

[UsedImplicitly]
public class Year2021Day01 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var measurements = input.Select(int.Parse).ToList();
		var currentMeasurement = measurements.First();
		var increasing = 0;
		foreach (var measurement in measurements.Skip(1))
		{
			increasing += measurement > currentMeasurement ? 1 : 0;
			currentMeasurement = measurement;
		}
		
		return increasing;
	}

	public object Part2(IEnumerable<string> input)
	{
		var measurements = input.Select(int.Parse).ToList();
		var sumMeasurements = new List<int>();
		for (var i = 0; i <= measurements.Count - 3; i++)
			sumMeasurements.Add(measurements.Skip(i).Take(3).Sum());
		
		var currentSumMeasurement = sumMeasurements.First();
		var increasing = 0;
		foreach (var sumMeasurement in sumMeasurements.Skip(1))
		{
			increasing += sumMeasurement > currentSumMeasurement ? 1 : 0;
			currentSumMeasurement = sumMeasurement;
		}
		
		return increasing;
	}
}