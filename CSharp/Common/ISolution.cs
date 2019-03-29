using System.Collections.Generic;

namespace AdventOfCode.Common
{
	public interface ISolution
	{
		string Part1(IEnumerable<string> input);
		string Part2(IEnumerable<string> input);
	}
}