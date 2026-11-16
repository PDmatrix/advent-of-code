using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventOfCode.Solutions._2022._13;

[UsedImplicitly]
public class Year2022Day13 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var pairs = ParseInput(input).ToList();

		var sum = 0;
		for (var i = 0; i < pairs.Count; i++)
		{
			if (IsInRightOrder(pairs[i].left, pairs[i].right).order)
				sum += i + 1;
		}

		return sum;
	}

	private static (bool order, bool cont) IsInRightOrder(JToken left, JToken right)
	{
		var leftArr = left as JArray;
		var rightArr = right as JArray;

		for (var i = 0; i < leftArr!.Count; i++)
		{
			if (i >= rightArr!.Count)
				return (false, false);

			if (leftArr[i] is JValue && rightArr[i] is JValue)
			{
				var leftVal = leftArr[i].Value<int>();
				var rightVal = rightArr[i].Value<int>();

				if (leftVal < rightVal)
					return (true, false);

				if (leftVal > rightVal)
					return (false, false);

				continue;
			}

			if (leftArr[i] is JArray && rightArr[i] is JArray)
			{
				var oc = IsInRightOrder(leftArr[i], rightArr[i]);

				if (!oc.cont)
					return oc;

				continue;
			}

			if (leftArr[i] is JArray && rightArr[i] is JValue)
			{
				var oc = IsInRightOrder(leftArr[i], new JArray(rightArr[i]));

				if (!oc.cont)
					return oc;

				continue;
			}

			if (leftArr[i] is JValue && rightArr[i] is JArray)
			{
				var oc = IsInRightOrder(new JArray(leftArr[i]), rightArr[i]);

				if (!oc.cont)
					return oc;

				continue;
			}
		}

		return (true, leftArr!.Count >= rightArr!.Count);
	}

	private static IEnumerable<(JToken left, JToken right)> ParseInput(IEnumerable<string> input)
	{
		var pairs = new List<(JToken left, JToken right)>();
		JToken? left = null;
		JToken? right = null;
		foreach (var line in input)
		{
			if (string.IsNullOrEmpty(line))
			{
				pairs.Add((left!, right!));
				left = null;
				right = null;
				continue;
			}

			if (left == null)
				left = JsonConvert.DeserializeObject<JToken>(line);
			else
				right = JsonConvert.DeserializeObject<JToken>(line);
		}

		pairs.Add((left!, right!));
		return pairs;
	}

	private static IEnumerable<JToken> ParseInputV2(IEnumerable<string> input)
	{
		foreach (var line in input)
		{
			if (string.IsNullOrEmpty(line))
				continue;

			yield return JsonConvert.DeserializeObject<JToken>(line);
		}
	}

	private static List<JToken> Sort(List<JToken> arr)
	{
		var n = arr.Count;
		for (var i = 0; i < n - 1; i++)
		for (var j = 0; j < n - i - 1; j++)
			if (!IsInRightOrder(arr[j], arr[j + 1]).order)
			{
				(arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
			}

		return arr;
	}

	public object Part2(IEnumerable<string> input)
	{
		input = input.Append("[[2]]");
		input = input.Append("[[6]]");

		var arr = ParseInputV2(input);
		var sortedArr = Sort(arr.ToList());

		var answer = 1;
		for (var i = 0; i < sortedArr.Count(); i++)
		{
			if (sortedArr[i].ToString(Formatting.None) == "[[2]]")
				answer = i + 1;

			if (sortedArr[i].ToString(Formatting.None) == "[[6]]")
				answer *= i + 1;
		}

		return answer;
	}
}