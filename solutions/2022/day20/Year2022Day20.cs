using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using CSharpx;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._20;

[UsedImplicitly]
public partial class Year2022Day20 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var c = 0;
		var list = input.Select(x => (c++, int.Parse(x))).ToList();
		
		var copyList = new List<(int, int)>(list);
		
		for (var i = 0; i < list.Count; i++)
		{
			var (index, instruction) = list[i];
			var indexOfInstruction = copyList.FindIndex(x => x.Item1 == index);
			
			instruction %= (list.Count - 1);
			var newIndex = indexOfInstruction + instruction;
			
			if (newIndex >= list.Count)
				newIndex = newIndex - list.Count + 1;
			else if (newIndex <= 0)
				newIndex = newIndex + list.Count - 1;
			
			var removed = copyList[indexOfInstruction];
			copyList.RemoveAt(indexOfInstruction);
			copyList.Insert(newIndex, removed);
		}

		var zeroIndex = copyList.FindIndex(x => x.Item2 == 0);

		return copyList[(zeroIndex + 1000) % copyList.Count].Item2 +
		       copyList[(zeroIndex + 2000) % copyList.Count].Item2 +
		       copyList[(zeroIndex + 3000) % copyList.Count].Item2;
	}

	public object Part2(IEnumerable<string> input)
	{
		var c = 0;
		var list = input.Select(x => (c++, long.Parse(x))).ToList();
		list = list.Select(x => (x.Item1, x.Item2 * 811589153)).ToList();
		
		var copyList = new List<(int, long)>(list);

		for (var shuffle = 0; shuffle < 10; shuffle++)
		{
			for (var i = 0; i < list.Count; i++)
			{
				var (index, instruction) = list[i];
				var indexOfInstruction = copyList.FindIndex(x => x.Item1 == index);

				instruction %= (list.Count - 1);
				var newIndex = indexOfInstruction + instruction;

				if (newIndex >= list.Count)
					newIndex = newIndex - list.Count + 1;
				else if (newIndex <= 0)
					newIndex = newIndex + list.Count - 1;

				var removed = copyList[indexOfInstruction];
				copyList.RemoveAt(indexOfInstruction);
				copyList.Insert((int)newIndex, removed);
			}
		}

		var zeroIndex = copyList.FindIndex(x => x.Item2 == 0);

		return copyList[(zeroIndex + 1000) % copyList.Count].Item2 +
		       copyList[(zeroIndex + 2000) % copyList.Count].Item2 +
		       copyList[(zeroIndex + 3000) % copyList.Count].Item2;
	}
}