using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2019._6;

[UsedImplicitly]
public class Year2019Day06 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var root = ParseInput(input);
        return GetNumberOfConnections(root);
    }

    private static object GetNumberOfConnections(Node root)
    {
        var stack = new Stack<Node>();
        stack.Push(root);
        var answer = 0;
        while (stack.Count != 0)
        {
            var node = stack.Pop();

            answer += CountChildren(node);

            if (node.Left != null)
            {
                stack.Push(node.Left);
            }

            if (node.Right != null)
            {
                stack.Push(node.Right);
            }
        }

        return answer;
    }

    private static int CountChildren(Node root)
    {
        var stack = new Stack<Node>();
        stack.Push(root);
        var count = 0;
        while (stack.Count != 0)
        {
            var node = stack.Pop();
            if (node.Left != null)
            {
                stack.Push(node.Left);
                count++;
            }

            if (node.Right != null)
            {
                stack.Push(node.Right);
                count++;
            }
        }

        return count;
    }

    private static Node ParseInput(IEnumerable<string> input, string root = "COM")
    {
        var nodes = new Dictionary<string, Node>();
        var reg = new Regex(@"(?<left>\w+)\)(?<right>\w+)", RegexOptions.Compiled);
        foreach (var line in input)
        {
            var match = reg.Match(line);
            var left = match.Groups["left"].Value;
            var right = match.Groups["right"].Value;
            nodes.TryGetValue(left, out var leftNode);
            nodes.TryGetValue(right, out var rightNode);
            rightNode ??= new Node { Value = right };
            if (leftNode != null)
            {
                if (leftNode.Left == null)
                {
                    leftNode.Left = rightNode;
                }
                else
                {
                    if (leftNode.Right != null)
                        throw new Exception("Invalid input");
                    leftNode.Right = rightNode;
                }
            }
            else
            {
                leftNode = new Node { Value = left, Left = rightNode};
                nodes.Add(left, leftNode);
            }

            nodes.TryAdd(right, rightNode);
        }

        return nodes[root];
    }
	
    
    public object Part2(IEnumerable<string> input)
    {
        var root = ParseInput(input);
        var lca = GetLca(root);

        var pathForSanta = new Stack<Node>();
        GetPath(lca, "SAN", pathForSanta);
        var pathForYou = new Stack<Node>();
        GetPath(lca, "YOU", pathForYou);
        
        // -4 is for excluding root in path
        return (pathForSanta.Count + pathForYou.Count) - 4;
    }

    private static Node GetLca(Node root)
    {
        var queue = new Queue<Node>();
        queue.Enqueue(root);
        var lca = root;
        while (queue.Count != 0)
        {
            var node = queue.Dequeue();
            if (IsReachable(node, "YOU") && IsReachable(node, "SAN"))
            {
                lca = node;
            }

            if (node.Left != null)
            {
                queue.Enqueue(node.Left);
            }

            if (node.Right != null)
            {
                queue.Enqueue(node.Right);
            }
        }

        return lca;
    }

    private static bool GetPath(Node? root, string dest, Stack<Node> path)
    {
        if (root == null)
            return false;
        
        path.Push(root);

        if (root.Value == dest)
            return true;

        if ((root.Left != null && GetPath(root.Left, dest, path)) ||
            (root.Right != null && GetPath(root.Right, dest, path)))
        {
            return true;
        }

        path.Pop();
        return false;
    }

    private static bool IsReachable(Node root, string dest)
    {
        var stack = new Stack<Node>();
        stack.Push(root);
        while (stack.Count != 0)
        {
            var node = stack.Pop();
            if (node.Value == dest)
                return true;
            
            if (node.Left != null)
                stack.Push(node.Left);
            
            if (node.Right != null)
                stack.Push(node.Right);
        }

        return false;
    }
    
    private class Node
    {
        public Node? Left { get; set; }
        public Node? Right { get; set; }
        public string Value { get; set; }
    }
}