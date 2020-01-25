using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2017._22
{
    // ReSharper disable once UnusedMember.Global
    public class Year2017Day22 : ISolution
    {
        private const string Directions = "URDLURDL";
        private const string PossibleStates = ".W#F.";
        
        public string Part1(IEnumerable<string> input)
        {
            var gridState = GetGridState(input);

            var currentPosition = (x: 12, y: 12);

            var currentDirection = 'U';
            var newInfections = 0;
            for (var i = 0; i < 10000; i++)
            {
                var isInfected =
                    gridState.GetValueOrDefault(currentPosition, '.') == '#';

                currentDirection =
                    Directions[Directions.IndexOf(currentDirection) + (isInfected ? 1 : 3)];

                if (isInfected)
                {
                    gridState.Remove(currentPosition);
                }
                else
                {
                    newInfections++;
                    gridState[currentPosition] = '#';
                }

                currentPosition = GetNewPosition(currentPosition, currentDirection);
            }

            return newInfections.ToString();
        }

        public string Part2(IEnumerable<string> input)
        {
            var gridState = GetGridState(input);

            var currentPosition = (x: 12, y: 12);

            
            var currentDirection = 'U';
            var newInfections = 0;
            for (var i = 0; i < 10000000; i++)
            {
                var currentState = gridState.GetValueOrDefault(currentPosition, '.');

                currentDirection = currentState switch
                {
                    '.' => Directions[Directions.IndexOf(currentDirection) + 3],
                    '#' => Directions[Directions.IndexOf(currentDirection) + 1],
                    'F' => Directions[Directions.IndexOf(currentDirection) + 2],
                    _ => currentDirection
                };
                
                var newState = PossibleStates[PossibleStates.IndexOf(currentState) + 1];
                if (newState == '#')
                    newInfections++;
                
                gridState[currentPosition] = newState;

                currentPosition = GetNewPosition(currentPosition, currentDirection);
            }

            return newInfections.ToString();
        }

        private static Dictionary<(int x, int y), char> GetGridState(IEnumerable<string> input)
            => input.SelectMany((row, y) =>
                    row.Select((state, x) => (state, x, y)))
                .ToDictionary(key => (key.x, key.y), value => value.state);

        private static (int x, int y) GetNewPosition((int x, int y) position, char direction)
            => direction switch
            {
                'U' => (position.x, position.y - 1),
                'D' => (position.x, position.y + 1),
                'L' => (position.x - 1, position.y),
                'R' => (position.x + 1, position.y),
                _ => throw new Exception("Unexpected position")
            };

    }
}