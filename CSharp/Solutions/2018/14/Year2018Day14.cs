using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2018._14
{
	// ReSharper disable once UnusedMember.Global
	public class Year2018Day14 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var recipes = new List<int> {3, 7};
			var recipeTimes = int.Parse(input.First());

			var firstElf = 0;
			var secondElf = 1;
			while (true)
			{
				var next = recipes[firstElf] + recipes[secondElf];
				recipes.AddRange(next.ToString().Select(x => int.Parse(x.ToString())));
				firstElf = (recipes[firstElf] + firstElf + 1) % recipes.Count;
				secondElf = (recipes[secondElf] + secondElf + 1) % recipes.Count;
				
				if (recipes.Count > recipeTimes + 10)
					break;
			}

			var sb = new StringBuilder();
			for (var i = recipeTimes; i < recipeTimes + 10; i++)
				sb.Append(recipes[i]);
			
			return sb.ToString();
		}
		
		public object Part2(IEnumerable<string> input)
		{
			var recipes = new List<int> {3, 7};
			var recipe = input.First();

			var firstElf = 0;
			var secondElf = 1;
			var idx = 0;
			var sb = new StringBuilder();
			while (true)
			{
				if (sb.Length != recipe.Length)
					sb.Append(recipes[idx].ToString());
				else
				{
					sb.Remove(0, 1);
					sb.Append(recipes[idx].ToString());
				}
				
				if (sb.ToString() == recipe)
					break;

				idx++;
				var next = recipes[firstElf] + recipes[secondElf];
				recipes.AddRange(next.ToString().Select(x => int.Parse(x.ToString())));
				firstElf = (recipes[firstElf] + firstElf + 1) % recipes.Count;
				secondElf = (recipes[secondElf] + secondElf + 1) % recipes.Count;
			}

			
			return (1 + idx - recipe.Length).ToString();
		}
	}
}