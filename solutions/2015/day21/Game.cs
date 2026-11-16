using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2015._21;

public static class Game
{
	private static readonly IEnumerable<Weapon> Weapons = new List<Weapon>
	{
		new() {Cost = 8, Damage = 4},
		new() {Cost = 10, Damage = 5},
		new() {Cost = 25, Damage = 6},
		new() {Cost = 40, Damage = 7},
		new() {Cost = 74, Damage = 8}
	};
		
	private static readonly IEnumerable<Armor> Armors = new List<Armor>
	{
		new() {Cost = 13, Defense = 1},
		new() {Cost = 31, Defense = 2},
		new() {Cost = 53, Defense = 3},
		new() {Cost = 75, Defense = 4},
		new() {Cost = 102, Defense = 5}
	};
		
	private static readonly IEnumerable<Ring> Rings = new List<Ring>
	{
		new() {Cost = 25, Bonus = 1, RingBonus = RingBonus.Damage},
		new() {Cost = 50, Bonus = 2, RingBonus = RingBonus.Damage},
		new() {Cost = 100, Bonus = 3, RingBonus = RingBonus.Damage},
		new() {Cost = 20, Bonus = 1, RingBonus = RingBonus.Defense},
		new() {Cost = 40, Bonus = 2, RingBonus = RingBonus.Defense},
		new() {Cost = 80, Bonus = 3, RingBonus = RingBonus.Defense}
	};

	// returns all possible combinations of equipment 
	private static IEnumerable<Character> GetAllEquipments()
	{
		foreach (var weapon in Weapons)
		{
			yield return new Character {Weapon = weapon};
			foreach (var armor in Armors)
			{
				yield return new Character
				{
					Weapon = weapon,
					Armor = armor
				};
				foreach (var ring in Rings)
				{
					yield return new Character
					{
						Weapon = weapon,
						Armor = armor,
						LeftRing = ring
					};
					foreach (var rightRing in Rings.Where(r => r.Cost != ring.Cost))
					{
						yield return new Character
						{
							Weapon = weapon,
							Armor = armor,
							LeftRing = ring,
							RightRing = rightRing
						};
					}
				}
			}
			foreach (var ring in Rings)
			{
				yield return new Character
				{
					Weapon = weapon,
					LeftRing = ring
				};
				foreach (var rightRing in Rings.Where(r => r.Cost != ring.Cost))
				{
					yield return new Character
					{
						Weapon = weapon,
						LeftRing = ring,
						RightRing = rightRing
					};
				}
			}
		}
	}

	public static IEnumerable<Character> ExecuteUntilWin(Character enemy)
	{
		foreach (var character in GetAllEquipments())
		{
			var innerEnemy = enemy.Clone();
			var innerCharacter = character.Clone();
			if (Fight(innerCharacter, innerEnemy))
				yield return innerCharacter;
		}
	}
		
	public static IEnumerable<Character> ExecuteUntilLose(Character enemy)
	{
		foreach (var character in GetAllEquipments())
		{
			var innerEnemy = enemy.Clone();
			var innerCharacter = character.Clone();
			if (!Fight(innerCharacter, innerEnemy))
				yield return innerCharacter;
		}
	}

	private static bool Fight(Character hero, Character enemy)
	{
		while (true)
		{
			DealDamage(hero, enemy);
			if (enemy.Hp <= 0)
				return true;
				
			DealDamage(enemy, hero);
			if (hero.Hp <= 0)
				return false;
		}
	}

	private static void DealDamage(Character from, Character to)
	{
		var damage = Math.Max(from.Damage - to.Defense, 1);
		to.Hp -= damage;
	}
}