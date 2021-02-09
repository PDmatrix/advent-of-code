using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2015._10
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day10 : ISolution
	{
        private static string GetInput(IEnumerable<string> lines)
        {
            var input = lines.FirstOrDefault();
            if (input == null)
                throw new Exception("Invalid input");

            return input;
        }
		
		public object Part1(IEnumerable<string> lines)
        {
            var input = GetInput(lines);
            for (var i = 0; i < 40; i++)
            {
                input = ProcessInput(input);
            }
            return input.Length.ToString();
        }

        private static string ProcessInput(string input)
        {
            var counter = -1;
            var processing = '\0';
            var sb = new StringBuilder();
            foreach (var num in input)
            {
                if (counter == -1)
                {
                    processing = num;
                    counter = 1;
                    continue;
                }

                if (num == processing)
                    counter++;
                else
                {
                    sb.Append(counter.ToString());
                    sb.Append(processing);
                    counter = 1;
                    processing = num;
                }
            }
            sb.Append(counter.ToString());
            sb.Append(processing);
            return sb.ToString();
        }

        public object Part2(IEnumerable<string> lines)
		{
            var input = GetInput(lines);
            for (var i = 0; i < 50; i++)
            {
                input = ProcessInput(input);
            }
            return input.Length.ToString();
        }
	}
}