using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Solutions._2016._4
{
    // ReSharper disable once UnusedMember.Global
    public class Year2016Day04 : ISolution
    {
        public string Part1(IEnumerable<string> input)
        {
            var sectorIds = GetSectorIds(input);
            return sectorIds.Sum().ToString();
        }

        public string Part2(IEnumerable<string> input)
        {
            var list = GetNameAndSectorIds(input);
            return list.First(r => Encipher(r.Item1.Trim(), r.Item2) == "northpole object storage").Item2.ToString();
        }

        private static IEnumerable<int> GetSectorIds(IEnumerable<string> input)
        {
            foreach (var room in input)
            {
                var encryptedName = Regex.Match(room, @".*-").Value.Replace("-", string.Empty);
                var checksum = room.Split("[")[1].Trim(']');
                var computedChecksum = string.Join(string.Empty,
                    encryptedName.GroupBy(x => x)
                    .OrderByDescending(x => x.Count())
                    .ThenBy(x => x.Key)
                    .Take(5)
                    .Select(r => r.Key)
                    .ToArray());
                if (checksum == computedChecksum)
                    yield return int.Parse(Regex.Match(room, @"\d+").Value);
            }
        }

        private static IEnumerable<(string, int)> GetNameAndSectorIds(IEnumerable<string> input)
        {
            foreach (var room in input)
            {
                var encryptedName = Regex.Match(room, @".*-").Value;
                var replacedEncryptedName = encryptedName.Replace("-", string.Empty);
                var checksum = room.Split("[")[1].Trim(']');
                var computedChecksum = string.Join(string.Empty,
                    replacedEncryptedName.GroupBy(x => x)
                        .OrderByDescending(x => x.Count())
                        .ThenBy(x => x.Key)
                        .Take(5)
                        .Select(r => r.Key)
                        .ToArray());
                if (checksum == computedChecksum)
                    yield return (encryptedName.Replace("-", " "), int.Parse(Regex.Match(room, @"\d+").Value));
            }
        }

        private static char Cipher(char ch, int key)
        {
            if (!char.IsLetter(ch))
            {
                return ch;
            }

            return (char)((ch + key - 'a') % 26 + 'a');
        }

        private static string Encipher(string input, int key)
        {
            var sb = new StringBuilder();
            foreach (var ch in input)
                sb.Append(Cipher(ch, key));

            return sb.ToString();
        }
    }
}