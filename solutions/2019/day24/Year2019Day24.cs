using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._24;

[UsedImplicitly]
public class Year2019Day24 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var grid = ParseGrid(input);
        var patterns = new Dictionary<int, List<List<bool>>> { { GetHash(grid), grid.Select(x => x.ToList()).ToList() } };
        while (true)
        {
            var newGrid = grid.Select(x => x.ToList()).ToList();
            for (var y = 0; y < grid.Count; y++)
            {
                for (var x = 0; x < grid[y].Count; x++)
                {
                    var adjacentCount = GetAdjacentCount(grid, x, y);
                    if (grid[y][x] && adjacentCount != 1)
                        newGrid[y][x] = false;
                    if (!grid[y][x] && adjacentCount is 1 or 2)
                        newGrid[y][x] = true;
                }
            }

            grid = newGrid.Select(x => x.ToList()).ToList();
            var newGridHash = GetHash(grid);
            
            if (patterns.ContainsKey(newGridHash))
                break;
            
            patterns.Add(newGridHash, newGrid.Select(x => x.ToList()).ToList());
        }
        
        return GetBiodiversityRating(grid);
    }

    private static int GetBiodiversityRating(List<List<bool>> grid)
    {
        var index = 1;
        var rating = 0;
        for (var y = 0; y < grid.Count; y++)
        {
            for (var x = 0; x < grid[y].Count; x++)
            {
                if (grid[y][x])
                    rating += index;
                index *= 2;
            }
        }

        return rating;
    }

    private static int GetAdjacentCount(List<List<bool>> grid, int x, int y)
    {
        var adjacentCount = 0;
        var diff = new List<(int x, int y)> { (0, 1), (0, -1), (1, 0), (-1, 0) };
        foreach (var (dx, dy) in diff)
        {
            var newX = x + dx;
            var newY = y + dy;
            if (newX < 0 || newX >= grid.First().Count)
                continue;
            if (newY < 0 || newY >= grid.Count)
                continue;

            if (grid[newY][newX])
                adjacentCount++;
        }

        return adjacentCount;
    }
    
    private static int GetAdjacentCountPart2(Dictionary<(int x, int y, int z), bool> grid, int x, int y, int z)
    {
        var adjacentCount = 0;

        if (x == 0)
            adjacentCount += grid.GetValueOrDefault((1, 2, z - 1), false) ? 1 : 0;
        if (x == 4)
            adjacentCount += grid.GetValueOrDefault((3, 2, z - 1), false) ? 1 : 0;
        if (y == 0)
            adjacentCount += grid.GetValueOrDefault((2, 1, z - 1), false) ? 1 : 0;
        if (y == 4)
            adjacentCount += grid.GetValueOrDefault((2, 3, z - 1), false) ? 1 : 0;

        if ((x, y) == (1, 2))
            adjacentCount += Enumerable.Range(0, 5).Select(y => grid.GetValueOrDefault((0, y, z + 1), false) ? 1 : 0).Sum();
        if ((x, y) == (3, 2))
            adjacentCount += Enumerable.Range(0, 5).Select(y => grid.GetValueOrDefault((4, y, z + 1), false) ? 1 : 0).Sum();
        if ((x, y) == (2, 1))
            adjacentCount += Enumerable.Range(0, 5).Select(x => grid.GetValueOrDefault((x, 0, z + 1), false) ? 1 : 0).Sum();
        if ((x, y) == (2, 3))
            adjacentCount += Enumerable.Range(0, 5).Select(x => grid.GetValueOrDefault((x, 4, z + 1), false) ? 1 : 0).Sum();

        var diff = new List<(int x, int y)> { (0, 1), (0, -1), (1, 0), (-1, 0) };
        foreach (var (dx, dy) in diff)
        {
            var newX = x + dx;
            var newY = y + dy;
            
            if (newX == 2 && newY == 2)
                continue;
            if (newX is < 0 or >= 5)
                continue;
            if (newY is < 0 or >= 5)
                continue;

            if (grid.GetValueOrDefault((newX, newY, z), false))
                adjacentCount++;
        }

        return adjacentCount;
    }


    private static int GetHash(List<List<bool>> grid)
    {
        unchecked
        {
            var hash = 19;
            foreach (var row in grid)
            {
                foreach (var cell in row)
                {
                    hash = hash * 31 + (cell ? 1 : 0);
                }
            }

            return hash;
        }
    }

    private static List<List<bool>> ParseGrid(IEnumerable<string> input)
    {
        var grid = new List<List<bool>>();
        var enumerable = input as string[] ?? input.ToArray();
        for (var y = 0; y < enumerable.Length; y++)
        {
            var row = new List<bool>();
            for (int x = 0; x < enumerable[y].Length; x++)
            {
                row.Add(enumerable[y][x] == '#');
            }
            grid.Add(row);
        }

        return grid;
    }
    
    private static Dictionary<(int x, int y, int z), bool> ParseGridPart2(IEnumerable<string> input)
    {
        var grid = new Dictionary<(int x, int y, int z), bool>();
        var enumerable = input as string[] ?? input.ToArray();
        for (var y = 0; y < enumerable.Length; y++)
        {
            for (var x = 0; x < enumerable[y].Length; x++)
            {
                grid.Add((x, y, 0), enumerable[y][x] == '#');
            }
        }

        return grid;
    }


    public object Part2(IEnumerable<string> input)
    {
        var grid = ParseGridPart2(input);
        for(var minute = 0; minute < 200; minute++)
        {
            var newGrid = grid.ToDictionary(x => x.Key, x => x.Value);
            var minZ = grid.Min(x => x.Key.z);
            var maxZ = grid.Max(x => x.Key.z);
            for (var z = minZ - 1; z <= maxZ + 1; z++)
            {
                for (var y = 0; y < 5; y++)
                {
                    for (var x = 0; x < 5; x++)
                    {
                        if (x == 2 && y == 2)
                            continue;
                        
                        var adjacentCount = GetAdjacentCountPart2(grid, x, y, z);
                        if (grid.GetValueOrDefault((x, y, z), false) && adjacentCount != 1)
                            newGrid[(x, y, z)] = false;
                        else if (adjacentCount is 1 or 2)
                            newGrid[(x, y, z)] = true;
                    }
                }
            }

            grid = newGrid.ToDictionary(x => x.Key, x => x.Value);
        }

        return grid.Count(x => x.Value);
    }
}