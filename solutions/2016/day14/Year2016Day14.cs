using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2016._14;

[UsedImplicitly]
public class Year2016Day14 : ISolution
{
    private static Dictionary<string, string> _hashCodes = new Dictionary<string, string>();
    public object Part1(IEnumerable<string> input)
    {
        var keys = new List<int>();
        var salt = input.First();
        var idx = 1;
        while (keys.Count < 64)
        {
            var hashKey = $"{salt}{idx}";
            var hash = GetHash(hashKey);
            var match = Regex.Match(hash, @"(.+?)(\1){2}");
            if (match.Success)
            {
                if(CheckNext(idx, salt, match.Value.First()))
                    keys.Add(idx);
            }

            idx++;
        }
        return keys.Last().ToString();
    }
        
    public object Part2(IEnumerable<string> input)
    {
        _hashCodes = new Dictionary<string, string>();
        var keys = new List<int>();
        var salt = input.First();
        var idx = 0;
        while (keys.Count < 64)
        {
            var hashKey = $"{salt}{idx}";
            var hash = GetStretchedHash(hashKey);
            var match = Regex.Match(hash, @"(\w)\1{2}");
            if (match.Success)
            {
                if (CheckNextStretched(idx, salt, match.Value.First()))
                    keys.Add(idx);
            }

            idx++;
        }
        return keys.Last().ToString();
    }

    private static bool CheckNext(int idx, string salt, char value)
    {
        for (var i = 1; i <= 1000; i++)
        {
            var hashKey = $"{salt}{i + idx}";
            var hash = GetHash(hashKey);
            if (hash.Contains(new string(value, 5)))
                return true;
        }

        return false;
    }
        
    private static bool CheckNextStretched(int idx, string salt, char value)
    {
        for (var i = 1; i <= 1000; i++)
        {
            var hashKey = $"{salt}{i + idx}";
            var hash = GetStretchedHash(hashKey);
            if (hash.Contains(new string(value, 5)))
                return true;
        }

        return false;
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

            return sb.ToString().ToLower();
        }
    }

    private static string GetHash(string hashKey)
    {
        if (_hashCodes.ContainsKey(hashKey))
            return _hashCodes[hashKey];
            
        var hash = CreateMd5(hashKey);
        _hashCodes[hashKey] = hash;
        return hash;
    }
        
    private static string GetStretchedHash(string hashKey)
    {
        if (_hashCodes.ContainsKey(hashKey))
            return _hashCodes[hashKey];
            
        var hash = CreateMd5(hashKey);
        for (var i = 0; i < 2016; i++)
        {
            hash = CreateMd5(hash);
        }
        _hashCodes[hashKey] = hash;
        return hash;
    }
}