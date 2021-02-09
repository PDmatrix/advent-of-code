using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._21
{
    // ReSharper disable once UnusedMember.Global
    public class Year2017Day21 : ISolution
    {
        public object Part1(IEnumerable<string> input)
        {
            var state = new[] {".#.", "..#", "###"};
            var rules = input.Select(x => x.Replace("/", string.Empty).Split(' ')).Select(x => (x[0], x[2]));
            var lookup = rules.SelectMany(x => Expand(x.Item1).Select(c => (c, x.Item2)))
                .ToDictionary(key => key.c, val => val.Item2);
            for (var i = 0; i < 5; i++)
            {
                state = Iterate(state, lookup);
            }

            return state.Sum(x => x.Count(c => c != '.')).ToString();
        }

        public object Part2(IEnumerable<string> input)
        {
            var state = new[] {".#.", "..#", "###"};
            var rules = input.Select(x => x.Replace("/", string.Empty).Split(' ')).Select(x => (x[0], x[2]));
            var lookup = rules.SelectMany(x => Expand(x.Item1).Select(c => (c, x.Item2)))
                .ToDictionary(key => key.c, val => val.Item2);
            for (var i = 0; i < 18; i++)
            {
                state = Iterate(state, lookup);
            }

            return state.Sum(x => x.Count(c => c != '.')).ToString();
        }


        private static string Flip2(string s) => string.Concat(s[2], s[3], s[0], s[1]);
        private static string Flip2Lr(string s) => string.Concat(s[1], s[0], s[3], s[2]);
        private static string Rot2(string s) => string.Concat(s[2], s[0], s[3], s[1]);

        private static string Flip3(string s) =>
            string.Concat(s[6], s[7], s[8], s[3], s[4], s[5], s[0], s[1], s[2]);

        private static string Flip3Lr(string s) =>
            string.Concat(s[2], s[1], s[0], s[5], s[4], s[3], s[8], s[7], s[6]);

        private static string Rot3(string s) =>
            string.Concat(s[6], s[3], s[0], s[7], s[4], s[1], s[8], s[5], s[2]);

        private static void Perms(ISet<string> s, string q, IEnumerable<Func<string, string>> ops)
        {
            s.Add(q);
            foreach (var op in ops)
            {
                var f = op(q);
                if (!s.Contains(f))
                {
                    Perms(s, f, ops);
                }
            }
        }

        private ISet<string> Expand(string p)
        {
            var s = new HashSet<string>();
            switch (p.Length)
            {
                case 4:
                    Perms(s, p, new List<Func<string, string>> {Flip2, Flip2Lr, Rot2});
                    break;
                case 9:
                    Perms(s, p, new List<Func<string, string>> {Flip3, Flip3Lr, Rot3});
                    break;
                default:
                    throw new Exception();
            }

            return s;
        }

        private static string[] Iterate(IReadOnlyList<string> p, IDictionary<string, string> lookup)
        {
            var result = new List<string>();
            if (p.Count % 2 == 0)
            {
                var y2 = 0;
                for (var y = 0; y < p.Count; y += 2)
                {
                    for (var x = 0; x < p.Count; x += 2)
                    {
                        var key = string.Concat(p[y][x], p[y][x + 1], p[y + 1][x], p[y + 1][x + 1]);
                        var r = lookup[key];

                        Upsert(result, y2, string.Concat(result.ElementAtOrDefault(y2), string.Concat(r.Take(3))));
                        Upsert(result, y2 + 1,
                            string.Concat(result.ElementAtOrDefault(y2 + 1), string.Concat(r.Skip(3).Take(3))));
                        Upsert(result, y2 + 2,
                            string.Concat(result.ElementAtOrDefault(y2 + 2), string.Concat(r.Skip(6).Take(3))));
                    }

                    y2 += 3;
                }
            }
            else
            {
                var y2 = 0;
                for (var y = 0; y < p.Count; y += 3)
                {
                    for (var x = 0; x < p.Count; x += 3)
                    {
                        var key = string.Concat(p[y][x], p[y][x + 1], p[y][x + 2], p[y + 1][x], p[y + 1][x + 1],
                            p[y + 1][x + 2], p[y + 2][x], p[y + 2][x + 1], p[y + 2][x + 2]);
                        var r = lookup[key];

                        Upsert(result, y2, string.Concat(result.ElementAtOrDefault(y2), string.Concat(r.Take(4))));
                        Upsert(result, y2 + 1,
                            string.Concat(result.ElementAtOrDefault(y2 + 1), string.Concat(r.Skip(4).Take(4))));
                        Upsert(result, y2 + 2,
                            string.Concat(result.ElementAtOrDefault(y2 + 2), string.Concat(r.Skip(8).Take(4))));
                        Upsert(result, y2 + 3,
                            string.Concat(result.ElementAtOrDefault(y2 + 3), string.Concat(r.Skip(12).Take(4))));
                    }

                    y2 += 4;
                }
            }

            return result.ToArray();
        }

        private static void Upsert(IList<string> xs, int idx, string value)
        {
            if (xs.Count <= idx)
            {
                xs.Insert(idx, value);
            }
            else
            {
                xs[idx] = value;
            }
        }
    }
}