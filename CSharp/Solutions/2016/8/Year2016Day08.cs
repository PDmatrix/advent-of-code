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
        private const int ScreenHeight = 6;
        private const int ScreenWidth = 50;
        
        public object Part1(IEnumerable<string> input)
        {
            var screen = new bool[ScreenHeight, ScreenWidth];
            foreach (var instruction in input)
            {
                var groups = 
                    Regex.Match(instruction, @"\D*(\d+)\D*(\d+)").Groups;
                var a = int.Parse(groups[1].Value);
                var b = int.Parse(groups[2].Value);
                if (instruction.Contains("rect"))
                    Rect(a, b, screen);

                if (instruction.Contains("column"))
                    RotateColumn(a, b, screen);
                
                if (instruction.Contains("row"))
                    RotateRow(a, b, screen);
            }
            var result = 0;
            for (var x = 0; x < ScreenHeight; x++)
            {
                for (var y = 0; y < ScreenWidth; y++)
                {
                    result += screen[x, y] ? 1 : 0;
                }
            }
            return result.ToString();
        }

        public object Part2(IEnumerable<string> input)
        {
            var screen = new bool[ScreenHeight, ScreenWidth];
            foreach (var instruction in input)
            {
                var groups = 
                    Regex.Match(instruction, @"\D*(\d+)\D*(\d+)").Groups;
                var a = int.Parse(groups[1].Value);
                var b = int.Parse(groups[2].Value);
                if (instruction.Contains("rect"))
                    Rect(a, b, screen);

                if (instruction.Contains("column"))
                    RotateColumn(a, b, screen);
                
                if (instruction.Contains("row"))
                    RotateRow(a, b, screen);
            }

            var sb = new StringBuilder(Environment.NewLine);
            for (var x = 0; x < ScreenHeight; x++)
            {
                for (var y = 0; y < ScreenWidth; y++)
                {
                    sb.Append(screen[x, y] ? "#" : " ");
                }

                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static void Rect(int a, int b, bool[,] screen)
        {
            for (var i = 0; i < b; i++)
            {
                for (var j = 0; j < a; j++)
                {
                    screen[i, j] = true;
                }
            }
        }
        
        private static void RotateRow(int a, int b, bool[,] screen)
        {
            var coords = new List<int>();
            for (var i = 0; i < ScreenWidth; i++)
            {
                if(screen[a, i])
                    coords.Add(i);
                screen[a, i] = false;
            }

            coords = coords.Select(r => (r + b) % ScreenWidth).ToList();
            foreach (var coord in coords)
            {
                screen[a, coord] = true;
            }
        }
        
        private static void RotateColumn(int a, int b, bool[,] screen)
        {
            var coords = new List<int>();
            for (var i = 0; i < ScreenHeight; i++)
            {
                if(screen[i, a])
                    coords.Add(i);
                screen[i, a] = false;
            }

            coords = coords.Select(r => (r + b) % ScreenHeight).ToList();
            foreach (var coord in coords)
            {
                screen[coord, a] = true;
            }
        }
    }
}