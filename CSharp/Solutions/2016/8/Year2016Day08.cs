using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._8
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day08 : ISolution
    {
        public string Part1(IEnumerable<string> input)
        {
            const int screenHeight = 6;
            const int screenWidth = 50;
            var screen = new bool[screenHeight, screenWidth];
            foreach (var instruction in input)
            {
                var groups = 
                    Regex.Match(instruction, @"\D*(\d+)\D*(\d+)").Groups;
                var a = int.Parse(groups[1].Value);
                var b = int.Parse(groups[2].Value);
                if (instruction.Contains("rect"))
                {
                    for (var i = 0; i < b; i++)
                    {
                        for (var j = 0; j < a; j++)
                        {
                            screen[i, j] = true;
                        }
                    }
                }

                if (instruction.Contains("column"))
                {
                    var coords = new List<int>();
                    for (var i = 0; i < screenHeight; i++)
                    {
                        if(screen[i, a])
                            coords.Add(i);
                        screen[i, a] = false;
                    }

                    coords = coords.Select(r => (r + b) % screenHeight).ToList();
                    foreach (var coord in coords)
                    {
                        screen[coord, a] = true;
                    }
                }
                
                if (instruction.Contains("row"))
                {
                    var coords = new List<int>();
                    for (var i = 0; i < screenWidth; i++)
                    {
                        if(screen[a, i])
                            coords.Add(i);
                        screen[a, i] = false;
                    }

                    coords = coords.Select(r => (r + b) % screenWidth).ToList();
                    foreach (var coord in coords)
                    {
                        screen[a, coord] = true;
                    }
                }
            }
            var result = 0;
            for (var x = 0; x < screenHeight; x++)
            {
                for (var y = 0; y < screenWidth; y++)
                {
                    result += screen[x, y] ? 1 : 0;
                }
            }
            return result.ToString();
        }

        public string Part2(IEnumerable<string> input)
        {
            const int screenHeight = 6;
            const int screenWidth = 50;
            var screen = new bool[screenHeight, screenWidth];
            foreach (var instruction in input)
            {
                var groups = 
                    Regex.Match(instruction, @"\D*(\d+)\D*(\d+)").Groups;
                var a = int.Parse(groups[1].Value);
                var b = int.Parse(groups[2].Value);
                if (instruction.Contains("rect"))
                {
                    for (var i = 0; i < b; i++)
                    {
                        for (var j = 0; j < a; j++)
                        {
                            screen[i, j] = true;
                        }
                    }
                }

                if (instruction.Contains("column"))
                {
                    var coords = new List<int>();
                    for (var i = 0; i < screenHeight; i++)
                    {
                        if(screen[i, a])
                            coords.Add(i);
                        screen[i, a] = false;
                    }

                    coords = coords.Select(r => (r + b) % screenHeight).ToList();
                    foreach (var coord in coords)
                    {
                        screen[coord, a] = true;
                    }
                }
                
                if (instruction.Contains("row"))
                {
                    var coords = new List<int>();
                    for (var i = 0; i < screenWidth; i++)
                    {
                        if(screen[a, i])
                            coords.Add(i);
                        screen[a, i] = false;
                    }

                    coords = coords.Select(r => (r + b) % screenWidth).ToList();
                    foreach (var coord in coords)
                    {
                        screen[a, coord] = true;
                    }
                }
            }

            var sb = new StringBuilder(Environment.NewLine);
            for (var x = 0; x < screenHeight; x++)
            {
                for (var y = 0; y < screenWidth; y++)
                {
                    sb.Append(screen[x, y] ? "#" : " ");
                }

                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}