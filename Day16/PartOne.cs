using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day16
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day16/data.txt");
            var numberInRules = new List<int>();

            // read all rules
            for (var i = 0; i < 20; i++)
            {
                var rule = sr.ReadLine();
                // we are interest only in numbers after colon
                var colonId = rule.IndexOf(":") + 2;
                rule = rule[colonId..];

                // parse ranges
                var foo = rule.Split(" or ")
                              .Select(x => x.Split("-")
                                            .Select(x => int.Parse(x))
                                            .ToArray())
                               .ToList();

                numberInRules.AddRange(GetRanges(foo));
            }

            // avoid next 5 lines
            for (var i = 0; i < 5; i++)
                sr.ReadLine();

            // get all tickets
            var tickets = sr.ReadToEnd()
                            .Split("\n")
                            .Select(x => x.Split(","))
                            .SelectMany(x => x)
                            .Select(x => int.Parse(x))
                            .ToArray();

            var invalidNumbers = new List<int>();

            // save invalid tickets
            for (var i = 0; i < tickets.Length; i++)
            {
                if (!numberInRules.Contains(tickets[i]))
                    invalidNumbers.Add(tickets[i]);
            }

            return invalidNumbers.Sum();
        }

        // return list with numbers by given ranges
        private static List<int> GetRanges(List<int[]> numbers)
        {
            var foo = new List<int>();

            for (var i = 0; i < numbers.Count; i++)
            {
                for (var j = numbers[i][0]; j <= numbers[i][1]; j++)
                    foo.Add(j);
            }

            return foo;
        }
    }
}