using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._16;
[UsedImplicitly]

public partial class Year2022Day16 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var valves = ParseInput(input);
		var startValve = valves["AA"];
		var maxPressure = GetMaxPressure(valves, startValve, 30);
		return maxPressure;
	}

	private int GetMaxPressure(Dictionary<string, Valve> valves, Valve startValve, int maxMinutes)
	{
		var queue = new Queue<(Valve valve, int minute, int pressure, HashSet<Valve> opened)>();
		var visited = new Dictionary<(Valve, int), int>();
		queue.Enqueue((startValve, 1, 0, new HashSet<Valve>()));
		var maxPressure = 0;

		while (queue.Count > 0)
		{
			var (valve, minute, pressure, opened) = queue.Dequeue();
			
			if (visited.TryGetValue((valve, minute), out var value) && value >= pressure)
				continue;
			
			visited[(valve, minute)] = pressure;
			
			if (minute == maxMinutes)
			{
				maxPressure = Math.Max(maxPressure, pressure);
				continue;
			}

			// if we open the valve here
			if (valve.FlowRate > 0 && !opened.Contains(valve))
			{
				var newOpened = new HashSet<Valve>(opened) { valve };
				var newPressure = pressure + newOpened.Sum(x => x.FlowRate);
				
				queue.Enqueue((valve, minute + 1, newPressure, newOpened));
			}

			var oldOpened = new HashSet<Valve>(opened);
			var nextPressure = pressure + oldOpened.Sum(x => x.FlowRate);
			
			// if we don't open a valve here
			foreach (var connectedValveName in valve.ConnectedValves)
			{
				var nextValve = valves[connectedValveName];
				queue.Enqueue((nextValve, minute + 1, nextPressure, oldOpened));
			}
		}

		return maxPressure;
	}
	

	public object Part2(IEnumerable<string> input)
	{
		var valves = ParseInput(input);
		var startValve = valves["AA"];
		var maxPressure = GetMaxPressureWithElephant(valves, startValve, 26);
		return maxPressure;
	}
	
	private int GetMaxPressureWithElephant(Dictionary<string, Valve> valves, Valve startValve, int maxMinutes)
	{
		var queue = new Queue<(Valve valve, Valve elephant, int minute, int pressure, HashSet<Valve> opened)>();
		var visited = new Dictionary<(Valve, Valve, int), int>();
		queue.Enqueue((startValve, startValve, 1, 0, new HashSet<Valve>()));
		var maxPressure = 0;
		var maxFlow = valves.Values.Sum(x => x.FlowRate);

		while (queue.Count > 0)
		{
			var (valve, elephant, minute, pressure, opened) = queue.Dequeue();

			if (visited.TryGetValue((valve, elephant, minute), out var value) && value >= pressure)
				continue;
			
			visited[(valve, elephant, minute)] = pressure;
			
			if (minute == maxMinutes)
			{
				maxPressure = Math.Max(maxPressure, pressure);
				continue;
			}
			
			var currentFlow = opened.Sum(x => x.FlowRate);
			
			if (currentFlow >= maxFlow)
			{
				var newPressure = pressure + currentFlow;
				while (minute < maxMinutes - 1)
				{
					minute++;
					newPressure += currentFlow;
				}
				queue.Enqueue((valve, elephant, minute + 1, newPressure, opened));
				continue;
			}
			
			// case 1: we open a valve here
			if (valve.FlowRate > 0 && !opened.Contains(valve))
			{
				var newOpened = new HashSet<Valve>(opened) { valve };
				
				// case 1A: and the elephant open its valve too!
				if (elephant.FlowRate > 0 && !newOpened.Contains(elephant))
				{
					var nnOpened = new HashSet<Valve>(newOpened) { elephant };
					
					var nnPressure = pressure + nnOpened.Sum(x => x.FlowRate);
					queue.Enqueue((valve, elephant, minute + 1, nnPressure, nnOpened));
				}
				
				// case 1B: the elephant goes somewhere
				var newPressure = pressure + newOpened.Sum(x => x.FlowRate);
				foreach (var option in elephant.ConnectedValves)
				{
					var nextValve = valves[option];
					queue.Enqueue((valve, nextValve, minute + 1, newPressure, newOpened));
				}
			}
			
			// case 2: we go somewhere else
			foreach (var option in valve.ConnectedValves)
			{
				var nextValve = valves[option];
				// case 2A: and the elephant open its valve!
				if (elephant.FlowRate > 0 && !opened.Contains(elephant))
				{
					var nnOpened = new HashSet<Valve>(opened) { elephant };
					
					var nnPressure = pressure + nnOpened.Sum(x => x.FlowRate);
					queue.Enqueue((nextValve, elephant, minute + 1, nnPressure, nnOpened));
				}
				
				// case 2B: and the elephant goes somewhere
				var oldOpened = new HashSet<Valve>(opened);
				var newPressure = pressure + oldOpened.Sum(x => x.FlowRate);
				foreach (var optionElephant in elephant.ConnectedValves)
				{
					var nextElephant = valves[optionElephant];
					queue.Enqueue((nextValve, nextElephant, minute + 1, newPressure, oldOpened));
				}
			}
		}

		return maxPressure;
	}

	private static Dictionary<string, Valve> ParseInput(IEnumerable<string> input)
	{
		var dict = new Dictionary<string, Valve>();
		
		var regex = InputRegex();

		foreach (var line in input)
		{
			var match = regex.Match(line);
			dict[match.Groups["name"].Value] = new Valve(match.Groups["name"].Value, int.Parse(match.Groups["flowRate"].Value),
				match.Groups["connectedValves"].Value.Split(", ").ToList());
		}

		return dict;
	}

	private record struct Valve(string Name, int FlowRate, List<string> ConnectedValves);

	[GeneratedRegex(
		@"Valve (?<name>\w+) has flow rate=(?<flowRate>\d+); tunnels? leads? to valves? (?<connectedValves>.+)")]
	private static partial Regex InputRegex();
}
