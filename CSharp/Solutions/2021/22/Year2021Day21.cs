using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2021._22;

[UsedImplicitly]
public class Year2021Day22 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var grid = new Dictionary<Vector3, bool>();

        var regex = new Regex(
            @"(?<state>\w+) x=(?<from_x>-?\d+)\.\.(?<to_x>-?\d+),y=(?<from_y>-?\d+)\.\.(?<to_y>-?\d+),z=(?<from_z>-?\d+)\.\.(?<to_z>-?\d+)");
        foreach (var line in input)
        {
            var match = regex.Match(line);
            var state = match.Groups["state"].Value == "on";
            var fromX = int.Parse(match.Groups["from_x"].Value);
            var fromY = int.Parse(match.Groups["from_y"].Value);
            var fromZ = int.Parse(match.Groups["from_z"].Value);
            var toX = int.Parse(match.Groups["to_x"].Value);
            var toY = int.Parse(match.Groups["to_y"].Value);
            var toZ = int.Parse(match.Groups["to_z"].Value);

            for (var x = fromX; x <= toX; x++)
            {
                if (x is < -50 or > 50)
                    continue;
                for (var y = fromY; y <= toY; y++)
                {
                    if (y is < -50 or > 50)
                        continue;
                    for (var z = fromZ; z <= toZ; z++)
                    {
                        if (z is < -50 or > 50)
                            continue;

                        grid[new Vector3(x, y, z)] = state;
                    }
                }
            }
        }

        return grid.Count(x => x.Value);
    }

    public object Part2(IEnumerable<string> input)
    {
        var regex = new Regex(
            @"(?<state>\w+) x=(?<from_x>-?\d+)\.\.(?<to_x>-?\d+),y=(?<from_y>-?\d+)\.\.(?<to_y>-?\d+),z=(?<from_z>-?\d+)\.\.(?<to_z>-?\d+)");
        var commands = new List<Command>();
        foreach (var line in input)
        {
            var match = regex.Match(line);
            var state = Enum.Parse<Action>(match.Groups["state"].Value, true);
            var fromX = int.Parse(match.Groups["from_x"].Value);
            var fromY = int.Parse(match.Groups["from_y"].Value);
            var fromZ = int.Parse(match.Groups["from_z"].Value);
            var toX = int.Parse(match.Groups["to_x"].Value);
            var toY = int.Parse(match.Groups["to_y"].Value);
            var toZ = int.Parse(match.Groups["to_z"].Value);

            var x = new Interval(fromX, toX);
            var y = new Interval(fromY, toY);
            var z = new Interval(fromZ, toZ);
            commands.Add(new Command(state, new Cube(x, y, z)));
        }


        return ExecuteCommands(commands);
    }

    long ExecuteCommands(IEnumerable<Command> commands)
    {
        var regionsTurnedOn = ImmutableList.Create<Cube>();

        foreach (var cmd in commands)
        {
            switch (cmd.Action)
            {
                case Action.On:
                    var regionsToEnable = ImmutableList.Create(cmd.Cube);
                    foreach (var existing in regionsTurnedOn)
                    {
                        regionsToEnable = regionsToEnable.SelectMany(a => a.Subtract(existing))
                            .Where(a => !a.IsEmpty)
                            .ToImmutableList();
                    }

                    regionsTurnedOn = regionsTurnedOn.AddRange(regionsToEnable);
                    break;

                case Action.Off:
                    regionsTurnedOn = regionsTurnedOn.SelectMany(a => a.Subtract(cmd.Cube))
                        .Where(a => !a.IsEmpty)
                        .ToImmutableList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return regionsTurnedOn.Sum(r => r.Volume);
    }

    private record struct Interval(long Begin, long End)
    {
        public static readonly Interval Empty = new(0, -1);

        public bool IsEmpty => Size == 0L;

        public long Size => Math.Max(0, End - Begin + 1);

        public Interval Intersect(Interval other)
        {
            var newBegin = Math.Max(Begin, other.Begin);
            var newEnd = Math.Min(End, other.End);
            return newBegin <= newEnd
                ? new Interval(newBegin, newEnd)
                : Empty;
        }

        public IEnumerable<Interval> Split(Interval other)
        {
            var intersect = Intersect(other);
            if (intersect.IsEmpty)
            {
                yield return this;
            }
            else
            {
                if (intersect.Begin > Begin)
                    yield return this with { End = intersect.Begin - 1 };

                yield return intersect;

                if (intersect.End < End)
                    yield return this with { Begin = intersect.End + 1 };
            }
        }
    }

    private record struct Cube(Interval X, Interval Y, Interval Z)
    {
        public long Volume
            => X.Size * Y.Size * Z.Size;

        public bool IsEmpty => X.IsEmpty || Y.IsEmpty || Z.IsEmpty;

        private Cube Intersect(Cube other) => new(X.Intersect(other.X), Y.Intersect(other.Y), Z.Intersect(other.Z));

        private IEnumerable<Cube> SplitX(Interval interval)
        {
            var (y, z) = (Y, Z);
            return X.Split(interval).Select(x => new Cube(x, y, z));
        }

        private IEnumerable<Cube> SplitY(Interval interval)
        {
            var (x, z) = (X, Z);
            return Y.Split(interval).Select(y => new Cube(x, y, z));
        }

        private IEnumerable<Cube> SplitZ(Interval interval)
        {
            var (x, y) = (X, Y);
            return Z.Split(interval).Select(z => new Cube(x, y, z));
        }

        public IEnumerable<Cube> Subtract(Cube other)
        {
            var intersection = Intersect(other);

            if (intersection.IsEmpty)
                return Enumerable.Repeat(this, 1);
            if (intersection == this)
                return Enumerable.Empty<Cube>();

            return SplitX(intersection.X).SelectMany(a => a.SplitY(intersection.Y))
                .SelectMany(a => a.SplitZ(intersection.Z))
                .Where(a => a != intersection);
        }
    }

    private record Command(Action Action, Cube Cube);

    private enum Action
    {
        On,
        Off
    }
}