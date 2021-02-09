using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._16
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day16 : ISolution
	{
		private char[] _state = null!;
		
		public object Part1(IEnumerable<string> input)
		{
			const string start = "abcdefghijklmnop";
			_state = start.ToCharArray();
			
			return Dance(input.First().Split(','));
		}

		private string Dance(IEnumerable<string> moves)
		{
			var regex = new Regex(@"s(?<by>\d+)|x(?<f>\d+)\/(?<s>\d+)|p(?<f>.?)\/(?<s>.?)",
				RegexOptions.Compiled);
			foreach (var move in moves)
			{
				var match = regex.Match(move);
				switch(match.Value[0])
				{
					case 's':
						var value = int.Parse(match.Groups["by"].Value);
						Spin(value);
						break;
					case 'x':
						var firstVal = int.Parse(match.Groups["f"].Value);
						var secondVal = int.Parse(match.Groups["s"].Value);
						Exchange(firstVal, secondVal);
						break;
					case 'p':
						var firstIdx = Array.IndexOf(_state, match.Groups["f"].Value[0]);
						var secondIdx = Array.IndexOf(_state, match.Groups["s"].Value[0]);
						Exchange(firstIdx, secondIdx);
						break;
				}
			}

			return string.Join(string.Empty, _state);
		}

		private void Exchange(int firstIdx, int secondIdx)
		{
			var tmp = _state[firstIdx];
			_state[firstIdx] = _state[secondIdx];
			_state[secondIdx] = tmp;
		}

		private void Spin(int by)
		{
			var b = _state.Clone() as char[] ?? throw new InvalidCastException();
			for (var i = 0; i < _state.Length; i++)
			{
				var idx = (i + by) % _state.Length;
				_state[idx] = b[i];
			}
		}

		public object Part2(IEnumerable<string> input)
		{
			const string start = "abcdefghijklmnop";
			_state = start.ToCharArray();
			var repetitions = 1_000_000_000;
			var cur = start;
			var moves = input.First().Split(',');
			for (var i = 0; i < repetitions; i++)
			{
				cur = Dance(moves);
				if (cur != start) 
					continue;
				
				var cycle = i+1;
				var extras = repetitions % cycle;
				i = 0;
				repetitions = extras+1;
			}

			return cur;
		}
	}
}