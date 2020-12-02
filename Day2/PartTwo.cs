using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020.Day2
{
    public static class PartTwo
    {
        public static int Solve()
        {
            using var sr = new System.IO.StreamReader("Day2/data.txt");

            var res = 0;

            foreach (var passPolicy in sr.ReadToEnd().Split("\r\n"))
            {
                var foo = passPolicy.Split(" ");

                var pos = foo[0].Split("-")
                                .Select(x => Convert.ToInt32(x) - 1)
                                .ToList();

                // only first character
                var character = foo[1][0];

                var pass = foo[2];

                var first = pass[pos[0]];
                var second = pass[pos[1]];

                if (first != second && (first == character || second == character))
                    res++;
            }

            return res;
        }
    }
}