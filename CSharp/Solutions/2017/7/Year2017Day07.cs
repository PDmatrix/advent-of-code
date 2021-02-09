using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._7
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day07 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var towers = GetTowers(input).ToList();
			BuildTowerTree(towers);
			var bottomTower = GetBottomTower(towers);
			return bottomTower.Name;
		}

		public object Part2(IEnumerable<string> input)
		{
			var towers = GetTowers(input).ToList();
			BuildTowerTree(towers);
			return GetUnbalancedWeight(GetBottomTower(towers))?.ToString()!;
		}
		
		private static Tower GetBottomTower(IEnumerable<Tower> towers)
		{
			return towers.First(r => r.Parents.Count == 0);
		}

		private static void BuildTowerTree(IReadOnlyCollection<Tower> towers)
		{
			var dict = towers.ToDictionary(k => k.Name, v => v);
			foreach (var tower in towers)
			{
				tower.Children = tower.Children.Select(r => dict[r.Name]).ToList();
				foreach (var child in tower.Children)
				{
					child.Parents.Add(dict[tower.Name]);
				}
			}
		}

		private static IEnumerable<Tower> GetTowers(IEnumerable<string> input)
		{
			const string pattern = @"(\w+) \((\d+)\)( -> ([\w, ]+))?";
			foreach (var rawTower in input)
			{
				var groups = Regex.Match(rawTower, pattern).Groups;
				yield return new Tower
				{
					Name = groups[1].Value,
					Weight = int.Parse(groups[2].Value),
					Parents = new List<Tower>(),
					Children = string.IsNullOrWhiteSpace(groups[4].Value) ? new List<Tower>() : groups[4].Value.Split(", ").Select(r => new Tower
					{
						Name = r
					}).ToList()
				};
			}
		}

		private static int? GetUnbalancedWeight(Tower tower)
		{
			if (tower.Children.Count == 0) return null;
			foreach (var child in tower.Children)
			{
				var unbalanced = GetUnbalancedWeight(child);
				if (unbalanced != null) return unbalanced;
			}

			var weights = tower.Children.Select(GetTowerWidth).ToArray();
			var (differentIndex, correctIndex) = GetDifferentIndex(weights);
			if (differentIndex < 0) return null;

			var difference = weights[differentIndex] - weights[correctIndex];
			var differentTower = tower.Children.ElementAt(differentIndex);
			return differentTower.Weight - difference;
		}

		private static int GetTowerWidth(Tower tower)
		{
			return tower.Weight + tower.Children.Aggregate(0, (a, b) => a + GetTowerWidth(b));
		}

		private static (int differentIndex, int correctIndex) GetDifferentIndex(IList<int> arr)
		{
			var list = arr.ToList();
			var idx = list.FindIndex(w => w != arr[0]);
			if (idx < 0) return (-1, -1);
			return idx == 1 && arr[idx] == arr[arr.Count - 1] ? (0, idx) : (idx, 0);
		}

		private class Tower
		{
			public string Name { get; set; } = null!;
			public int Weight { get; set; }
			public ICollection<Tower> Parents { get; set; } = null!;
			public ICollection<Tower> Children { get; set; } = null!;
		}
	}
}