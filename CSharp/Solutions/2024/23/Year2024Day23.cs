using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2024._23;

[UsedImplicitly]
public class Year2024Day23 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var connections = ParseInput(input);

        var set = new HashSet<string>();

        foreach (var connection in connections)
        {
            var l = new List<string> { connection.Key };
            foreach (var hs in connection.Value)
            {
                l.Add(hs);
                foreach (var v in connections[hs])
                {
                    if (!connection.Value.Contains(v))
                        continue;
                    
                    l.Add(v);
                    l.Sort();
                    set.Add(string.Join('-', l));
                    l.Remove(v);
                }

                l.Remove(hs);
            }
        }

        var answer = set.Select(s => s.Split("-")).Count(split => split.Any(c => c.StartsWith('t')));

        return answer;
    }

    public object Part2(IEnumerable<string> input)
    {
        var connections = ParseInput(input);
        return CountMaxClique(connections);
    }
    
    private static Dictionary<string, HashSet<string>> ParseInput(IEnumerable<string> input)
    {
        var connections = new Dictionary<string, HashSet<string>>();
        foreach (var line in input)
        {
            var split = line.Split('-');
            if (!connections.ContainsKey(split[0]))
                connections[split[0]] = new HashSet<string>();
            
            if (!connections.ContainsKey(split[1]))
                connections[split[1]] = new HashSet<string>();
            
            connections[split[0]].Add(split[1]);
            connections[split[1]].Add(split[0]);
        }

        return connections;
    }
    
    private static string CountMaxClique(Dictionary<string, HashSet<string>> graph)
    {
        var cliques = new List<HashSet<string>>();
        BronKerbosch(graph, r: new HashSet<string>(), p: new HashSet<string>(graph.Keys), x: new HashSet<string>(), cliques);
        var max = cliques.MaxBy(clique => clique.Count);
        var str = string.Join(",", max!.Order());
        return str;
    }
    
    private static void BronKerbosch(Dictionary<string, HashSet<string>> graph, HashSet<string> r, HashSet<string> p, HashSet<string> x,
        List<HashSet<string>> cliques)
    {
        if (p.Count == 0 && x.Count == 0)
        {
            cliques.Add(new HashSet<string>(r));
            return;
        }

        var pivot = p.Concat(x).First();
        var nonNeighbors = new HashSet<string>(p.Except(graph[pivot]));

        foreach (var v in nonNeighbors)
        {
            var newR = new HashSet<string>(r) { v };
            var newP = new HashSet<string>(p.Intersect(graph[v]));
            var newX = new HashSet<string>(x.Intersect(graph[v]));
            BronKerbosch(graph, newR, newP, newX, cliques);
            p.Remove(v);
            x.Add(v);
        }
    }
}