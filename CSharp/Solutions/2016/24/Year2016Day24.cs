using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2016._24
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day24 : ISolution
    {
        public object Part1(IEnumerable<string> input)
        {
            var enumerable = input.ToList();
            var maxx = enumerable.First().Length;
            var maxy = enumerable.Count;
            var grid = new Grid(maxx, maxy);
            var startLocation = new Position(0, 0);
            var locations = new List<Position>();
            for (var y = 0; y < maxy; y++)
            {
                for (var x = 0; x < maxx; x++)
                {
                    switch (enumerable[y][x])
                    {
                        case '#':
                            grid.BlockCell(new Position(x, y));
                            break;
                        case '0':
                            startLocation = new Position(x, y);
                            break;
                        case '.':
                            break;
                        default:
                            locations.Add(new Position(x, y));
                            break;
                    }
                }
            }

            var a = GetPer(locations.ToArray()).ToArray();
            var len = new List<int>();
            foreach (var element in a)
            {
                var currentNode = startLocation;
                var res = 0;
                foreach (var position in element)
                {
                    res += grid.GetPath(currentNode, position, MovementPatterns.LateralOnly).Length - 1;
                    currentNode = position;
                }
                len.Add(res);
            }
            return len.Min().ToString();
        }

        public object Part2(IEnumerable<string> input)
        {
            var enumerable = input.ToList();
            var maxx = enumerable.First().Length;
            var maxy = enumerable.Count;
            var grid = new Grid(maxx, maxy);
            var startLocation = new Position(0, 0);
            var locations = new List<Position>();
            for (var y = 0; y < maxy; y++)
            {
                for (var x = 0; x < maxx; x++)
                {
                    switch (enumerable[y][x])
                    {
                        case '#':
                            grid.BlockCell(new Position(x, y));
                            break;
                        case '0':
                            startLocation = new Position(x, y);
                            break;
                        case '.':
                            break;
                        default:
                            locations.Add(new Position(x, y));
                            break;
                    }
                }
            }

            var a = GetPer(locations.ToArray()).ToArray();
            var len = new List<int>();
            foreach (var element in a)
            {
                var currentNode = startLocation;
                var res = 0;
                foreach (var position in element)
                {
                    res += grid.GetPath(currentNode, position, MovementPatterns.LateralOnly).Length - 1;
                    currentNode = position;
                }
                res += grid.GetPath(currentNode, startLocation, MovementPatterns.LateralOnly).Length - 1;
                len.Add(res);
            }
            return len.Min().ToString();
        }

        private static void Swap(ref Position a, ref Position b)
        {
            if (a.Equals(b)) 
                return;

            var temp = a;
            a = b;
            b = temp;
        }

        private static IEnumerable<List<Position>> GetPer(Position[] list)
        {
            var x = list.Length - 1;
            return GetPer(list, 0, x);
        }

        private static IEnumerable<List<Position>> GetPer(Position[] list, int k, int m)
        {
            var res = new List<List<Position>>();
            if (k == m)
            {
                res.Add(list.ToList());
            }
            else
                for (var i = k; i <= m; i++)
                {
                    Swap(ref list[k], ref list[i]);
                    var permutations = GetPer(list, k + 1, m);
                    res = res.Union(permutations).ToList();
                    Swap(ref list[k], ref list[i]);
                }

            return res;
        }
    }
}