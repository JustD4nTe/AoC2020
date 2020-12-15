using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day15
{
    public static class PartTwo
    {
        public static ulong Solve()
        {
            using var sr = new StreamReader("Day15/data.txt");
            var temp = sr.ReadToEnd()
                                  .Split(",")
                                  .Select(x => ulong.Parse(x))
                                  .ToList();

            // read last number
            var lastNumber = temp[^1];
            temp.RemoveAt(temp.Count - 1);

            // it'll store information about numbers, when they were spoken
            var spokenNumbers = new Dictionary<ulong, uint>();

            for (int i = 0; i < temp.Count; i++)
                spokenNumbers[temp[i]] = (uint)i + 1;

            for (uint turn = (uint)temp.Count + 1; turn < 30_000_000; turn++)
            {
                // when number is in dictionary
                if (spokenNumbers.TryGetValue(lastNumber, out uint lastTurn))
                {
                    // get difference between turns
                    var currNumber = turn - lastTurn;
                    spokenNumbers[lastNumber] = turn;
                    lastNumber = currNumber;
                }
                else
                {
                    spokenNumbers[lastNumber] = turn;
                    lastNumber = 0;
                }
            }

            return lastNumber;
        }
    }
}