using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2016._2;

[UsedImplicitly]
public class Year2016Day02 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var keypad = new[] {"123", "456", "789"};
		var location = new {Row = 2, Col = 2};
		var answer = "";
		foreach (var instructions in input)
		{
			foreach (var instruction in instructions)
			{
				if (instruction == 'U')
					location = new {Row = Math.Max(1, location.Row - 1), location.Col};
				if (instruction == 'D')
					location = new {Row = Math.Min(3, location.Row + 1), location.Col};
				if (instruction == 'R')
					location = new {location.Row, Col = Math.Min(3, location.Col + 1)};
				if (instruction == 'L')
					location = new {location.Row, Col = Math.Max(1, location.Col - 1)};
			}

			answer += keypad[location.Row - 1][location.Col - 1];
		}
		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var keypad = new[] {"  1  ", " 234 ", "56789", " ABC ", "  D  "};
		var location = new {Row = 3, Col = 1};
		var answer = "";
		foreach (var instructions in input)
		{
			foreach (var instruction in instructions)
			{
				var prevLocation = location;
				if (instruction == 'U')
					location = new {Row =  Math.Max(1, location.Row - 1), location.Col};
				if (instruction == 'D')
					location = new {Row = Math.Min(5, location.Row + 1), location.Col};
				if (instruction == 'R')
					location = new {location.Row, Col = Math.Min(5, location.Col + 1)};
				if (instruction == 'L')
					location = new {location.Row, Col = Math.Max(1, location.Col - 1)};
				if (keypad[location.Row - 1][location.Col - 1] == ' ')
					location = prevLocation;
			}

			answer += keypad[location.Row - 1][location.Col - 1];
		}
		return answer;
	}
}