using System.Collections.Generic;

namespace AdventOfCode.Common
{
	public interface ISolution
	{
		object Part1(IEnumerable<string> input);
		object Part2(IEnumerable<string> input);
	}
}