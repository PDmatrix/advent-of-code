using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._5;

[UsedImplicitly]
public class Year2023Day05 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var seeds = input.First().Split(": ")[1].Split().Select(long.Parse).ToList();
		var maps = ParseMaps(input.Skip(2));
		
		var mappingOrder = new List<string>
		{
			"seed-to-soil",
			"soil-to-fertilizer",
			"fertilizer-to-water",
			"water-to-light",
			"light-to-temperature",
			"temperature-to-humidity",
			"humidity-to-location"
		};

		var locations = new List<long>();
		foreach (var seed in seeds)
		{
			var current = seed;
			foreach (var mapping in mappingOrder)
			{
				var suitableMap = maps[mapping].SingleOrDefault(x => x.Source <= current && x.Source + x.Count > current, new Map(0, 0, 0));
				current = suitableMap.Dest + (current - suitableMap.Source);
			}
			locations.Add(current);
		}
		
		return locations.Min();
	}


	public object Part2(IEnumerable<string> input)
	{
		var seeds = input.First().Split(": ")[1].Split().Select(long.Parse).ToList();
		var maps = ParseMaps(input.Skip(2));
		
		var mappingOrder = new List<string>
		{
			"humidity-to-location",
			"temperature-to-humidity",
			"light-to-temperature",
			"water-to-light",
			"fertilizer-to-water",
			"soil-to-fertilizer",
			"seed-to-soil",
		};

		long location = 0;
		long origLocation = 0;
		while (true)
		{
			location = origLocation + 1;
			origLocation = location;
			
			foreach (var mapping in mappingOrder)
			{
				var suitableMap = maps[mapping].SingleOrDefault(x => x.Dest <= location && x.Dest + x.Count > location, new Map(0, 0, 0));
				location = suitableMap.Source + (location - suitableMap.Dest);
			}

			for (var i = 0; i < seeds.Count - 1; i += 2)
			{
				var from = seeds[i];
				var count = seeds[i + 1];
				if (location >= from && location < from + count)
					return origLocation;
			}
		}
	}
	
	private static Dictionary<string, List<Map>> ParseMaps(IEnumerable<string> input)
	{
		var currentMap = "";
		var maps = new Dictionary<string, List<Map>>();
		foreach (var line in input)
		{
			if (line == "")
				continue;

			if (line.Contains("map"))
			{
				currentMap = line.Split()[0];
				continue;
			}
			
			if (!maps.ContainsKey(currentMap))
				maps[currentMap] = new List<Map>();
			
			var parts = line.Split().Select(long.Parse).ToList();
			maps[currentMap].Add(new Map(parts[0], parts[1], parts[2]));
		}
		
		return maps;
	}

	private record struct Map(long Dest, long Source, long Count);
}