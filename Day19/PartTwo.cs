using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020.Day19
{
    public static class PartTwo
    {
        private static Dictionary<string, IEnumerable<string>> rules;
        public static int Solve()
        {
            using var sr = new StreamReader("Day19/data2.txt");
            rules = new Dictionary<string, IEnumerable<string>>();

            for (var i = 0; i < 132; i++)
            {
                var temp = sr.ReadLine().Split(":");
                rules[temp[0]] = temp[1].Trim()
                                        .Split(" ")
                                        .Select(x => x.Trim());
            }

            sr.ReadLine();

            var messages = sr.ReadToEnd().Split("\n");

            var regex = new Regex($"^{Resolve()}$");

            return messages.Count(x => regex.IsMatch(x));
        }

        private static string ResolveToken(string token)
        {
            if (token[0] == '\"')
                return token[1].ToString();
            if (token == "|")
                return "|";
            return $"({Resolve(token)})";
        }

        private static string Resolve(string id = "0")
        {
            if (id == "8")
                return $"({ResolveToken("42")})+";
            if (id == "11")
            {
                return string.Join("|", Enumerable.Range(1, 10)
                                                  .Select(x => string.Concat(Enumerable.Repeat(ResolveToken("42"), x))
                                                             + string.Concat(Enumerable.Repeat(ResolveToken("31"), x))));
            }

            return string.Concat(rules[id].Select(x => ResolveToken(x)));
        }
    }
}