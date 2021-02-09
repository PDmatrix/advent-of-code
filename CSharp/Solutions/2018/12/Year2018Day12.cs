using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._12
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day12 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var inputList = input.ToList();
			var rawInitialState = Regex.Match(inputList.First(), @": (.+)").Groups[1].Value;
			var state = new Dictionary<int, char>();
			for (var i = 0; i < rawInitialState.Length; i++)
				state[i] = rawInitialState[i];

			var notes = new Dictionary<string, char>();
			foreach (var note in inputList.Skip(2))
			{
				var groups = Regex.Match(note, @"(.+) => (.)").Groups;
				notes[groups[1].Value] = groups[2].Value.First();
			}

			for (var i = 0; i < 20; i++)
			{
				var indices = state.Keys.ToList();
				var intermediateState = state.ToDictionary(key => key.Key, value => value.Value);
				for (var j = indices.Min() - 3; j < indices.Max() + 3; j++)
				{
					var cur = GetCurrentPot(intermediateState, j);
					state[j] = notes.GetValueOrDefault(cur, '.');
				}
			}

			return state.Sum(x => x.Value == '#' ? x.Key : 0).ToString();
		}

		private static string GetCurrentPot(IReadOnlyDictionary<int, char> state, int index)
		{
			var sb = new StringBuilder();
			for (var i = index - 2; i <= index + 2; i++)
				sb.Append(state.GetValueOrDefault(i, '.'));

			return sb.ToString();
		}

		public object Part2(IEnumerable<string> input)
		{
			var inputList = input.ToList();
			var rawInitialState = Regex.Match(inputList.First(), @": (.+)").Groups[1].Value;
			var state = new Dictionary<int, char>();
			for (var i = 0; i < rawInitialState.Length; i++)
				state[i] = rawInitialState[i];

			var notes = new Dictionary<string, char>();
			foreach (var note in inputList.Skip(2))
			{
				var groups = Regex.Match(note, @"(.+) => (.)").Groups;
				notes[groups[1].Value] = groups[2].Value.First();
			}

			var lastSum = 0;
			for (var i = 0; i < 2000; i++)
			{
				lastSum = state.Sum(x => x.Value == '#' ? x.Key : 0);
				var indices = state.Where(x => x.Value == '#').ToList();
				var intermediateState = new Dictionary<int, char>(state);
				for (var j = indices.Min(x => x.Key) - 3; j < indices.Max(x => x.Key) + 3; j++)
				{
					var cur = GetCurrentPot(intermediateState, j);
					state[j] = notes.GetValueOrDefault(cur, '.');
				}
			}

			var currentSum = state.Sum(x => x.Value == '#' ? x.Key : 0);

			return ((50000000000 - 2000) * (currentSum - lastSum) + currentSum).ToString();
		}
	}
}