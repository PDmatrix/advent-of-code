using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._21;

[UsedImplicitly]
public class Year2021Day21 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var positions = new List<int>();
        foreach (var line in input)
        {
            var splitted = line.Split(": ");
            positions.Add(int.Parse(splitted[1]));
        }

        var scores = new List<int>
        {
            0, 0
        };

        var dice = 1;
        var diceRolls = 0;
        var currentPlayer = 0;
        while (!scores.Any(x => x >= 1000))
        {
            var result = 0;
            diceRolls += 3;
            for (var i = 0; i < 3; i++)
            {
                result += dice;
                dice++;
            }
            
            positions[currentPlayer] += result;
            
            if (positions[currentPlayer] > 10)
                positions[currentPlayer] %= 10;
            
            if (positions[currentPlayer] == 0)
                positions[currentPlayer] = 10;

            
            scores[currentPlayer] += positions[currentPlayer];
            
            currentPlayer = currentPlayer == 0 ? 1 : 0;
        }
        
        
        return scores.Min() * diceRolls;
    }

    public object Part2(IEnumerable<string> input)
    {
        var positions = new List<int>();
        foreach (var line in input)
        {
            var splitted = line.Split(": ");
            positions.Add(int.Parse(splitted[1]));
        }

        var answer = Play((positions[0], positions[1]), (0, 0));
        return Math.Max(answer.p1, answer.p2);
    }

    private static Dictionary<(int p1, int p2, int s1, int s2), (long p1, long p2)> _dp = new();

    private static (long p1, long p2) Play((int p1, int p2) position, (int p1, int p2) score)
    {
        if (score.p1 >= 21)
            return (1, 0);
        if (score.p2 >= 21)
            return (0, 1);

        if (_dp.ContainsKey((position.p1, position.p2, score.p1, score.p2)))
            return _dp[(position.p1, position.p2, score.p1, score.p2)];

        var answer = ((long)0, (long)0);
        for (var i = 1; i <= 3; i++)
        {
            for (var j = 1; j <= 3; j++)
            {
                for (var k = 1; k <= 3; k++)
                {
                    var newP1 = position.p1 + i + j + k;
                    if (newP1 > 10)
                        newP1 %= 10;
            
                    if (newP1 == 0)
                        newP1 = 10;

                    var newS1 = score.p1 + newP1;

                    var (x1, y1) = Play((position.p2, newP1), (score.p2, newS1));
                    answer = (answer.Item1 + y1, answer.Item2 + x1);
                }
            }
        }

        _dp[(position.p1, position.p2, score.p1, score.p2)] = answer;
        return answer;
    }
}