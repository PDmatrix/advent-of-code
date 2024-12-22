using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._21;

[UsedImplicitly]
public class Year2024Day21 : ISolution
{
    private readonly Dictionary<char, (int x, int y)> _directions;
    private readonly Dictionary<(string code, int depthLimit, int curDepth), long> _cache = new();
    private readonly Dictionary<(char start, char end), List<string>> _movesCache = new();

    public Year2024Day21()
    {
        var numPad = new Dictionary<char, (int x, int y)>
        {
            { '7', (0, 0) },
            { '8', (1, 0) },
            { '9', (2, 0) },
            { '4', (0, 1) },
            { '5', (1, 1) },
            { '6', (2, 1) },
            { '1', (0, 2) },
            { '2', (1, 2) },
            { '3', (2, 2) },
            { '0', (1, 3) },
            { 'A', (2, 3) },
        };

        var keyPad = new Dictionary<char, (int x, int y)>
        {
            { '^', (1, 0) },
            {
                'a', (2, 0)
            },
            {
                '<', (0, 1)
            },
            { 'v', (1, 1) },
            { '>', (2, 1) },
        };

        _directions = new Dictionary<char, (int x, int y)>
        {
            { '^', (0, -1) },
            { 'v', (0, 1) },
            { '<', (-1, 0) },
            { '>', (1, 0) },
        };

        foreach (var c in Permutations(numPad, 2))
        {
            var s = c.First().Key;
            var e = c.Last().Key;
            var sLoc = c.First().Value;
            var eLoc = c.Last().Value;
            _movesCache[(s, e)] = MovesBetweenPositions(sLoc, eLoc, false);
        }

        foreach (var c in numPad.Keys)
            _movesCache[(c, c)] = MovesBetweenPositions(numPad[c], numPad[c], false);

        foreach (var c in Permutations(keyPad, 2))
            _movesCache[(c.First().Key, c.Last().Key)] = MovesBetweenPositions(c.First().Value, c.Last().Value);

        foreach (var c in keyPad.Keys)
            _movesCache[(c, c)] = MovesBetweenPositions(keyPad[c], keyPad[c]);
    }

    public object Part1(IEnumerable<string> input)
    {
        long res = 0;
        foreach (var code in input)
        {
            long val = int.Parse(code[..3]);

            res += ShortestLength(code, 2, 0) * val;
        }

        return res;
    }

    public object Part2(IEnumerable<string> input)
    {
        long res = 0;
        foreach (var code in input)
        {
            long val = int.Parse(code[..3]);

            res += ShortestLength(code, 25, 0) * val;
        }

        return res;
    }


    private long ShortestLength(string code, int depthLimit, int curDepth)
    {
        if (_cache.TryGetValue((code, depthLimit, curDepth), out var res))
            return res;

        var curChar = curDepth == 0 ? 'A' : 'a';

        foreach (var c in code)
        {
            if (curDepth == depthLimit) res += _movesCache[(curChar, c)][0].Length;
            else
                res += _movesCache[(curChar, c)].Min(a => ShortestLength(a, depthLimit, curDepth + 1));

            curChar = c;
        }


        _cache[(code, depthLimit, curDepth)] = res;
        return res;
    }

    private List<string> MovesBetweenPositions((int x, int y) start, (int x, int y) end, bool isKeypad = true)
    {
        if (start == end) return new List<string> { "a" };
        List<string> res = new();
        StringBuilder sb = new();

        var (dX, dY) = (end.x - start.x, end.y - start.y);

        sb.Append(dX < 0 ? new string('<', Math.Abs(dX)) : new string('>', Math.Abs(dX)));
        sb.Append(dY < 0 ? new string('^', Math.Abs(dY)) : new string('v', Math.Abs(dY)));

        foreach (var p in Permutations(sb.ToString()))
        {
            var curLoc = start;
            var isValid = true;

            foreach (var c in p)
            {
                curLoc.x += _directions[c].x;
                curLoc.y += _directions[c].y;
                if ((!isKeypad || curLoc != (0, 0)) && (isKeypad || curLoc != (0, 3)))
                    continue;
                isValid = false;
                break;
            }

            if (isValid)
                res.Add(string.Join("", p) + "a");
        }

        return res.Distinct().ToList();
    }

    private static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> values)
    {
        var valList = values.ToArray();
        var current = new T[values.Count()];
        yield return (T[])valList.Clone();

        var indices = Enumerable.Range(0, valList.Length).ToArray();

        int j, k;

        while (true)
        {
            for (j = valList.Length - 2; j >= 0; j--)
                if (indices[j + 1] > indices[j])
                    break;

            if (j == -1) yield break;

            for (k = valList.Length - 1; k > j; k--)
                if (indices[k] > indices[j])
                    break;

            var temp = indices[j];
            indices[j] = indices[k];
            indices[k] = temp;

            for (var i = j + 1; i < indices.Length; i++)
            {
                if (i > indices.Length - i + j) break;
                temp = indices[i];
                indices[i] = indices[indices.Length - i + j];
                indices[indices.Length - i + j] = temp;
            }

            for (var i = 0; i < valList.Length; i++)
                current[i] = valList[indices[i]];

            yield return (T[])current.Clone();
        }

    }

    private static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> values, int subcount)
    {
        var comboList = Combinations(values, subcount).ToList();
        foreach (var combination in comboList)
        {
            IEnumerable<IEnumerable<T>> perms = Permutations(combination);
            foreach (var i in Enumerable.Range(0, perms.Count())) yield return perms.ElementAt(i);
        }
    }

    private static IEnumerable<int[]> Combinations(int m, int n)
    {
        var result = new int[m];

        Stack<int> stack = new(m);
        stack.Push(0);
        while (stack.Count > 0)
        {
            var index = stack.Count - 1;
            var value = stack.Pop();
            while (value < n)
            {
                result[index++] = value++;
                stack.Push(value);
                if (index != m) continue;
                yield return (int[])result.Clone();
                break;
            }
        }
    }

    private static IEnumerable<IEnumerable<T>> Combinations<T>(IEnumerable<T> array, int m)
    {
        if (array.Count() < m)
            throw new ArgumentException("Array length can't be less than number of selected elements");
        if (m < 1)
            throw new ArgumentException("Number of selected elements can't be less than 1");
        var result = new T[m];
        foreach (var j in Combinations(m, array.Count()))
        {
            for (var i = 0; i < m; i++)
            {
                result[i] = array.ElementAt(j[i]);
            }

            yield return (IEnumerable<T>)result.Clone();
        }
    }
}