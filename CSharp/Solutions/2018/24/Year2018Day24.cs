using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._24
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day24 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var army = Parse(input);
			return Fight(army, 0).units.ToString();
		}

		public string Part2(IEnumerable<string> input)
		{
			var l = 0;
			var h = int.MaxValue / 2;
			while (h - l > 1) {
				var m = (h + l) / 2;
				if (Fight(Parse(input), m).immuneSystem) {
					h = m;
				} else {
					l = m;
				}
			}
			return Fight(Parse(input), h).units.ToString();
		}

		private static (bool immuneSystem, int units) Fight(List<Group> army, int b) {
			foreach (var g in army.Where(g => g.ImmuneSystem))
			{
				g.Damage += b;
			}
			var attack = true;
			while (attack) {
				attack = false;
				var remainingTarget = new HashSet<Group>(army);
				var targets = new Dictionary<Group, Group>();
				foreach (var g in army.OrderByDescending(g => (effectivePower: g.EffectivePower, initiative: g.Initiative)))
				{
					var maxDamage = remainingTarget.Select(t => g.DamageGivenTo(t)).Max();
					if (maxDamage <= 0) continue;
					var possibleTargets = remainingTarget.Where(t => g.DamageGivenTo(t) == maxDamage);
					targets[g] = possibleTargets.OrderByDescending(t => (effectivePower: t.EffectivePower, initiative: t.Initiative)).First();
					remainingTarget.Remove(targets[g]);
				}

				foreach (var g in targets.Keys.OrderByDescending(g => g.Initiative))
				{
					if (g.Units <= 0) continue;
					var target = targets[g];
					var damage = g.DamageGivenTo(target);
					if (damage <= 0 || target.Units <= 0) continue;
					var dies = damage / target.Hp;
					target.Units = Math.Max(0, target.Units - dies);
					if (dies > 0)
					{
						attack = true;
					}
				}

				army = army.Where(g => g.Units > 0).ToList();
			}
			return (army.All(x => x.ImmuneSystem), army.Select(x => x.Units).Sum());
		}

		private static List<Group> Parse(IEnumerable<string> lines) {
            var immuneSystem = false;
            var res = new List<Group>();
            foreach (var line in lines)
                switch (line)
                {
	                case "Immune System:":
		                immuneSystem = true;
		                break;
	                case "Infection:":
		                immuneSystem = false;
		                break;
	                default:
	                {
		                if (line != "") {
			                const string? rx = @"(\d+) units each with (\d+) hit points(.*)with an attack that does (\d+)(.*)damage at initiative (\d+)";
			                var m = Regex.Match(line, rx);
			                if (m.Success) {
				                Group g = new Group();
				                g.ImmuneSystem = immuneSystem;
				                g.Units = int.Parse(m.Groups[1].Value);
				                g.Hp = int.Parse(m.Groups[2].Value);
				                g.Damage = int.Parse(m.Groups[4].Value);
				                g.AttackType = m.Groups[5].Value.Trim();
				                g.Initiative = int.Parse(m.Groups[6].Value);
				                var st = m.Groups[3].Value.Trim();
				                if (st != "") {
					                st = st.Substring(1, st.Length - 2);
					                foreach (var part in st.Split(";")) {
						                var k = part.Split(" to ");
						                var set = new HashSet<string>(k[1].Split(", "));
						                var w = k[0].Trim();
						                switch (w)
						                {
							                case "immune":
								                g.ImmuneTo = set;
								                break;
							                case "weak":
								                g.WeakTo = set;
								                break;
							                default:
								                throw new Exception();
						                }
					                }
				                }
				                res.Add(g);
			                } else {
				                throw new Exception();
			                }

		                }

		                break;
	                }
                }
            return res;
        }

		private class Group
		{
			//4 units each with 9798 hit points (immune to bludgeoning) with an attack that does 1151 fire damage at initiative 9
			public bool ImmuneSystem;
			public int Units;
			public int Hp;
			public int Damage;
			public int Initiative;
			public string AttackType;
			public HashSet<string> ImmuneTo = new HashSet<string>();
			public HashSet<string> WeakTo = new HashSet<string>();

			public int EffectivePower => Units * Damage;

			public int DamageGivenTo(Group target)
			{
				if (target.ImmuneSystem == ImmuneSystem)
					return 0;
				if (target.ImmuneTo.Contains(AttackType))
					return 0;
				if (target.WeakTo.Contains(AttackType))
					return EffectivePower * 2;
				return EffectivePower;
			}
		}
	}
}