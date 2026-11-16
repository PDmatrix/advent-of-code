using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2016._5;

[UsedImplicitly]
public class Year2016Day05 : ISolution
{
    public object Part1(IEnumerable<string> input)
    {
        var doorId = input.First();
        var password = "";
        var idx = 0;
        while (password.Length < 8)
        {
            var md5 = CreateMd5(string.Concat(doorId, idx));
            if (md5.StartsWith("00000"))
                password += md5[5];
            idx++;
        }
        return password;
    }

    public object Part2(IEnumerable<string> input)
    {
        var doorId = input.First();
        var password = new string[8];
        var idx = 0;
        while (password.Count(r => !string.IsNullOrWhiteSpace(r)) < 8)
        {
            var md5 = CreateMd5(string.Concat(doorId, idx));
            idx++;
            if (!md5.StartsWith("00000")) 
                continue;
                
            if(!int.TryParse(md5[5].ToString(), out var position))
                continue;
                    
            if (0 <= position && position <= 7 && string.IsNullOrWhiteSpace(password[position]))
                password[position] = md5[6].ToString();
        }
        return string.Join("", password);
    }
        
    private static string CreateMd5(string input)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                sb.Append(hashByte.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}