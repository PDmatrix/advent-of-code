using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2015._22;

public static class Game
{
	private static readonly IEnumerable<Spell> Spells = new List<Spell>
	{
		new MagicMissile(),
		new Recharge(),
		new Shield(),
		new Poison(),
		new Drain()
	};
	public static int Execute(Character enemy)
	{
		var minAmountOfMana = int.MaxValue;
		for (var i = 0; i < 100000; i++)
		{
			var hero = new Character
			{
				Mana = 500,
				HP = 50
			};
			if (Fight(hero, enemy.Clone(), minAmountOfMana))
				minAmountOfMana = Math.Min(hero.SpentMana, minAmountOfMana);
		}

		return minAmountOfMana;
	}
		
	public static int ExecuteHard(Character enemy)
	{
		var minAmountOfMana = int.MaxValue;
		for (var i = 0; i < 100000; i++)
		{
			var hero = new Character
			{
				Mana = 500,
				HP = 50
			};
			if (FightHard(hero, enemy.Clone(), minAmountOfMana))
				minAmountOfMana = Math.Min(hero.SpentMana, minAmountOfMana);
		}

		return minAmountOfMana;
	}

	private static bool Fight(Character hero, Character enemy, int bestResult)
	{
		while (true)
		{
			hero.UseSpells(enemy);
			if (enemy.HP <= 0)
				return true;
			var spell = GetAvailableSpell(hero, bestResult);
			if (spell == null)
				return false;
				
			hero.AddSpell(spell, enemy);
			if (enemy.HP <= 0)
				return true;
				
			hero.UseSpells(enemy);
			if (enemy.HP <= 0)
				return true;
				
			hero.HitBy(enemy);
			if (hero.HP <= 0)
				return false;
		}
	}
		
	private static bool FightHard(Character hero, Character enemy, int bestResult)
	{
		while (true)
		{
			hero.HP--;
			if (hero.HP <= 0)
				return false;
			hero.UseSpells(enemy);
			if (enemy.HP <= 0)
				return true;
			var spell = GetAvailableSpell(hero, bestResult);
			if (spell == null)
				return false;
				
			hero.AddSpell(spell, enemy);
			if (enemy.HP <= 0)
				return true;
				
			hero.UseSpells(enemy);
			if (enemy.HP <= 0)
				return true;
				
			hero.HitBy(enemy);
			if (hero.HP <= 0)
				return false;
		}
	}
		
	private static Spell? GetAvailableSpell(Character hero, int bestResult)
	{
			
		var options = Spells.Where(s => s.Cost <= hero.Mana && 
		                                s.Cost + hero.SpentMana < bestResult &&
		                                !hero.IsSpellActive(s.Name)).ToList();
		if (options.Count == 0) 
			return null;
			
		var rand = new Random();
		var t = options[rand.Next(options.Count)].GetType();
		var spell = (Spell)Activator.CreateInstance(t)!;
		return spell;
	}
}