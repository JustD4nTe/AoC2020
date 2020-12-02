using System;
using System.IO;
using System.Linq;

namespace AoC2020.Day2
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day2/data.txt");

            var res = 0;

            foreach (var passwordPolicy in sr.ReadToEnd().Split("\r\n"))
            {
                var foo = passwordPolicy.Split(" ");

                var limit = foo[0].Split("-")
                                .Select(x => Convert.ToInt32(x))
                                .ToList();

                // only first character
                var character = foo[1][0];

                var pass = foo[2];

                var charCount = pass.Count(x => x == character);

                if (charCount >= limit[0] && charCount <= limit[1])
                    res++;
            }

            return res;
        }
    }
}