using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._25;

[UsedImplicitly]
public class Year2020Day25 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var doorPublicKey = int.Parse(input.First());
		var cardPublicKey = int.Parse(input.Last());
		var subjectKey = 7;
		var loopSize = 2;
		long value;
		var dp = new Dictionary<int, long>
		{
			[1] = subjectKey
		};

		while (true)
		{
			value = dp[loopSize - 1];
			value *= subjectKey;
			value %= 20201227;

			if (value == doorPublicKey)
				break;

			dp[loopSize] = value;
			loopSize++;
		}

		value = 1;
		subjectKey = cardPublicKey;
		for (var i = 0; i < loopSize; i++)
		{
			value *= subjectKey;
			value %= 20201227;
		}

		return value;
	}

	public object Part2(IEnumerable<string> input)
	{
		return "Congratulations!";
	}
}