using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._20
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day20 : ISolution
	{
		public object Part1(IEnumerable<string> lines)
		{
			var input = int.Parse(lines.FirstOrDefault());
			var calculatedValue = 0;
			var houseNumber = 0;
			while (input > calculatedValue)
			{
				houseNumber++;
				calculatedValue = GetDivisorsSum(houseNumber) * 10;
			}
			return houseNumber.ToString();
		}

		private static int GetDivisorsSum(int n) 
		{ 
			var sum = 0; 
			for (var i = 1; i <= Math.Sqrt(n); i++)
			{
				if (n % i != 0) 
					continue;
				
				sum += i;
				if (n / i != i)
					sum += n / i;
			}
			return sum; 
		}
		
		private static int GetDivisorsSumWithLimit(int n) 
		{ 
			var sum = 0;
			var iterations = 1;
			for (var i = 1; i <= Math.Sqrt(n); i++)
			{
				if(iterations >= 50)
					break;
				
				if (n % i != 0) 
					continue;
				
				sum += i;
				iterations++;
				if (n / i == i) 
					continue;
				
				sum += n / i;
				iterations++;
			}
			return sum; 
		}
		
		public object Part2(IEnumerable<string> lines)
		{
			var input = int.Parse(lines.FirstOrDefault());
			var calculatedValue = 0;
			var houseNumber = 0;
			while (input > calculatedValue)
			{
				houseNumber++;
				calculatedValue = GetDivisorsSumWithLimit(houseNumber) * 11;
			}
			return houseNumber.ToString();
		}
	}
}