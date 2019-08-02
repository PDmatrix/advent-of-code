using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._20
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day20 : ISolution
    {
        public string Part1(IEnumerable<string> input)
        {
            var blacklist = input.Select(r =>
                {
                    var splitted = r.Split("-").Select(long.Parse).ToArray();
                    return (splitted[0], splitted[1]);
                })
                .ToList();
            
            blacklist.Sort((a, b) =>
            {
                var (minA, _) = a;
                var (minB, _) = b;
                
                if (minA > minB)
                    return 1;
                if (minA < minB)
                    return -1;

                return 0;
            });

            long current = 0;
            foreach (var (min, max) in blacklist)
            {
                if (current >= min)
                    current = max + 1;
            }

            return current.ToString();
        }

        public string Part2(IEnumerable<string> input)
        {
            var blacklist = input.Select(r =>
                {
                    var splitted = r.Split("-").Select(long.Parse).ToArray();
                    return (splitted[0], splitted[1]);
                })
                .ToList();
            
            blacklist.Sort((a, b) =>
            {
                var (minA, _) = a;
                var (minB, _) = b;
                
                if (minA > minB)
                    return 1;
                if (minA < minB)
                    return -1;

                return 0;
            });

            long lastMax = 0;
            long total = 0;
            foreach (var (min, max) in blacklist)
            {
                if (lastMax >= max) 
                    continue;

                total += max - Math.Max(min - 1, lastMax);
                lastMax = max;
            }

            return (4294967295 - total).ToString();
        }
    }
}