using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._17
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day17 : ISolution
    {
        public object Part1(IEnumerable<string> input)
        {
            var start = new State
            {
                Path = "",
                Position = (0, 0)
            };
            var passCode = input.First();
            return Bfs(IsSolution, GetValidMoves(passCode), start).First().Path;
        }

        public object Part2(IEnumerable<string> input)
        {
            var start = new State
            {
                Path = "",
                Position = (0, 0)
            };
            var passCode = input.First();
            return Bfs(IsSolution, GetValidMoves(passCode), start)
                .Select(solution => solution.Path.Length)
                .Max()
                .ToString();
        }
        
        private class State
        {
            public (int x, int y) Position { get; set; }
            public string Path { get; set; } = null!;
        }
        
        private static IEnumerable<T> Bfs<T>(Func<T, bool> isSolution, Func<T, IEnumerable<T>> getChildren, T start)
        {
            var q = new Queue<T>();
            q.Enqueue(start);

            IEnumerable<T> Search()
            {
                while (true)
                {
                    if (q.Count == 0) break;
                    
                    var s = q.Dequeue();
                    if (isSolution(s))
                        yield return s;
                    else
                    {
                        foreach (var child in getChildren(s))
                        {
                            q.Enqueue(child);
                        }
                    }
                }
            }

            return Search();
        }
        
        private static bool IsSolution(State state)
        {
            return state.Position == (3, 3);
        }

        private static bool IsOpen(char c)
        {
            return c >= 'b' && c <= 'f';
        }

        private static Func<State, IEnumerable<State>> GetValidMoves(string passCode)
        {
            IEnumerable<State> InnerGetValidMoves(State state)
            {
                var (x, y) = state.Position;
                var h = CreateMd5(passCode + state.Path);
                if (IsOpen(h[0]) && y > 0)
                    yield return new State
                    {
                        Position = (x, y - 1),
                        Path = state.Path + "U"
                    };
                if (IsOpen(h[1]) && y < 3)
                    yield return new State
                    {
                        Position = (x, y + 1),
                        Path = state.Path + "D"
                    };
                if (IsOpen(h[2]) && x > 0)
                    yield return new State
                    {
                        Position = (x - 1, y),
                        Path = state.Path + "L"
                    };
                if (IsOpen(h[3]) && x < 3)
                    yield return new State
                    {
                        Position = (x + 1, y),
                        Path = state.Path + "R"
                    };
            }
                
            return InnerGetValidMoves;
        }
        
        private static string CreateMd5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.Append(hashByte.ToString("X2"));
                }

                return sb.ToString().ToLower();
            }
        }
    }
}