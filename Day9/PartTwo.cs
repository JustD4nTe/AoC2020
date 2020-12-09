using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day9
{
    public static class PartTwo
    {
        public static ulong Solve()
        {
            using var sr = new StreamReader("Day9/data.txt");
            var numbers = sr.ReadToEnd()
                            .Split("\n")
                            .Select(x => ulong.Parse(x))
                            .ToList();

            const int preamble = 25;
            var i = preamble;

            // iterate for every number in list
            for (; i < numbers.Count; i++)
            {
                var currNumber = numbers[i];
                var isHaveNotPair = true;

                // then iterate for every number before current number
                for (var j = 0; j < preamble; j++)
                {
                    // get all numbers before current number
                    var previousNumbers = numbers.GetRange(i - preamble, preamble);
                    var foo = previousNumbers[j];
                    var index = previousNumbers.IndexOf(currNumber - foo);

                    if (index != -1 && index != j)
                    {
                        isHaveNotPair = false;
                        break;
                    }
                }

                if (isHaveNotPair)
                    break;
            }

            // change starting index to contiguous summing
            for (var j = 0; j < i; j++)
            {
                var temp = new List<ulong>();

                for (var k = j; k < i; k++)
                {
                    temp.Add(numbers[k]);

                    // func Sum() there is ambigous because of 2 using:
                    // Linq and Collections.Generic
                    var a = temp.Aggregate((s, x) => s + x);

                    if (a > numbers[i])
                        break;
                    else if (a == numbers[i])
                        return temp.Min() + temp.Max();
                }
            }

            return 0;
        }
    }
}