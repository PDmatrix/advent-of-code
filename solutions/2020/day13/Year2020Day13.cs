using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._13;

[UsedImplicitly]
public class Year2020Day13 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var depart = int.Parse(input.First());
		var buses = input.Last().Split(',').Where(x => x != "x").Select(int.Parse).ToList();

		var currentDepart = depart;
		while (true)
		{
			foreach (var bus in buses)
			{
				if (currentDepart % bus == 0)
					return (currentDepart - depart) * bus;
			}

			currentDepart++;
		}
	}

	public object Part2(IEnumerable<string> input)
	{
		var busStrings = input.Last().Split(',');
		var buses = new List<(int bus, int offset)>();
		for (int i = 0; i < busStrings.Length; i++)
		{
			if (busStrings[i] == "x")
				continue;
			
			buses.Add((int.Parse(busStrings[i]), i));
		}

		long index = 0;
		long d = 1;
		foreach (var (bus, offset) in buses)
		{
			while (true)
			{
				index += d;
				if ((index + offset) % bus != 0)
					continue;
				
				d *= bus;
				break;
			}
		}

		return index;
	}
}