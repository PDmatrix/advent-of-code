using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;
using JetBrains.Annotations;
using RoyT.AStar;

namespace AdventOfCode.Solutions._2019._18;

[UsedImplicitly]
public class Year2019Day18 : ISolution
{
    private static Dictionary<string, int> _memo = new();

    public object Part1(IEnumerable<string> input)
    {
        var (maze, robotPosition) = ParseGrid(input);

        return MultiShortestPath(new List<Position> { robotPosition }, maze, GetKeysFromMaze(maze));
    }

    private static List<string> GetKeysFromMaze(Dictionary<Position, string> maze)
    {
        var keys = new List<string>();
        foreach (var kv in maze)
        {
            if (kv.Value[0] >= 'a' && kv.Value[0] <= 'z')
                keys.Add(kv.Value);
        }

        return keys;
    }

    private static string GetMemoKey(List<Position> positions, List<string> need)
    {
        return $"{string.Join(string.Empty, positions)}-{string.Join(string.Empty, need)}";
    }
    
    private static Dictionary<string, BfsNode> GetNeighbors(Dictionary<Position, string> maze, Position position)
    {
        var neighbors = new Dictionary<string, BfsNode>();
        var queue = new Queue<BfsNode>();
        queue.Enqueue(new BfsNode(position, maze[position], 0));
        var visited = new HashSet<Position> { position };
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach (var adjacent in GetAdjacent(maze, current.Position))
            {
                if (visited.Contains(adjacent))
                    continue;

                visited.Add(adjacent);

                var c = maze[adjacent];
                var nd = new BfsNode(adjacent, c, current.Distance + 1);
                if ((c[0] >= 'A' && c[0] <= 'Z') || (c[0] >= 'a' && c[0] <= 'z'))
                    neighbors[c] = nd;
                else
                    queue.Enqueue(nd);
            }
        }

        return neighbors;
    }

    private record BfsNode(Position Position, string Cell, int Distance);
    private static List<Position> GetAdjacent(Dictionary<Position, string> m, Position p)
    {
        var adjacent = new List<Position>();
        var diff = new[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
        foreach (var (dx, dy) in diff)
        {
            var newPos = new Position(p.X + dx, p.Y + dy);
            if (m.TryGetValue(newPos, out string value) && value != "#")
                adjacent.Add(newPos);
        }

        return adjacent;
    }
    private int MultiShortestPath(List<Position> positions, Dictionary<Position, string> maze, List<string> need)
    {
        var memoKey = GetMemoKey(positions, need);
        if (_memo.ContainsKey(memoKey))
            return _memo[memoKey];

        if (need.Count == 0)
            return 0;

        var shortest = 0;
        for (int j = 0; j < positions.Count; j++)
        {
            var neighbors = GetNeighbors(maze, positions[j]);
            for (int i = 0; i < need.Count; i++)
            {
                if (!neighbors.ContainsKey(need[i]))
                    continue;
                
                var neighbor = neighbors[need[i]];
                var clonedMaze = maze.ToDictionary(x => x.Key, x => x.Value);
                clonedMaze[neighbor.Position] = ".";
                if (maze.ContainsValue(neighbor.Cell.ToUpper()))
                {
                    clonedMaze[maze.First(x => x.Value == neighbor.Cell.ToUpper()).Key] = ".";
                }

                var subSteps = MultiShortestPath(ReplaceFromList(positions, j, neighbor.Position), clonedMaze, RemoveFromList(need, i)) + neighbor.Distance;
                if (subSteps >= 0 && (shortest == 0 || subSteps < shortest))
                    shortest = subSteps;
            }
        }

        if (shortest <= 0) 
            return -1;
        
        _memo[memoKey] = shortest;
        return shortest;
    }

    private static List<Position> ReplaceFromList(List<Position> positions, int index, Position position)
    {
        var cloned = new List<Position>(positions)
        {
            [index] = position
        };
        
        return cloned;
    }
    
    private static List<string> RemoveFromList(List<string> positions, int index)
    {
        var cloned = new List<string>(positions);
        cloned.RemoveAt(index);
        
        return cloned;
    }
    
    private static (Dictionary<Position, string> grid, Position robotPosition) ParseGrid(IEnumerable<string> input)
    {
        var grid = new Dictionary<Position, string>();

        var robotPosition = new Position(0, 0);

        var enumerable = input as string[] ?? input.ToArray();
        for (var y = 0; y < enumerable.Length; y++)
        {
            for (var x = 0; x < enumerable[y].Length; x++)
            {
                var cell = enumerable[y][x];
                grid.Add(new Position(x, y), cell.ToString());

                if (cell == '@')
                    robotPosition = new Position(x, y);
            }
        }

        return (grid, robotPosition);
    }

    private static List<Position> SplitMaze(Dictionary<Position, string> maze, Position atPoint)
    {
        maze[atPoint] = "#";
     
        var diff = new[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
        foreach (var (dx, dy) in diff)
        {
            var newPos = new Position(atPoint.X + dx, atPoint.Y + dy);
            maze[newPos] = "#";
        }

        var newRobotPositions = new List<Position>();
        var diff2 = new[] { ("1", -1, -1), ("2", 1, 1), ("3", 1, -1), ("4", -1, 1) };
        foreach (var (i, dx, dy) in diff2)
        {
            var newPosition = new Position(atPoint.X + dx, atPoint.Y + dy);
            maze[newPosition] = i;
            newRobotPositions.Add(newPosition);
        }

        return newRobotPositions;
    }

    public object Part2(IEnumerable<string> input)
    {
        _memo = new Dictionary<string, int>();
        var (maze, robotPosition) = ParseGrid(input);
        var newRobotPositions = SplitMaze(maze, robotPosition);
        
        return MultiShortestPath(newRobotPositions, maze, GetKeysFromMaze(maze));
    }
}