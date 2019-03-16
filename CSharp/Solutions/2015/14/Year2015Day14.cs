using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._14
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day14 : ISolution
	{
		// ReSharper disable once UnusedMember.Global
		public static int Year = 2015;
		// ReSharper disable once UnusedMember.Global
		public static int Day = 14;
		
		private class Deer
		{
			private readonly int _speed;
			private readonly int _flyTime;
			private readonly int _restTime;

			private int _flyTimeLeft;
			private int _restTimeLeft;
			private bool _isResting;
			
			public int TravelledDistance { get; set; }
			
			public int Points { get; set; }

			public Deer(int speed, int flyTime, int restTime)
			{
				_speed = speed;
				_flyTime = flyTime;
				_restTime = restTime;
				_flyTimeLeft = flyTime;
			}

			public void Tick()
			{
				if (_isResting)
				{
					_restTimeLeft--;
					if (_restTimeLeft != 0) 
						return;
					
					_flyTimeLeft = _flyTime;
					_isResting = false;
					return;
				}
				
				_flyTimeLeft--;
				TravelledDistance += _speed;
				if(_flyTimeLeft != 0)
					return;

				_restTimeLeft = _restTime;
				_isResting = true;
			}
		}

		private static IEnumerable<Deer> GetDeers(IEnumerable<string> lines)
		{
			return lines
				.Select(r => r.Split(" "))
				.Select(r => new Deer(int.Parse(r[3]), int.Parse(r[6]), int.Parse(r[13])));
		}
		
		public string Part1(IEnumerable<string> lines)
		{
			var deers = GetDeers(lines);
			var enumerable = deers as Deer[] ?? deers.ToArray();
			for (var i = 0; i < 2503; i++)
			{
				foreach (var deer in enumerable)
				{
					deer.Tick();
				}
			}
			return enumerable.Select(r => r.TravelledDistance).Max().ToString();
		}

		public string Part2(IEnumerable<string> lines)
		{
			var deers = GetDeers(lines);
			var enumerable = deers as Deer[] ?? deers.ToArray();
			for (var i = 0; i < 2503; i++)
			{
				foreach (var deer in enumerable)
				{
					deer.Tick();
				}

				var max = enumerable.Select(r => r.TravelledDistance).Max();
				enumerable = enumerable.Select(r =>
					{
						if(r.TravelledDistance == max)
							r.Points++;
						return r;
					}).ToArray();
			}
			return enumerable.Select(r => r.Points).Max().ToString();
		}
	}
}