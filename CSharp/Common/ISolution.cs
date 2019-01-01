using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode.Common
{
	public interface ISolution
	{
		string Part1(IEnumerable<string> lines);
		string Part2(IEnumerable<string> lines);
	}
}