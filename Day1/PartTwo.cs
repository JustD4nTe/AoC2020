using System;
using System.IO;
using System.Linq;

namespace AoC2020.Day1
{
    public static class PartTwo
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day1/data.txt");

            var numbers = sr.ReadToEnd()
                      .Split("\r\n")
                      .Select(x => Convert.ToInt32(x));

            var foo = 0;

            foreach (var firstNumber in numbers)
            {
                foreach (var secondNumber in numbers.Where(x => x != firstNumber))
                {

                    foo = 2020 - firstNumber - secondNumber;

                    if (numbers.Contains(foo))
                    {
                        Console.WriteLine(firstNumber);
                        Console.WriteLine(secondNumber);
                        Console.WriteLine(foo);
                        return firstNumber * secondNumber * foo;
                    }
                }
            }

            return -1;
        }
    }
}