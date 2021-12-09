using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2018._15;

[UsedImplicitly]
public class Year2018Day15 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var arr = input.ToArray();
		var grid = FillGrid(arr);

		return GetGameResult(grid).result.ToString();
	}

	public object Part2(IEnumerable<string> input)
	{
		var arr = input.ToArray();
		var failed = true;
		var result = 0;
		for (var i = 0; failed; i++)
		{
			var grid = FillGrid(arr, 4 + i);
			(failed, result) = GetGameResult(grid);
		}

		return result.ToString();
	}

	private static SortedList<int, Unit> GetAdjacentUnits(GameObject unit, IReadOnlyList<List<GameObject>> grid)
	{
		var sl = new SortedList<int, Unit>();
		foreach (var offset in MovementPatterns.LateralOnly)
		{
			GameObject some;
			try
			{
				some = grid[unit.Y + offset.Y][unit.X + offset.X];
			}
			catch (Exception)
			{
				continue;
			}
				
			if (some is Unit someUnit && someUnit.Type != unit.Type)
			{
				sl.TryAdd(someUnit.Hp, someUnit);
			}
		}

		return sl;
	}

	private static Grid GetPathGrid(IReadOnlyCollection<List<GameObject>> grid)
	{
		var pathGrid = new Grid(grid.First().Count, grid.Count);
		// block all walls
		var counter = 1;
		foreach (var line in grid)
		{
			foreach (var gameObject in line)
			{
				if (gameObject.Type != '.')
					pathGrid.BlockCell(new Position(gameObject.X, gameObject.Y));
				else
					pathGrid.SetCellCost(new Position(gameObject.X, gameObject.Y), counter);
				counter++;
			}
		}

		return pathGrid;
	}

	private static IEnumerable<Unit> GetEnemies(List<List<GameObject>> grid, Unit unit)
	{
		var listOfEnemies = new List<Unit>();

		// fill list of enemies
		foreach (var gameObject in grid.SelectMany(line => line))
		{
			if (!(gameObject is Unit enemy))
				continue;

			if (enemy.Type != unit.Type)
				listOfEnemies.Add(enemy);
		}

		return listOfEnemies;
	}

	private static List<List<GameObject>> FillGrid(string[] arr, int elfAttackPower = 3)
	{
		var grid = new List<List<GameObject>>();
		for (var y = 0; y < arr.Length; y++)
		{
			var list = new List<GameObject>();
			for (var x = 0; x < arr[y].Length; x++)
			{
				GameObject element = arr[y][x] switch
				{
					'#' => new Wall(x, y),
					'.' => new EmptySpot(x, y),
					'E' => new Elf(x, y) { Attack = elfAttackPower },
					'G' => new Goblin(x, y),
					_ => throw new Exception("Invalid element")
				};

				list.Add(element);
			}

			grid.Add(list);
		}

		return grid;
	}

	private static IEnumerable<Unit> GetUnitsFromGrid(List<List<GameObject>> grid)
	{
		var units = new List<Unit>();

		foreach (var gameObject in grid.SelectMany(line => line))
		{
			if (!(gameObject is Unit unit))
				continue;

			units.Add(unit);
		}

		return units;
	}

	private static void DrawMap(IEnumerable<List<GameObject>> grid)
	{
		var sb = new StringBuilder();
		foreach (var line in grid)
		{
			foreach (var gameObject in line)
			{
				sb.Append(gameObject.Type);
			}

			foreach (var gameObject in line)
			{
				if (!(gameObject is Unit unit))
					continue;

				sb.Append($" {unit.Type}({unit.Hp})");
			}

			sb.AppendLine();
		}
			
		Console.WriteLine(sb);
	}

		

	private static (bool failed, int result) GetGameResult(List<List<GameObject>> grid)
	{
		var rounds = 0;
		var failed = false;
		while (true)
		{
			var units = GetUnitsFromGrid(grid);

			// turn
			foreach (var unit in units.Where(x => x.Hp > 0))
			{
				var enemies = GetEnemies(grid, unit);

				var pathGrid = GetPathGrid(grid);

				pathGrid.UnblockCell(new Position(unit.X, unit.Y));
				var sortedList = new SortedList<int, Position>();
				foreach (var enemy in enemies)
				{
					pathGrid.UnblockCell(new Position(enemy.X, enemy.Y));

					var path = pathGrid.GetPath(new Position(unit.X, unit.Y), new Position(enemy.X, enemy.Y),
						MovementPatterns.LateralOnly);

					if (path.Length >= 1)
						sortedList.TryAdd(path.Length, path.Skip(1).First());

					pathGrid.BlockCell(new Position(enemy.X, enemy.Y));
				}

				if (sortedList.Count == 0)
					continue;

				var newPos = sortedList.First().Value;
				if (grid[newPos.Y][newPos.X] is Unit)
				{
					SortedList<int, Unit> sl = GetAdjacentUnits(unit, grid);
					var unitToAttack = sl.First().Value;
					unitToAttack.Hp -= unit.Attack;
					if (unitToAttack.Hp <= 0)
					{
						if (unitToAttack.Type == 'E')
							failed = true;
						grid[unitToAttack.Y][unitToAttack.X] = new EmptySpot(unitToAttack.X, unitToAttack.Y);
					}
				}
				else
				{
					grid[unit.Y][unit.X] = new EmptySpot(unit.X, unit.Y);
					unit.X = newPos.X;
					unit.Y = newPos.Y;
					grid[unit.Y][unit.X] = unit;

					SortedList<int, Unit> sl = GetAdjacentUnits(unit, grid);
					if (!sl.Any())
						continue;

					var unitToAttack = sl.First().Value;
					unitToAttack.Hp -= unit.Attack;
					if (unitToAttack.Hp <= 0)
					{
						if (unitToAttack.Type == 'E')
							failed = true;
						grid[unitToAttack.Y][unitToAttack.X] = new EmptySpot(unitToAttack.X, unitToAttack.Y);
					}
				}
			}

			units = GetUnitsFromGrid(grid).ToList();
			if (units.All(x => x.Type != 'G') || units.All(x => x.Type != 'E'))
				break;

			rounds++;
		}

		var survivingUnitHps = GetUnitsFromGrid(grid).Sum(x => x.Hp);
		var result = rounds * survivingUnitHps;
		return (failed, result);
	}

	private abstract class GameObject
	{
		public abstract char Type { get; set; }
			
		public int X { get; set; }
		public int Y { get; set; }

		public GameObject(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	private abstract class Unit : GameObject
	{
		public Unit(int x, int y) : base(x, y)
		{
		}

		public int Hp { get; set; } = 200;
		public int Attack { get; set; } = 3;
	}

	private class Elf : Unit
	{
		public override char Type { get; set; } = 'E';

		public Elf(int x, int y) : base(x, y)
		{
		}
	}
		
	private class Goblin : Unit
	{
		public override char Type { get; set; } = 'G';

		public Goblin(int x, int y) : base(x, y)
		{
		}
	}
		
	private class Wall : GameObject
	{
		public override char Type { get; set; } = '#';

		public Wall(int x, int y) : base(x, y)
		{
		}
	}
		
	private class EmptySpot : GameObject
	{
		public override char Type { get; set; } = '.';

		public EmptySpot(int x, int y) : base(x, y)
		{
		}
	}
}