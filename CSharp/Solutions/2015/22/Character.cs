using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2015._22
{
	public class Character
	{
		public int HP { get; set; } = 100;

		public int Damage { get; set; }

		public int Defense { get; set; }

		public int Mana { get; set; }
		public int SpentMana { get; set; }
		private List<Spell> activeSpells = new List<Spell>();
		
		public void AddSpell(Spell spell, Character enemy)
		{		
			Mana -= spell.Cost;
			SpentMana += spell.Cost;
			if (Mana < 0) throw new ArgumentException("Can't afford this spell");
			if (spell.Timer == 0)
			{
				spell.Use(this, enemy);
			}
			else
			{
				activeSpells.Add(spell);
			}
		}

		public void UseSpells(Character enemy)
		{
			foreach (var spell in activeSpells)
			{
				if (enemy.HP <= 0) 
					continue;
			
				spell.Use(this, enemy);
			}
			activeSpells.RemoveAll(s => s.Timer == 0);
		}
		
		public bool IsSpellActive(string spellName)
		{
			return activeSpells.Any(s => s.Name == spellName);
		}
		
		public void HitBy(Character enemy)
		{
			var bossDamage = Math.Max(1, enemy.Damage - Defense); 
			HP -= bossDamage;
		}
		
		public Character Clone()
		{
			return new Character
			{
				HP = HP,
				Damage = Damage,
				Defense = Defense
			};
		}
	}
}