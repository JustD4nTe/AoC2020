using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day10
{
    public static class PartTwo
    {
        private static int[] adapters;
        private static int maxValue;

        public static ulong Solve()
        {
            using var sr = new StreamReader("Day10/data.txt");
            var arr = sr.ReadToEnd()
                        .Split("\n")
                        .Select(x => int.Parse(x))
                        .ToList();

            arr.Add(arr.Max() + 3);
            arr.Sort();

            adapters = arr.ToArray();
            maxValue = arr.Max();

            Func<int, int, ulong> backtracking = null;

            backtracking = (lastAdapter, i) =>
            {
                if (lastAdapter == maxValue)
                    return 1;

                ulong solutions = 0;

                for (var j = i + 1; j < i + 4; j++)
                {
                    if (j < 100 && lastAdapter + 3 >= adapters[j])
                        solutions += backtracking(adapters[j], j);
                    else
                        break;
                }

                return solutions;
            };

            backtracking = Memoize(backtracking);

            return backtracking(0, -1);
        }

        // https://trenki2.github.io/blog/2018/12/31/memoization-in-csharp/
        private static Func<A, I, R> Memoize<A, I, R>(Func<A, I, R> func)
        {
            var cache = new Dictionary<A, R>();

            return (a, i) =>
            {
                if (cache.TryGetValue(a, out R value))
                    return value;

                value = func(a, i);
                cache.Add(a, value);

                return value;
            };
        }
    }
}