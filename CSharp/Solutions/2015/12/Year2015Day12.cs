using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using Newtonsoft.Json.Linq;

namespace AdventOfCode.Solutions._2015._12
{
	// ReSharper disable once UnusedMember.Global
	public class Year2015Day12 : ISolution
	{
        private static string GetInput(IEnumerable<string> lines)
        {
            var input = lines.FirstOrDefault();
            if (input == null)
                throw new Exception("Invalid input");

            return input;
        }

        public string Part1(IEnumerable<string> lines)
        {
            var input = GetInput(lines);
            var matches = Regex.Matches(input, @"-?\d+");
            var sum = 0;
            foreach (Match? match in matches)
            {
                if (int.TryParse(match?.Value, out var result))
                    sum += result;
                else
                    throw new Exception("Bad string");
            }
            return sum.ToString();
        }

        public string Part2(IEnumerable<string> lines)
		{
            var input = GetInput(lines);
            return ProcessArray(JArray.Parse(input)).ToString();
        }

        private static int ProcessArray(JArray array)
        {
	        return array.Children().Sum(ProcessToken);
        }

        private static int ProcessValue(JValue value)
        {
	        return int.TryParse(value.Value.ToString(), out var elem) ? elem : 0;
        }

        private static int ProcessObject(JObject jObject)
        {
	        foreach (var jProperty in jObject.Properties())
	        {
		        if (!(jProperty.Value is JValue jValue)) 
			        continue;
		        
		        if (jValue.Value.ToString() == "red")
			        return 0;
	        }

	        return jObject.Properties().Sum(jProperty => ProcessToken(jProperty.Value));
        }

        private static int ProcessToken(JToken token)
        {
	        switch (token)
	        {
		        case JArray array:
			        return ProcessArray(array);
		        case JObject jObject:
			        return ProcessObject(jObject);
		        case JValue value:
			        return ProcessValue(value);
	        }

	        return 0;
        }
	}
}