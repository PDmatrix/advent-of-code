using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2025._10;

[UsedImplicitly]
public class Year2025Day10 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var machines = ParseInput(input);

		var answer = 0;
		foreach (var machine in machines)
		{
			var q = new Queue<(string state, int presses)>();
			var visited = new HashSet<string>();
			q.Enqueue((GetState(machine.Lights).Replace('#', '.'), 0));
			while (q.Any())
			{
				var (currentState, presses) = q.Dequeue();
				if (!visited.Add(currentState))
					continue;

				if (currentState == GetState(machine.Lights))
				{
					answer += presses;
					break;
				}

				var lights = currentState.Select(c => c == '#').ToArray();
				foreach (var buttons in machine.Buttons)
				{
					var lightsCopy = (bool[])lights.Clone();
					foreach (var button in buttons)
						lightsCopy[button] = !lightsCopy[button];

					q.Enqueue((GetState(lightsCopy), presses + 1));
				}
			}
		}

		return answer;
	}

	// Kudos to u/Gabba333
	// https://www.reddit.com/r/adventofcode/comments/1pity70/comment/nte1moj/
	public object Part2(IEnumerable<string> input)
	{
		var inputParsed = input.Select(line => line.Split(' ') switch
		{
			var arr => (arr.First(),
				arr[1..^1].ToArray(),
				arr.Skip(arr.Length - 1).Single())
		});

		var lights = inputParsed.Select(tp => tp.Item1[1..^1]).ToArray();
		var buttons = inputParsed.Select(tp => tp.Item2.Select(button => button[1..^1].Split(',')
			.Select(int.Parse).ToArray()).ToArray()).ToArray();
		var joltages = inputParsed.Select(tp => tp.Item3[1..^1].Split(',').Select(int.Parse).ToArray()).ToArray();

		return Enumerable.Range(0, lights.Length).Select(reduceAndBackSubstitute).Sum();

		long reduceAndBackSubstitute(int index)
		{
			(int R, int C) = (joltages[index].Length, buttons[index].Length);

			//create our matrix, Ax = b
			int[][] A = new int[R][];
			for (int r = 0; r < R; r++)
			{
				//make an extra col. for the b values
				A[r] = new int[C + 1];
				A[r][C] = joltages[index][r];
			}

			foreach (var button in buttons[index].Index())
			foreach (var toggle in button.Item)
				A[toggle][button.Index] = 1;

			//we are going to reduce out matrix as much as possible
			for (int r = 0; r < R && r < C; r++)
			{
				//for row i, we need to reduce column i if required
				int pivot = -1;
				for (int p = r; p < R; p++)
					if (A[p][r] != 0)
						pivot = p;

				if (pivot == -1)
					//column is all zeroes already so we are done
					continue;

				//swap our pivot row (and values)
				swap(A, pivot, r);
				//and reduce all the rows below our pivot row
				for (int p = r + 1; p < R; p++)
				{
					if (A[p][r] == 0)
						continue;

					//make sure we keep our maths to integers!
					(var deltaNom, var deltaDen) = (A[p][r], A[r][r]);
					for (int c = 0; c < C; c++)
					{
						A[p][c] = deltaDen * A[p][c] - A[r][c] * deltaNom;
					}

					A[p][C] = A[p][C] * deltaDen - A[r][C] * deltaNom;
				}
				//we have fully reduced from r+1 to the end
			}

			//now we have to start substituting in
			//and if we get stuck start brute forcing the parameters
			return backSub(index, R, C, A);
		}

		int backSub(int index, int R, int C, int[][] A)
		{
			var maximums = buttons[index].Select(toggles => toggles.Min(bi => joltages[index][bi])).ToArray();

			int best = int.MaxValue;
			Stack<(int row, int[] pressed)> stack = new();

			stack.Push((R - 1, Enumerable.Repeat(-1, C).ToArray()));
			while (stack.Any())
			{
				(var r, var pressed) = stack.Pop();
				if (r < 0)
				{
					//we've reached the end with no failures, this must be a solution
					best = Math.Min(pressed.Sum(), best);
					//Console.WriteLine($"#{index} in {best} total: {String.Join(", ", pressed.Index().Select(tp => $"b{tp.Index}={tp.Item}").ToArray())}");
					continue;
				}

				//we have a row of the form 0 + 0 + ... xr + r(r+1) ... xfinal = br
				//we need to sub in any values we know and then start brute forcing the rest
				int rowTotal = A[r][C];
				for (int c = r; c < C; c++)
					if (A[r][c] != 0)
						if (pressed[c] >= 0)
							rowTotal -= A[r][c] * pressed[c];
						else
							goto bruteForce;

				//1) rowTotal = 0, the row is consistent and all values are specified, move to next row
				//2) rowTotal != 0, this was a failed substitution so quit this path
				if (rowTotal == 0)
					stack.Push((r - 1, pressed));
				continue;
				bruteForce: ;
				//we need to choose our next value
				//so push all the options to test
				for (int c = r; c < C; c++)
				{
					if (A[r][c] != 0 && pressed[c] == -1)
					{
						//upper bound of our choice is confusing because of -'ve numbers in the matrix
						//so just use an easily calculated pre-computed max
						var max = maximums[c];

						//this tigher bound reduces runtime by about 20%
						//even better might be to find the min number of presses for any button to reach
						//each joltage total this button doesn't impact
						var pressedSoFar = pressed.Sum();
						if (pressedSoFar + max >= best)
						{
							max = best - pressedSoFar;
						}

						for (int p = 0; p <= max; p++)
						{
							var newPressed = pressed.ToArray();
							newPressed[c] = p;
							stack.Push((r, newPressed));
						}

						break;
					}
				}
			}

			Console.WriteLine($"#{index}: {best}");
			return best;
		}

		void swap<T>(T[] arr, int i0, int i1)
		{
			var tmp = arr[i0];
			arr[i0] = arr[i1];
			arr[i1] = tmp;
		}

		void print(int[][] arr)
		{
			foreach (var a in arr)
			{
				Console.WriteLine(String.Join("",
					                  a[0..^1].Select(d => $"{(Math.Sign(d) >= 0 ? "+" : "")}{d}     ").ToArray()) +
				                  $" = {a.Last()}");
			}

			Console.WriteLine();
		}

		string empty(int length) => new string(Enumerable.Repeat('.', length).ToArray());

		string press(string before, int[] button)
		{
			var res = new System.Text.StringBuilder(before);
			foreach (var toggle in button)
			{
				res[toggle] = res[toggle] == '#' ? '.' : '#';
			}

			return res.ToString();
		}
	}

	record State(string lights, long presses);

	private static string GetState(bool[] lights)
	{
		return string.Concat(lights.Select(l => l ? '#' : '.'));
	}

	private static List<Machine> ParseInput(IEnumerable<string> input)
	{
		var machines = new List<Machine>();
		var regex = new Regex(
			@"\[(?<l>.*?)\]\s+(?<b>(?:\([0-9, ]+\)\s*)+)\s*\{(?<j>[0-9, ]+)\}",
			RegexOptions.Compiled);

		foreach (var line in input)
		{
			var match = regex.Match(line);
			if (!match.Success)
				throw new ArgumentException("Invalid input line: " + line);

			var lightsStr = match.Groups["l"].Value;
			var buttonsStr = match.Groups["b"].Value.TrimEnd();
			var joltagesStr = match.Groups["j"].Value;

			var lights = lightsStr.Select(c => c == '#').ToArray();

			var buttons = buttonsStr.Split(')', StringSplitOptions.RemoveEmptyEntries).Select(b =>
				b.Trim('(', ' ').Split(',', StringSplitOptions.RemoveEmptyEntries)
					.Select(int.Parse).ToList()).ToList();

			var joltages = joltagesStr.Split(',', StringSplitOptions.RemoveEmptyEntries)
				.Select(s => int.Parse(s.Trim())).ToList();

			machines.Add(new Machine(lights, buttons, joltages));
		}

		return machines;
	}

	private record struct Machine(bool[] Lights, List<List<int>> Buttons, List<int> Joltages);
}