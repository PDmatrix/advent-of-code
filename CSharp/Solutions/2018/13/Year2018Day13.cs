using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._13
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day13 : ISolution
	{
		public string Part1(IEnumerable<string> input)
		{
			var grid = input.ToArray();
			var carts = FillCarts(grid);

			while (true)
			{
				carts.Sort((a, b) => (a.Y, a.X).CompareTo((b.Y, b.X)));
				var cartIds = new Dictionary<(int x, int y), List<Cart>>();
				foreach (var cart in carts)
				{
					var cartCoords = (cart.X, cart.Y);
					if (cartIds.ContainsKey(cartCoords))
						return $"{cart.X},{cart.Y}";
					
					Move(cart);
					Turn(cart, grid);
					
					var newCartCoords = (cart.X, cart.Y);
					if (!cartIds.ContainsKey(newCartCoords))
						cartIds[newCartCoords] = new List<Cart> {cart};
					else
						return $"{cart.X},{cart.Y}";
				}
			}
		}

		private static List<Cart> FillCarts(IReadOnlyList<string> grid)
		{
			var carts = new List<Cart>();
			var directions = new Dictionary<char, int>()
			{
				['>'] = 0,
				['v'] = 1,
				['<'] = 2,
				['^'] = 3
			};
			for (var y = 0; y < grid.Count; y++)
			{
				for (var x = 0; x < grid[y].Length; x++)
				{
					if (directions.ContainsKey(grid[y][x]))
						carts.Add(new Cart
						{
							X = x,
							Y = y,
							Mode = 0,
							Direction = directions[grid[y][x]]
						});
				}
			}

			return carts;
		}

		private static void Turn(Cart cart, IReadOnlyList<string> grid)
		{
			var symbol = grid[cart.Y][cart.X];

			switch (symbol)
			{
				case '/':
					cart.Direction = 3 - cart.Direction;
					break;
				case '\\':
					cart.Direction = cart.Direction switch
					{
						var x when x == 0 || x == 2 => cart.Direction + 1,
						_ => cart.Direction - 1
					};
					break;
				case '+':
					cart.Direction = cart.Mode switch
					{
						0 => (int)(cart.Direction - 1 - 4 * Math.Floor((float) (cart.Direction - 1) / 4)),
						2 => (cart.Direction + 1) % 4,
						_ => cart.Direction
					};
					cart.Mode = (cart.Mode + 1) % 3;
					break;
			}
		}

		private static void Move(Cart cart)
		{
			var (x, y) = cart.Direction switch
			{
				0 => (x: 1, y: 0),
				1 => (x: 0, y: 1),
				2 => (x: -1, y: 0),
				_ => (x: 0, y: -1)
			};

			cart.X += x;
			cart.Y += y;
		}

		public string Part2(IEnumerable<string> input)
		{
			var grid = input.ToArray();
			var carts = FillCarts(grid);

			while (carts.Count != 1)
			{
				carts.Sort((a, b) => (a.Y, a.X).CompareTo((b.Y, b.X)));
				var cartIds = new Dictionary<(int x, int y), List<Cart>>();
				foreach (var cart in carts)
				{
					var cartCoords = (cart.X, cart.Y);
					if (cartIds.ContainsKey(cartCoords))
					{
						cartIds[cartCoords].Add(cart);
						continue;
					}
					
					Move(cart);
					Turn(cart, grid);
					var newCartCoords = (cart.X, cart.Y);
					if (!cartIds.ContainsKey(newCartCoords))
						cartIds[newCartCoords] = new List<Cart> {cart};
					else
						cartIds[newCartCoords].Add(cart);
				}
				
				cartIds.Where(x => x.Value.Count > 1).ToList().ForEach(x =>
				{
					x.Value.ForEach(y =>
					{
						carts.Remove(y);
					});
				});
			}

			var remainingCart = carts.First();

			return $"{remainingCart.X},{remainingCart.Y}";
		}
		
		
		private class Cart
		{
			public int X { get; set; }
			public int Y { get; set; }
			public int Mode { get; set; }
			public int Direction { get; set; }
		}
	}
}