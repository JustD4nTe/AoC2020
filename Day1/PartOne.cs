using System;
using System.IO;
using System.Linq;

namespace AoC2020.Day1
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day1/data.txt");

            var numbers = sr.ReadToEnd()
                      .Split("\r\n")
                      .Select(x => Convert.ToInt32(x));

            var foo = 0;

            foreach (var number in numbers)
            {
                foo = 2020 - number;

                if (numbers.Contains(foo))
                {
                    foo *= number;
                    break;
                }
            }

            return foo;
        }
    }
}