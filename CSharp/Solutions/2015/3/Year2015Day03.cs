using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._3
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day03 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var directions = input.First();
			var location = new {X = 0, Y = 0};
			var points = new HashSet<object>{location};
			foreach (var direction in directions)
			{
				if (direction == '>')
					location = new {X = location.X + 1, location.Y};
				if (direction == '<')
					location = new {X = location.X - 1, location.Y};
				if (direction == '^')
					location = new {location.X, Y = location.Y + 1};
				if (direction == 'v')
					location = new {location.X, Y = location.Y - 1};
				points.Add(location);
			}
			return points.Count.ToString();
		}

		public object Part2(IEnumerable<string> input)
		{
			var directions = input.First();
			var santa = new {X = 0, Y = 0};
			var roboSanta = new {X = 0, Y = 0};
			var santaTurn = true;
			var points = new HashSet<object>{santa};
			foreach (var direction in directions)
			{
				var location = santaTurn ? santa : roboSanta;
				if (direction == '>')
					location = new {X = location.X + 1, location.Y};
				if (direction == '<')
					location = new {X = location.X - 1, location.Y};
				if (direction == '^')
					location = new {location.X, Y = location.Y + 1};
				if (direction == 'v')
					location = new {location.X, Y = location.Y - 1};
				if (santaTurn)
					santa = location;
				else
					roboSanta = location;
				santaTurn = !santaTurn;
				points.Add(location);
			}
			return points.Count.ToString();
		}
	}
}