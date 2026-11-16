using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2015._22;

[UsedImplicitly]
public class Year2015Day22 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var enemy = GetEnemy(input);
		var minAmount = Game.Execute(enemy);
		return minAmount.ToString();
	}

	public object Part2(IEnumerable<string> input)
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