using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._7
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day07 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var state = FillState(input);

			var res = new StringBuilder();
			while (state.Sum(x => x.Value.Count) >= 1)
			{
				var notDependencies =
					state.Where(x => x.Value.Count == 0).ToList();

				notDependencies.Sort((a, b) => string.CompareOrdinal(a.Key.ToString(), b.Key.ToString()));

				var removed = notDependencies.First().Key;
				res.Append(removed);
				state.Remove(removed);
				var keys = new List<char>(state.Keys);
				foreach (var key in keys)
					state[key].Remove(removed);
			}

			var remainingKeys = state.Keys.ToList();
			remainingKeys.Sort((a, b) => string.CompareOrdinal(a.ToString(), b.ToString()));
			
			res.Append(string.Join(string.Empty, remainingKeys));

			return res.ToString();
		}

		private static Dictionary<char, List<char>> FillState(IEnumerable<string> input)
		{
			var state = new Dictionary<char, List<char>>();
			foreach (var instruction in input)
			{
				var mainStep = char.Parse(Regex.Match(instruction, @"step (\w*) ").Groups[1].Value);
				var dependsStep = char.Parse(Regex.Match(instruction, @"Step (\w*)").Groups[1].Value);

				if (state.ContainsKey(mainStep))
					state[mainStep].Add(dependsStep);
				else
					state[mainStep] = new List<char> {dependsStep};

				if (!state.ContainsKey(dependsStep))
					state[dependsStep] = new List<char>();
			}

			return state;
		}

		public string Part2(IEnumerable<string> input)
		{
			var state = FillState(input);
			const int numWorkers = 5;
			const int timeToFinish = 60;

			var workers = new List<Worker>();
			for (var i = 0; i < numWorkers; i++)
				workers.Add(new Worker
				{
					Processing = '\0',
					Time = -1
				});
			
			var count = 0;
			var processed = new List<char>();
			while (true)
			{
				var notDependencies =
					state.Where(x => x.Value.Count == 0 && !processed.Contains(x.Key)).ToList();

				foreach (var worker in
					workers.Where(worker => worker.Time <= 0 && notDependencies.Count > 0))
				{
					worker.Processing = notDependencies.First().Key;
					worker.Time = worker.Processing - 64 + timeToFinish;
					notDependencies.RemoveAt(0);
					processed.Add(worker.Processing);
				}
				
				if (workers.Count(x => x.Time != -1) == 0)
					break;

				var minAmount = workers.Where(x => x.Time != -1).Min(x => x.Time);
				count += minAmount;
				foreach (var worker in workers.Where(worker => worker.Time >= minAmount))
				{
					worker.Time -= minAmount;
					if (worker.Time > 0) 
						continue;
					
					worker.Time = -1;
					state.Remove(worker.Processing);
					var keys = new List<char>(state.Keys);
					foreach (var key in keys)
						state[key].Remove(worker.Processing);
				}
			}

			return count.ToString();
		}
		
		private class Worker
		{
			public char Processing { get; set; }
			public int Time { get; set; }
		}
	}
}