using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day19
{
    public static class PartOne
    {
        private static Dictionary<int, string> rules;
        public static int Solve()
        {
            using var sr = new StreamReader("Day19/data.txt");
            rules = new Dictionary<int, string>();

            for (var i = 0; i < 132; i++)
            {
                var temp = sr.ReadLine().Split(":");
                rules[int.Parse(temp[0])] = temp[1].Trim();
            }

            var messages = sr.ReadToEnd().Split("\n");

            var validMsg = GenerateRules();

            return messages.Count(x => validMsg.Contains(x));
        }

        private static List<string> GenerateRules(int id = 0)
        {
            // return single character
            if (rules[id].Contains("\""))
                return new List<string>() { rules[id][1].ToString() };

            int[][] subRules;

            // when rule contains more than one rule 
            // (split them)
            if (rules[id].Contains("|"))
            {
                subRules = rules[id].Split("|")
                                   .Select(x => x.Trim()
                                                 .Split(" "))
                                   .Select(x => x.Select(x => int.Parse(x))
                                                 .ToArray())
                                   .ToArray();
            }
            // otherwise add single rule into two dimensional array 
            else
            {
                subRules = new int[1][]
                {
                    rules[id].Split(" ")
                             .Select(x => int.Parse(x))
                             .ToArray()
                };
            }

            // store sub-roles
            var matches = new List<string>();

            for (var i = 0; i < subRules.Length; i++)
            {
                IEnumerable<string> foo = new List<string>() { null };

                // 1. get all sub roles of sub-role
                // 2. connect each sub-role with another
                // basically, permutate every sub-sub-role to get all variations
                foreach (var item in subRules[i].Select(x => GenerateRules(x)))
                    foo = foo.SelectMany(o => item.Select(x => o + x));

                matches.AddRange(foo);
            }

            return matches;
        }
    }
}