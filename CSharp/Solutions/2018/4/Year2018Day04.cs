using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._4
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day04 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var records = input.ToList();
			records.Sort((a, b) => string.CompareOrdinal(a[..17], b[..17]));

			var states = GenerateGuardState(records);

			var (id, sleepyMinutes) =
				states.Aggregate((a, b) => a.Value.Sum(x => x.Value) > b.Value.Sum(x => x.Value) ? a : b);

			return (int.Parse(id.TrimStart('#')) *
			        sleepyMinutes.Aggregate((a, b) => a.Value > b.Value ? a : b).Key).ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var records = input.ToList();
			records.Sort((a, b) => string.CompareOrdinal(a[..17], b[..17]));

			var states = GenerateGuardState(records);

			var (id, sleepyMinutes) =
				states.Aggregate((a, b) => a.Value.Max(x => x.Value) > b.Value.Max(x => x.Value) ? a : b);

			return (int.Parse(id.TrimStart('#')) *
			        sleepyMinutes.Aggregate((a, b) => a.Value > b.Value ? a : b).Key).ToString();
		}

		private static Dictionary<string, Dictionary<int, int>> GenerateGuardState(IEnumerable<string> records)
		{
			var states = new Dictionary<string, Dictionary<int, int>>();
			var currentGuard = string.Empty;
			var fallTime = 0;
			foreach (var record in records)
			{
				var idMatch = Regex.Match(record, @"#\d+");
				if (idMatch.Success)
				{
					currentGuard = idMatch.Value;
					continue;
				}

				var fallsMatch = Regex.Match(record, @"falls");
				if (fallsMatch.Success)
				{
					fallTime = int.Parse(Regex.Match(record, @":(\d+)").Groups[1].Value);
					continue;
				}

				if (!states.ContainsKey(currentGuard))
					states[currentGuard] = new Dictionary<int, int>();

				var wakeUpTime = int.Parse(Regex.Match(record, @":(\d+)").Groups[1].Value);
				for (var i = fallTime; i < wakeUpTime; i++)
					states[currentGuard][i] = states[currentGuard].GetValueOrDefault(i) + 1;
			}

			return states;
		}
	}
}