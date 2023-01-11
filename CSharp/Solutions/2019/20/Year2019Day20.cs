using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2019._20;

[UsedImplicitly]
public class Year2019Day20 : ISolution
{
    /*
     * Connections between portals, i.e. {"AA": {"AB": 123, "AZ": 33}, "AZ": {"AA": 123, "AC": 32}}
     * Then BFS from AA to ZZ
     */
    public object Part1(IEnumerable<string> input)
    {
        var (outerPortals, innerPortals, grid) = ParseGrid(input);

        var dists = GetDistanceBetweenPortals(outerPortals, grid, innerPortals);

        var queue = new Queue<((Position pos, string portal, string debug) k, int dist)>();
        queue.Enqueue(((outerPortals.Single(x => x.Value == "AA").Key, "AA", ""), 0));
        var visited = new HashSet<(Position pos, string portal)>();
        var minDist = int.MaxValue;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.dist > 1000)
                continue;
            
            /*
            if (visited.Contains((current.k.pos, current.k.portal)))
                continue;
            
            visited.Add((current.k.pos, current.k.portal));
            */
            foreach (var kv in dists[(current.k.pos, current.k.portal)])
            {
                if (kv.Key.portal == "AA")
                    continue;
                
                if (kv.Key.portal == "ZZ")
                {
                    minDist = Math.Min(kv.Value + current.dist, minDist);
                    continue;
                }
                
                queue.Enqueue(((kv.Key.position, kv.Key.portal, current.k.debug + " " + kv.Key.portal), current.dist + kv.Value + 1));
            }

        }
        
        return minDist;
    }

    private static DefaultDictionary<(Position position, string portal), DefaultDictionary<(Position position, string portal), int>> GetDistanceBetweenPortals(Dictionary<Position, string> outerPortals, Dictionary<Position, string> grid,
        Dictionary<Position, string> innerPortals)
    {
        var dists =
            new DefaultDictionary<(Position position, string portal),
                DefaultDictionary<(Position position, string portal), int>>();
        foreach (var kv in outerPortals)
        {
            var queue = new Queue<(Position position, int dist)>();
            queue.Enqueue((kv.Key, 0));
            var visited = new HashSet<Position>();

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (visited.Contains(current.position))
                    continue;

                if (!grid.ContainsKey(current.position) || grid[current.position] != ".")
                    continue;

                if (innerPortals.TryGetValue(current.position, out string value) && value != kv.Value)
                {
                    dists[(kv.Key, kv.Value)].Add((outerPortals.Single(x => x.Value == value).Key, value), current.dist);
                }

                if (outerPortals.TryGetValue(current.position, out string value2) && value2 != kv.Value)
                {
                    if (innerPortals.ContainsValue(value2))
                        dists[(kv.Key, kv.Value)].Add((innerPortals.Single(x => x.Value == value2).Key, value2),
                            current.dist);
                    else
                        dists[(kv.Key, kv.Value)].Add((current.position, value2), current.dist);
                }

                var diff = new List<(int x, int y)> { (0, 1), (0, -1), (1, 0), (-1, 0) };
                foreach (var (dx, dy) in diff)
                {
                    var newPos = new Position(current.position.X + dx, current.position.Y + dy);
                    queue.Enqueue((newPos, current.dist + 1));
                }

                visited.Add(current.position);
            }
        }

        foreach (var kv in innerPortals)
        {
            var queue = new Queue<(Position position, int dist)>();
            queue.Enqueue((kv.Key, 0));
            var visited = new HashSet<Position>();

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (visited.Contains(current.position))
                    continue;

                if (!grid.ContainsKey(current.position) || grid[current.position] != ".")
                    continue;

                if (innerPortals.TryGetValue(current.position, out string value) && value != kv.Value)
                {
                    dists[(kv.Key, kv.Value)].Add((outerPortals.Single(x => x.Value == value).Key, value),
                        current.dist);
                }

                if (outerPortals.TryGetValue(current.position, out string value2) && value2 != kv.Value)
                {
                    if (innerPortals.ContainsValue(value2))
                        dists[(kv.Key, kv.Value)].Add((innerPortals.Single(x => x.Value == value2).Key, value2),
                            current.dist);
                    else
                        dists[(kv.Key, kv.Value)].Add((current.position, value2), current.dist);
                }

                var diff = new List<(int x, int y)> { (0, 1), (0, -1), (1, 0), (-1, 0) };
                foreach (var (dx, dy) in diff)
                {
                    var newPos = new Position(current.position.X + dx, current.position.Y + dy);
                    queue.Enqueue((newPos, current.dist + 1));
                }

                visited.Add(current.position);
            }
        }

        return dists;
    }

    private class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : new() where TKey : notnull
    {
        public new TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var val)) 
                    return val;
                val = new TValue();
                Add(key, val);
                return val;
            }
            set => base[key] = value;
        }
    }


    /*
     * Should return Dictionary<string, Position> (string = portal, Position (x,y)
     * And Grid itself Dictionary<Position, string> (Position (x,y), string = tile)
     */
    private (Dictionary<Position, string> outerPortals, Dictionary<Position, string> innerPortals, Dictionary<Position, string> grid) ParseGrid(IEnumerable<string> input)
    {
        var outerPortals = new Dictionary<Position, string>();
        var innerPortals = new Dictionary<Position, string>();
        var grid = new Dictionary<Position, string>();

        var enumerable = input as string[] ?? input.ToArray();
        for (var y = 2; y < enumerable.Length - 2; y++)
        {
            for (var x = 2; x < enumerable[y].Length - 2; x++)
            {
                var tile = enumerable[y][x];
                if (string.IsNullOrEmpty(tile.ToString()))
                    continue;

                if (char.IsAsciiLetterUpper(tile))
                {
                    if (char.IsAsciiLetterUpper(enumerable[y][x + 1]) && enumerable[y][x - 1] == '.')
                        innerPortals.Add(new Position(x - 1, y), $"{tile}{enumerable[y][x + 1]}");
                    else if (char.IsAsciiLetterUpper(enumerable[y][x + 1]) && enumerable[y][x + 2] == '.')
                        innerPortals.Add(new Position(x + 2, y), $"{tile}{enumerable[y][x + 1]}");
                    else if (char.IsAsciiLetterUpper(enumerable[y + 1][x]) && enumerable[y - 1][x] == '.')
                        innerPortals.Add(new Position(x, y - 1), $"{tile}{enumerable[y + 1][x]}");
                    else if (char.IsAsciiLetterUpper(enumerable[y + 1][x]) && enumerable[y + 2][x] == '.') 
                        innerPortals.Add(new Position(x, y + 2), $"{tile}{enumerable[y + 1][x]}");
                }
                
                grid.Add(new Position(x, y), tile.ToString());
            }
        }

        for (var x = 0; x < enumerable.First().Length; x++)
        {
            var tile = enumerable[0][x];
            if (!char.IsAsciiLetterUpper(tile))
                continue;

            var portal = $"{tile}{enumerable[1][x]}";
            outerPortals.Add(new Position(x, 2), portal);
        }
        
        for (var x = 0; x < enumerable.First().Length; x++)
        {
            var tile = enumerable[^1][x];
            if (!char.IsAsciiLetterUpper(tile))
                continue;

            var portal = $"{enumerable[^2][x]}{tile}";
            outerPortals.Add(new Position(x, enumerable.Length - 3), portal);
        }
        
        for (var y = 0; y < enumerable.Length; y++)
        {
            var tile = enumerable[y][0];
            if (!char.IsAsciiLetterUpper(tile))
                continue;

            var portal = $"{tile}{enumerable[y][1]}";
            outerPortals.Add(new Position(2, y), portal);
        }
        
        for (var y = 0; y < enumerable.Length; y++)
        {
            var tile = enumerable[y][^1];
            if (!char.IsAsciiLetterUpper(tile))
                continue;

            var portal = $"{enumerable[y][^2]}{tile}";
            outerPortals.Add(new Position(enumerable.First().Length - 3, y), portal);
        }

        return (outerPortals, innerPortals, grid);
    }

    public object Part2(IEnumerable<string> input)
    {
        var (outerPortals, innerPortals, grid) = ParseGrid(input);

        var dists = GetDistanceBetweenPortals(outerPortals, grid, innerPortals);

        var queue = new Queue<((Position pos, string portal, int depth, string debug) k, int dist)>();
        queue.Enqueue(((outerPortals.Single(x => x.Value == "AA").Key, "AA", 0, ""), 0));
        var visited = new HashSet<(Position pos, string portal, int depth)>();
        var minDist = int.MaxValue;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            
            if (visited.Contains((current.k.pos, current.k.portal, current.k.depth)))
                continue;
            
            visited.Add((current.k.pos, current.k.portal, current.k.depth));
            foreach (var kv in dists[(current.k.pos, current.k.portal)])
            {
                if (current.k.portal == "AA" && current.k.depth != 0)
                    continue;
                if (current.k.depth < 0 || current.k.depth > 100)
                    continue;
                
                if (kv.Key.portal == "ZZ" && current.k.depth == 0)
                {
                    minDist = Math.Min(kv.Value + current.dist, minDist);
                    continue;
                }
                
                if (kv.Key.portal == "ZZ" && current.k.depth != 0)
                    continue;
                if (kv.Key.portal == "AA" && current.k.depth != 0)
                    continue;
                
                if (current.k.depth == 0 && innerPortals.ContainsKey(kv.Key.position)) 
                    continue;
                
                if (outerPortals.ContainsKey(kv.Key.position))
                    queue.Enqueue(((kv.Key.position, kv.Key.portal, current.k.depth + 1, current.k.debug + " +" + kv.Key.portal), current.dist + kv.Value + 1));
                else if (innerPortals.ContainsKey(kv.Key.position))
                    queue.Enqueue(((kv.Key.position, kv.Key.portal, current.k.depth - 1, current.k.debug + " -" + kv.Key.portal), current.dist + kv.Value + 1));
            }
        }
        
        return minDist;
    }
}