namespace AdventOfCode.Solutions._2015._21;

public enum RingBonus
{
	Defense,
	Damage
}
	
public class Ring : Item
{
	public int Bonus { get; set; }
	public RingBonus RingBonus { get; set; }
}