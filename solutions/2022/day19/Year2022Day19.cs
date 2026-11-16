using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using CSharpx;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._19;

[UsedImplicitly]
public partial class Year2022Day19 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var blueprints = ParseInput(input);

		var blueprintId = 1;
		var answer = 0;
		
		foreach (var blueprint in blueprints)
		{
			var maxGeodes = RunBlueprint(blueprint, 25);

			answer += blueprintId * maxGeodes;
			blueprintId++;
		}

		return answer;
	}

	private static List<Dictionary<string, List<(int Cost, string Resource)>>> ParseInput(IEnumerable<string> input)
	{
		var blueprints = new List<Dictionary<string, List<(int Cost, string Resource)>>>();
		var r = new Regex(
			@"Each (?<robot>\w+) robot costs (?<cost1>\d+) (?<resource1>\w+) ?(and)? ?(?<cost2>\d+)? ?(?<resource2>\w+)?");
		foreach (var line in input)
		{
			var matches = r.Matches(line);
			var d = new Dictionary<string, List<(int Cost, string Resource)>>();
			foreach (Match m in matches)
			{
				var l = new List<(int Cost, string Resource)>
					{ (int.Parse(m.Groups["cost1"].Value), m.Groups["resource1"].Value) };
				if (m.Groups["cost2"].Success)
					l.Add((int.Parse(m.Groups["cost2"].Value), m.Groups["resource2"].Value));
				
				d.Add(m.Groups["robot"].Value, l);
			}
			
			blueprints.Add(d);
		}

		return blueprints;
	}

	public object Part2(IEnumerable<string> input)
	{
		var blueprints = ParseInput(input).Take(3).ToList();

		var answer = 1;
		
		foreach (var blueprint in blueprints)
		{
			var maxGeodes = RunBlueprint(blueprint, 33);
			answer *= maxGeodes;
		}

		return answer;
	}

	private static int RunBlueprint(Dictionary<string, List<(int Cost, string Resource)>> blueprint, int maxMinutes)
	{
		var cache = new HashSet<(Robots robots, Resources resources, int minutes)>();
		var robotOrder = new[] { "geode", "obsidian", "clay", "ore" };
		
		var maxGeodes = 0;
		var q = new Queue<(int Minutes, Robots robots, Resources resources)>();
		q.Enqueue((1, new Robots(1, 0, 0, 0), new Resources(0, 0, 0, 0)));

		var maxOre = 0;
		var maxClay = 0;
		var maxObsidian = 0;
		foreach (var (_, cost) in blueprint)
		{
			foreach (var c in cost)
			{
				if (c.Resource == "ore")
					maxOre = Math.Max(maxOre, c.Cost);
				if (c.Resource == "clay")
					maxClay = Math.Max(maxClay, c.Cost);
				if (c.Resource == "obsidian")
					maxObsidian = Math.Max(maxObsidian, c.Cost);
			}
		}

		while (q.Count != 0)
		{
			var (minutes, robots, resources) = q.Dequeue();
			var remaining = maxMinutes - minutes;
				
			if (minutes == maxMinutes)
			{
				maxGeodes = Math.Max(maxGeodes, resources.Geode);
				continue;
			}

			if (resources.Ore >= remaining * maxOre - robots.OreRobot * (remaining - 1))
				resources.Ore = remaining * maxOre - robots.OreRobot * (remaining - 1);
			if (resources.Clay >= remaining * maxClay - robots.ClayRobot * (remaining - 1))
				resources.Clay = remaining * maxClay - robots.ClayRobot * (remaining - 1);
			if (resources.Obsidian >= remaining * maxObsidian - robots.ObsidianRobot * (remaining - 1))
				resources.Obsidian = remaining * maxObsidian - robots.ObsidianRobot * (remaining - 1);
			
			if (!cache.Add((robots, resources, minutes)))
				continue;

			foreach (var robot in robotOrder)
			{
				switch (robot)
				{
					case "ore" when robots.OreRobot >= maxOre:
					case "clay" when robots.ClayRobot >= maxClay:
					case "obsidian" when robots.ObsidianRobot >= maxObsidian:
						continue;
				}

				var costs = blueprint[robot];
				var canAfford = true;
				foreach (var (cost, resource) in costs)
				{
					if (GetResource(resources, resource) >= cost)
						continue;

					canAfford = false;
					break;
				}

				if (!canAfford)
					continue;

				var newResources = resources;
				foreach (var (cost, resource) in costs)
				{
					newResources = RemoveResource(newResources, resource, cost);
				}

				newResources = Mine(newResources, robots);

				q.Enqueue((minutes + 1, AddRobot(robots, robot), newResources));
			}
			
			q.Enqueue((minutes + 1, robots, Mine(resources, robots)));
		}

		return maxGeodes;
	}

	private record struct Robots(int OreRobot, int ClayRobot, int ObsidianRobot, int GeodeRobot);
	private record struct Resources(int Ore, int Clay, int Obsidian, int Geode);
	
	private static int GetResource(Resources resources, string resource)
	{
		return resource switch
		{
			"ore" => resources.Ore,
			"clay" => resources.Clay,
			"obsidian" => resources.Obsidian,
			"geode" => resources.Geode,
			_ => throw new ArgumentOutOfRangeException(nameof(resource))
		};
	}
	
	private static Resources RemoveResource(Resources resources, string resource, int amount)
	{
		switch (resource)
		{
			case "ore":
				resources.Ore -= amount;
				break;
			case "clay":
				resources.Clay -= amount;
				break;
			case "obsidian":
				resources.Obsidian -= amount;
				break;
			case "geode":
				resources.Geode -= amount;
				break;
		}

		return resources;
	}

	private static Robots AddRobot(Robots robots, string robot)
	{
		switch (robot)
		{
			case "ore":
				robots.OreRobot++;
				break;
			case "clay":
				robots.ClayRobot++;
				break;
			case "obsidian":
				robots.ObsidianRobot++;
				break;
			case "geode":
				robots.GeodeRobot++;
				break;
		}

		return robots;
	}

	private static Resources Mine(Resources resources, Robots robots)
	{
		resources.Ore += robots.OreRobot;
		resources.Clay += robots.ClayRobot;
		resources.Obsidian += robots.ObsidianRobot;
		resources.Geode += robots.GeodeRobot;

		return resources;
	}
	
	private static int OptimisticBest(Resources resources, Robots robots, int timeRemaining, string robot)
	{
		var resource = robot switch
		{
			"ore" => resources.Ore,
			"clay" => resources.Clay,
			"obsidian" => resources.Obsidian,
			"geode" => resources.Geode,
			_ => throw new ArgumentOutOfRangeException(nameof(robot))
		};
		var robotCount = robot switch
		{
			"ore" => robots.OreRobot,
			"clay" => robots.ClayRobot,
			"obsidian" => robots.ObsidianRobot,
			"geode" => robots.GeodeRobot,
			_ => throw new ArgumentOutOfRangeException(nameof(robot))
		};
		
		return resource + robotCount * timeRemaining + timeRemaining * (timeRemaining - 1) / 2;
	}
}