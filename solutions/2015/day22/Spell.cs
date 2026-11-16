namespace AdventOfCode.Solutions._2015._22;

public abstract class Spell
{
	public string Name { get; }
		
	public int Cost { get; }
		
	public abstract void Use(Character hero, Character enemy);
		
	public abstract int Timer { get; }
		
	public Spell(string name, int cost)
	{
		Name = name;
		Cost = cost;
	}
}
	
class MagicMissile : Spell
{
	public MagicMissile() : base("Magic Missile", 53) { }
	public override void Use(Character hero, Character enemy)
	{
		enemy.HP -= 4;
	}
	public override int Timer => 0;
}

class Drain : Spell
{
	public Drain() : base("Drain", 73) { }
	public override void Use(Character hero, Character enemy)
	{
		hero.HP += 2;
		enemy.HP -= 2;
	}
	public override int Timer => 0;
}

class Shield : Spell
{
	public Shield() : base("Shield", 113) { }
	private int _timer = 6;
	public override void Use(Character hero, Character enemy)
	{
		if (_timer == 6)
		{
			hero.Defense += 7;
		}
		if (_timer == 1)
		{
			hero.Defense -= 7;
		}
		_timer--;
	}
	public override int Timer => _timer;
}

class Poison : Spell
{
	public Poison() : base("Poison", 173) { }
	private int _timer = 6;
	public override void Use(Character hero, Character enemy)
	{		
		enemy.HP -= 3;
		_timer--;
	}
	public override int Timer => _timer;
}

class Recharge : Spell
{
	public Recharge() : base("Recharge", 229) { }
	private int _timer = 5;
	public override void Use(Character hero, Character enemy)
	{
		hero.Mana += 101;
		_timer--;
	}
	public override int Timer => _timer;
}