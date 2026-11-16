using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2020._4;

[UsedImplicitly]
public class Year2020Day04 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var passports = ParsePassports(input);

		var requiredFields = new Dictionary<string, Func<string, bool>>
		{
			["byr"] = _ => true,
			["iyr"] = _ => true,
			["eyr"] = _ => true,
			["hgt"] = _ => true,
			["hcl"] = _ => true,
			["ecl"] = _ => true,
			["pid"] = _ => true
		};

		return GetValidPassports(passports, requiredFields);

	}

	private static List<Dictionary<string, string>> ParsePassports(IEnumerable<string> input)
	{
		var passports = new List<Dictionary<string, string>>();
		var tempDict = new Dictionary<string, string>();
		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				passports.Add(tempDict);
				tempDict = new Dictionary<string, string>();
				continue;
			}

			foreach (var kv in line.Split(' '))
			{
				var split = kv.Split(':');
				tempDict.Add(split[0], split[1]);
			}
		}

		passports.Add(tempDict);
		return passports;
	}

	public object Part2(IEnumerable<string> input)
	{
		var passports = ParsePassports(input);

		var requiredFields = new Dictionary<string, Func<string, bool>>
		{
			["byr"] = val => Regex.IsMatch(val, @"^\d{4}$") && int.Parse(val) >= 1920 && int.Parse(val) <= 2002,
			["iyr"] = val => Regex.IsMatch(val, @"^\d{4}$") && int.Parse(val) >= 2010 && int.Parse(val) <= 2020,
			["eyr"] = val => Regex.IsMatch(val, @"^\d{4}$") && int.Parse(val) >= 2020 && int.Parse(val) <= 2030,
			["hgt"] = val =>
			{
				var regex = new Regex(@"^(?<num>\d+)(?<dim>cm|in)$");
				var match = regex.Match(val);
				if (!match.Success)
					return false;
				var num = int.Parse(match.Groups["num"].Value);
				if (match.Groups["dim"].Value == "cm" && num is >= 150 and <= 193)
					return true;

				if (match.Groups["dim"].Value == "in" && num is >= 59 and <= 76)
					return true;

				return false;
			},
			["hcl"] = val => Regex.IsMatch(val, @"^#[0-9|a-f]{6}$"),
			["ecl"] = val => Regex.IsMatch(val, @"^(amb|blu|brn|gry|grn|hzl|oth)$"),
			["pid"] = val => Regex.IsMatch(val, @"^\d{9}$")
		};

		return GetValidPassports(passports, requiredFields);
	}

	private static int GetValidPassports(List<Dictionary<string, string>> passports,
		Dictionary<string, Func<string, bool>> requiredFields)
	{
		var approvedPasswordsCount = 0;
		foreach (var passport in passports)
		{
			var isValid = requiredFields.All(requiredField =>
				passport.ContainsKey(requiredField.Key) && requiredField.Value(passport[requiredField.Key]));

			if (isValid)
				approvedPasswordsCount++;
		}

		return approvedPasswordsCount;
	}
}