using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2016._13
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day13 : ISolution
    {
        public string Part1(IEnumerable<string> input)
        {
            const int maxX = 100;
            const int maxY = 100;
            var grid = new Grid(maxX, maxY);
            var officerDesignersFavoriteNumber = int.Parse(input.First());
            for (var x = 0; x < maxX; x++)
            {
                for (var y = 0; y < maxY; y++)
                {
                    var calc = x * x + 3 * x + 2 * x * y + y + y * y;
                    calc += officerDesignersFavoriteNumber;
                    var bits = Convert.ToString(calc, 2).Count(c => c == '1');
                    if (bits % 2 != 0)
                        grid.BlockCell(new Position(x, y));
                }
            }

            var length = grid.GetPath(new Position(1, 1), new Position(31, 39), MovementPatterns.LateralOnly).Length;
            return (length - 1).ToString();
        }

        public string Part2(IEnumerable<string> input)
        {
            const int maxX = 100;
            const int maxY = 100;
            var grid = new Grid(maxX, maxY);
            var officerDesignersFavoriteNumber = int.Parse(input.First());
            var openSpaces = new List<Tuple<int, int>>();
            for (var x = 0; x < maxX; x++)
            {
                for (var y = 0; y < maxY; y++)
                {
                    var calc = x * x + 3 * x + 2 * x * y + y + y * y;
                    calc += officerDesignersFavoriteNumber;
                    var bits = Convert.ToString(calc, 2).Count(c => c == '1');
                    if (bits % 2 != 0)
                        grid.BlockCell(new Position(x, y));
                    else
                        openSpaces.Add(new Tuple<int, int>(x, y));
                }
            }

            var answer = 0;
            foreach (var (x, y) in openSpaces)
            {
                var length = grid.GetPath(new Position(1, 1), new Position(x, y), MovementPatterns.LateralOnly).Length;
                if (length != 0 && length - 2 < 50)
                    answer++;
            }
            return answer.ToString();
        }
    }
}