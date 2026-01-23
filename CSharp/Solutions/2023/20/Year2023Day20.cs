using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2023._20;

[UsedImplicitly]
public class Year2023Day20 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var (connections, modules) = ParseInput(input);
		foreach (var connection in connections)
		{
			foreach (var module in connection.Value)
			{
				if (modules.TryGetValue(module, out var value) && value is ConjunctionModule conjunctionModule)
				{
					conjunctionModule?.Pulses.TryAdd(connection.Key, Pulse.Low);
				}
			}
		}
		
		var queue = new Queue<(string module, Pulse pulse, string from)>();
		var lowPulses = 0;
		var highPulses = 0;
		for (var i = 0; i < 1000; i++)
		{
			lowPulses++;
			queue.Enqueue(("broadcaster", Pulse.Low, ""));

			while (queue.Count > 0)
			{
				var (moduleName, pulse, from) = queue.Dequeue();
				if (!modules.TryGetValue(moduleName, out var module))
					continue;

				var outputPulse = module.Process(pulse, from);
				if (outputPulse is Pulse.None)
					continue;

				foreach (var to in connections[moduleName])
				{
					if (outputPulse is Pulse.Low)
						lowPulses++;
					else if (outputPulse is Pulse.High)
						highPulses++;
					
					queue.Enqueue((to, outputPulse, moduleName));
				}
			}
		}

		return lowPulses * highPulses;
	}
	
	public object Part2(IEnumerable<string> input)
	{
		var (connections, modules) = ParseInput(input);
		var connectedToRx = "";
		foreach (var connection in connections)
		{
			foreach (var module in connection.Value)
			{
				if (modules.TryGetValue(module, out var value) && value is ConjunctionModule conjunctionModule)
				{
					conjunctionModule?.Pulses.TryAdd(connection.Key, Pulse.Low);
				}

				if (module == "rx")
					connectedToRx += connection.Key;
			}
		}
		
		var connectedModulesToRx = (modules[connectedToRx] as ConjunctionModule).Pulses.Keys.ToList();
		var hs = new Dictionary<string, int>();
		
		var queue = new Queue<(string module, Pulse pulse, string from)>();
		var presses = 0;
		while (true)
		{
			presses++;
			queue.Enqueue(("broadcaster", Pulse.Low, ""));

			while (queue.Count > 0)
			{
				var (moduleName, pulse, from) = queue.Dequeue();
				if (moduleName == "rx" && pulse is Pulse.Low)
					return presses;
				
				if (!modules.TryGetValue(moduleName, out var module))
					continue;

				var outputPulse = module.Process(pulse, from);
				if (outputPulse is Pulse.None)
					continue;
				
				if (connectedModulesToRx.Contains(moduleName) && outputPulse is Pulse.High)
				{
					hs[moduleName] = presses;
					if (hs.Count == connectedModulesToRx.Count)
						return LCM(hs.Values.Select(x => (long)x).ToArray());
				}

				foreach (var to in connections[moduleName])
					queue.Enqueue((to, outputPulse, moduleName));
			}
		}

		return presses;
	}
	
	private static long LCM(long[] numbers)
	{
		return numbers.Aggregate(lcm);
	}
	private static long lcm(long a, long b)
	{
		return Math.Abs(a * b) / GCD(a, b);
	}
	private static long GCD(long a, long b)
	{
		return b == 0 ? a : GCD(b, a % b);
	}

	private static (Dictionary<string, List<string>>, Dictionary<string, IModule>) ParseInput(IEnumerable<string> input)
	{
		var modules = new Dictionary<string, IModule>();
		var connections = new Dictionary<string, List<string>>();
		foreach (var line in input)
		{
			var parts = line.Split("->", StringSplitOptions.TrimEntries);
			var type = parts[0][0];
			var from = parts[0][1..].Trim();
			if (type == 'b')
				from = "broadcaster";
			
			var toList = parts[1].Split(',', StringSplitOptions.TrimEntries);
			
			IModule module = type switch
			{
				'%' => new FlipFlopModule(),
				'&' => new ConjunctionModule(),
				_ => new BroadcasterModule()
			};
			connections[from] = toList.ToList();
			modules[from] = module;
		}
		
		return (connections, modules);
	}
	
	private interface IModule
	{
		Pulse Process(Pulse input, string from);
	}

	private class FlipFlopModule : IModule
	{
		private bool State { get; set; }
		
		public Pulse Process(Pulse input, string _)
		{
			if (input is Pulse.None or Pulse.High)
				return Pulse.None;

			State = !State;
			
			return State ? Pulse.High : Pulse.Low;
		}
	}
	
	private class ConjunctionModule : IModule
	{
		public Dictionary<string, Pulse> Pulses { get; } = new();
		
		public Pulse Process(Pulse input, string from)
		{
			if (input is Pulse.None)
				return Pulse.None;
			
			Pulses[from] = input;

			return Pulses.Values.All(p => p is Pulse.High) ? Pulse.Low : Pulse.High;
		}
	}
	
	private class BroadcasterModule : IModule
	{
		public Pulse Process(Pulse input, string _) => input;
	}
	
	private enum Pulse
	{
		High,
		Low,
		None
	}
}