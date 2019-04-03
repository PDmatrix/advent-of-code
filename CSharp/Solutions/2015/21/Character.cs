namespace AdventOfCode.Solutions._2015._21
{
	public class Character
	{
		public int HP { get; set; } = 100;

		public int Damage
		{
			get
			{
				var damage = Weapon.Damage;
				if (LeftRing?.RingBonus == RingBonus.Damage)
					damage += LeftRing.Bonus;
				if (RightRing?.RingBonus == RingBonus.Damage)
					damage += RightRing.Bonus;
				return damage;
			}
		}

		public int Defense
		{
			get
			{
				var defense = Armor?.Defense ?? 0;
				if (LeftRing?.RingBonus == RingBonus.Defense)
					defense += LeftRing.Bonus;
				if (RightRing?.RingBonus == RingBonus.Defense)
					defense += RightRing.Bonus;
				return defense;
			}
		}

		public int EqipmentCost => 
			Weapon.Cost 
			+ (Armor?.Cost ?? 0) 
			+ (LeftRing?.Cost ?? 0) 
			+ (RightRing?.Cost ?? 0);

		public Armor Armor { get; set; }
		public Weapon Weapon { get; set; }
		public Ring LeftRing { get; set; }
		public Ring RightRing { get; set; }

		public Character Clone()
		{
			return new Character
			{
				Armor = Armor,
				Weapon = Weapon,
				HP = HP,
				LeftRing = LeftRing,
				RightRing = RightRing
			};
		}
	}
}