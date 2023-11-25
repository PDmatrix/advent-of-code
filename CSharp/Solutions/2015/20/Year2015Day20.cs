using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2015._20;

[UsedImplicitly]
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

	private static int GetDivisorsSum(int n, int limit = -1) 
	{ 
		var sum = 0;
		var iterations = 1;
		for (var i = 1; i <= Math.Sqrt(n); i++)
		{
			if (limit != -1 && iterations >= limit)
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
			calculatedValue = GetDivisorsSum(houseNumber, 50) * 11;
		}
		return houseNumber.ToString();
	}
}