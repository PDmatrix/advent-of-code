using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._2
{
	// ReSharper disable once UnusedMember.Global
	public class Year2017Day02 : ISolution
	{
		public object Part1(IEnumerable<string> input)
		{
			var spreadSheet = GetSpreadSheet(input);
			return spreadSheet.Select(r =>
			{
				var enumerable = r as int[] ?? r.ToArray();
				return enumerable.Max() - enumerable.Min();
			}).Sum().ToString();
		}
		
		public object Part2(IEnumerable<string> input)
		{
			var spreadSheet = GetSpreadSheet(input);
			return spreadSheet.Select(GetEvenDivide).Sum().ToString();
		}

		private static IEnumerable<IEnumerable<int>> GetSpreadSheet(IEnumerable<string> rawData)
		{
			return rawData.Select(GetRow);
		}

		private static IEnumerable<int> GetRow(string rawRow)
		{
			return rawRow.Split('\t').Select(int.Parse);
		}

		private static int GetEvenDivide(IEnumerable<int> row)
		{
			var enumerable = row as int[] ?? row.ToArray();
			foreach (var value in enumerable)
			{
				var result = GetDivider(value, enumerable);
				if (result != -1) return result;
			}
			throw new Exception("No answer found");
		}

		private static int GetDivider(int value, IEnumerable<int> values)
		{
			foreach (var item in values)
			{
				if (item == value || item < value) continue;
				if (item % value == 0) return item / value;
			}
			return -1;
		}
	}
}