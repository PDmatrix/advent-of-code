using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2019._12;

[UsedImplicitly]
public class Year2019Day12 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var moons = ParseInput(input);
        
        const int stepCount = 1000;
        for (var i = 0; i < stepCount; i++)
        {
            Step(moons);
        }

        var result = 0;
        foreach (var moon in moons)
        {
            var pot = Math.Abs(moon.pos.X) + Math.Abs(moon.pos.Y) + Math.Abs(moon.pos.Z);
            var kin = Math.Abs(moon.vel.X) + Math.Abs(moon.vel.Y) + Math.Abs(moon.vel.Z);
            result += pot * kin;
        }
        
        return result;
    }

    private static void Step(List<(Dimension pos, Dimension vel)> moons)
    {
        for (var i = 0; i < moons.Count; i++)
        {
            for (var j = i + 1; j < moons.Count; j++)
            {
                if (moons[i].pos.X != moons[j].pos.X)
                {
                    var newVal = moons[i].pos.X > moons[j].pos.X ? moons[i].vel.X - 1 : moons[i].vel.X + 1;
                    moons[i] = (moons[i].pos, moons[i].vel with { X = newVal });
                    newVal = moons[j].pos.X > moons[i].pos.X ? moons[j].vel.X - 1 : moons[j].vel.X + 1;
                    moons[j] = (moons[j].pos, moons[j].vel with { X = newVal });
                }

                if (moons[i].pos.Y != moons[j].pos.Y)
                {
                    var newVal = moons[i].pos.Y > moons[j].pos.Y ? moons[i].vel.Y - 1 : moons[i].vel.Y + 1;
                    moons[i] = (moons[i].pos, moons[i].vel with { Y = newVal });
                    newVal = moons[j].pos.Y > moons[i].pos.Y ? moons[j].vel.Y - 1 : moons[j].vel.Y + 1;
                    moons[j] = (moons[j].pos, moons[j].vel with { Y = newVal });
                }

                if (moons[i].pos.Z != moons[j].pos.Z)
                {
                    var newVal = moons[i].pos.Z > moons[j].pos.Z ? moons[i].vel.Z - 1 : moons[i].vel.Z + 1;
                    moons[i] = (moons[i].pos, moons[i].vel with { Z = newVal });
                    newVal = moons[j].pos.Z > moons[i].pos.Z ? moons[j].vel.Z - 1 : moons[j].vel.Z + 1;
                    moons[j] = (moons[j].pos, moons[j].vel with { Z = newVal });
                }
            }
        }

        for (var i = 0; i < moons.Count; i++)
        {
            moons[i] = (
                new Dimension(X: moons[i].pos.X + moons[i].vel.X, Y: moons[i].pos.Y + moons[i].vel.Y,
                    Z: moons[i].pos.Z + moons[i].vel.Z), moons[i].vel);
        }
    }


    private record struct Dimension(int X, int Y, int Z);
    
    private static long Gcd(long a, long b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }

        return a | b;
    }

    private static long Lca(long a, long b)
    {
        return a / Gcd(a, b) * b;
    }
    
    public object Part2(IEnumerable<string> input)
    {
        var moons = ParseInput(input);

        long stepCount = 0;
        long repX = 0;
        long repY = 0;
        long repZ = 0;
        while (true)
        {
            if (repX != 0 && repY != 0 && repZ != 0)
                break;
            Step(moons);
            stepCount++;
            if (repX == 0 && moons.All(x => x.vel.X == 0))
                repX = stepCount;
            if (repY == 0 && moons.All(x => x.vel.Y == 0))
                repY = stepCount;
            if (repZ == 0 && moons.All(x => x.vel.Z == 0))
                repZ = stepCount;
        }

        return Lca(Lca(repX, repY), repZ) * 2;
    }

    private static List<(Dimension pos, Dimension vel)> ParseInput(IEnumerable<string> input)
    {
        var moons = new List<(Dimension pos, Dimension vel)>();
        var regex = new Regex(@"<x=(?<x>-?\d+), y=(?<y>-?\d+), z=(?<z>-?\d+)>", RegexOptions.Compiled);
        foreach (var line in input)
        {
            var match = regex.Match(line);
            moons.Add((new Dimension(int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value),
                int.Parse(match.Groups["z"].Value)), new Dimension(0, 0, 0)));
        }

        return moons;
    }
}