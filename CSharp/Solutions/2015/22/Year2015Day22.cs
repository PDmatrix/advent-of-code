using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._22
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day22 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var enemy = GetEnemy(input);
			var minAmount = Game.Execute(enemy);
			return minAmount.ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var enemy = GetEnemy(input);
			var minAmount = Game.ExecuteHard(enemy);
			return minAmount.ToString();
		}
		
		private static Character GetEnemy(IEnumerable<string> input)
		{
			var enumerable = input as string[] ?? input.ToArray();
			var hp = int.Parse(enumerable[0].Split(":")[1].Trim());
			var damage = int.Parse(enumerable[1].Split(":")[1].Trim());
			return new Character
			{
				HP = hp,
				Damage = damage
			};
		}
	}
}