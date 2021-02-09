using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._21
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day21 : ISolution
    {
        private const string SwapByIndexRegex = @"swap position (?<x>\d+) with position (?<y>\d+)";
        private const string SwapByLetterRegex = @"swap letter (?<x>\w+) with letter (?<y>\w+)";
        private const string RotateRegex = @"rotate (?<dir>left|right) (?<x>\d+) (step|steps)";
        private const string RotateByPositionRegex = @"rotate based on position of letter (?<x>\w+)";
        private const string ReversePositionsRegex = @"reverse positions (?<x>\d+) through (?<y>\d+)";
        private const string MovePositionRegex = @"move position (?<x>\d+) to position (?<y>\d+)";

        public object Part1(IEnumerable<string> input)
        {
            var password = "abcdefgh";
            foreach (var operation in input)
            {
                if (Regex.IsMatch(operation, SwapByIndexRegex))
                {
                    var match = Regex.Match(operation, SwapByIndexRegex);
                    var x = int.Parse(match.Groups["x"].Value);
                    var y = int.Parse(match.Groups["y"].Value);
                    password = SwapByIndex(password, x, y);
                }
                else if (Regex.IsMatch(operation, SwapByLetterRegex))
                {
                    var match = Regex.Match(operation, SwapByLetterRegex);
                    var x = match.Groups["x"].Value[0];
                    var y = match.Groups["y"].Value[0];
                    password = SwapByLetter(password, x, y);
                }
                else if (Regex.IsMatch(operation, RotateRegex))
                {
                    var match = Regex.Match(operation, RotateRegex);
                    var direction = match.Groups["dir"].Value;
                    var x = int.Parse(match.Groups["x"].Value);
                    password = direction == "left"
                        ? RotateLeft(password, x)
                        : RotateRight(password, x);
                }
                else if (Regex.IsMatch(operation, RotateByPositionRegex))
                {
                    var match = Regex.Match(operation, RotateByPositionRegex);
                    var x = match.Groups["x"].Value[0];
                    password = RotateByPosition(password, x);
                }
                else if (Regex.IsMatch(operation, ReversePositionsRegex))
                {
                    var match = Regex.Match(operation, ReversePositionsRegex);
                    var x = int.Parse(match.Groups["x"].Value);
                    var y = int.Parse(match.Groups["y"].Value);
                    password = ReversePositions(password, x, y);
                }
                else if (Regex.IsMatch(operation, MovePositionRegex))
                {
                    var match = Regex.Match(operation, MovePositionRegex);
                    var x = int.Parse(match.Groups["x"].Value);
                    var y = int.Parse(match.Groups["y"].Value);
                    password = Move(password, x, y);
                }
            }

            return password;
        }

        public object Part2(IEnumerable<string> input)
        {
            input = input.Reverse();
            var scrambled = "fbgdceah";
            foreach (var operation in input)
            {
                if (Regex.IsMatch(operation, SwapByIndexRegex))
                {
                    var match = Regex.Match(operation, SwapByIndexRegex);
                    var x = int.Parse(match.Groups["x"].Value);
                    var y = int.Parse(match.Groups["y"].Value);
                    scrambled = SwapByIndex(scrambled, x, y);
                }
                else if (Regex.IsMatch(operation, SwapByLetterRegex))
                {
                    var match = Regex.Match(operation, SwapByLetterRegex);
                    var x = match.Groups["x"].Value[0];
                    var y = match.Groups["y"].Value[0];
                    scrambled = SwapByLetter(scrambled, x, y);
                }
                else if (Regex.IsMatch(operation, RotateRegex))
                {
                    var match = Regex.Match(operation, RotateRegex);
                    var direction = match.Groups["dir"].Value;
                    var x = int.Parse(match.Groups["x"].Value);
                    scrambled = direction == "left"
                        ? RotateRight(scrambled, x)
                        : RotateLeft(scrambled, x);
                }
                else if (Regex.IsMatch(operation, RotateByPositionRegex))
                {
                    var match = Regex.Match(operation, RotateByPositionRegex);
                    var x = match.Groups["x"].Value[0];
                    scrambled = RotateByPosition(scrambled, x, true);
                }
                else if (Regex.IsMatch(operation, ReversePositionsRegex))
                {
                    var match = Regex.Match(operation, ReversePositionsRegex);
                    var x = int.Parse(match.Groups["x"].Value);
                    var y = int.Parse(match.Groups["y"].Value);
                    scrambled = ReversePositions(scrambled, x, y);
                }
                else if (Regex.IsMatch(operation, MovePositionRegex))
                {
                    var match = Regex.Match(operation, MovePositionRegex);
                    var x = int.Parse(match.Groups["x"].Value);
                    var y = int.Parse(match.Groups["y"].Value);
                    scrambled = Move(scrambled, y, x);
                }
            }

            return scrambled;
        }

        private static string SwapByIndex(string password, int first, int second)
        {
            var charPassword = password.ToCharArray();
            charPassword[first] = password[second];
            charPassword[second] = password[first];
            return new string(charPassword);
        }

        private static string SwapByLetter(string password, char first, char second)
        {
            var swapped = password
                .Select(r => r == first ? second : r == second ? first : r)
                .ToArray();
            return new string(swapped);
        }

        private static string RotateRight(string password, int steps)
        {
            var ll = new LinkedList<char>(password);
            for (var i = 0; i < steps; i++)
            {
                var last = ll.Last;
                ll.RemoveLast();
                ll.AddFirst(last!);
            }

            return new string(ll.ToArray());
        }

        private static string RotateLeft(string password, int steps)
        {
            var ll = new LinkedList<char>(password);
            for (var i = 0; i < steps; i++)
            {
                var first = ll.First;
                ll.RemoveFirst();
                ll.AddLast(first!);
            }

            return new string(ll.ToArray());
        }

        private string RotateByPosition(string password, char letter, bool reversed = false)
        {
            if (reversed)
            {
                for (var i = 1; i <= password.Length; i++)
                {
                    var tryVal = RotateLeft(password, i);
                    if (password == RotateByPosition(tryVal, letter))
                        return tryVal;
                }
            }

            var index = password.IndexOf(letter);
            var steps = index >= 4 ? index + 2 : index + 1;
            return RotateRight(password, steps);
        }

        private static string ReversePositions(string password, int first, int second)
        {
            var reversedArray = password
                .Substring(first, second - first + 1)
                .Reverse()
                .ToArray();

            var reversed = new string(reversedArray);
            var beginning = password.Substring(0, first);
            var ending = password.Substring(second + 1);
            return beginning + reversed + ending;
        }

        private static string Move(string password, int first, int second)
        {
            var updated = password.Remove(first, 1);
            return updated.Insert(second, password[first].ToString());
        }
    }
}