using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day8
{
    public static class PartTwo
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day8/data.txt");
            var instructions = sr.ReadToEnd()
                                 .Split("\n")
                                 .Select(x => x.Split(" "))
                                 .ToList();

            var isExecuted = new List<bool>(new bool[instructions.Count]);
            var nopAndJmp = new List<int>();

            for (var i = 0; i < instructions.Count; i++)
            {
                if ((instructions[i][0] == "nop"  && instructions[i][1] != "+0") || instructions[i][0] == "jmp")
                    nopAndJmp.Add(i);
            }

            foreach (var approach in nopAndJmp)
            {
                var newInstructions = new List<string[]>();

                foreach (var line in instructions)
                {
                    newInstructions.Add((string[])line.Clone());
                }

                if (newInstructions[approach][0] == "nop")
                    newInstructions[approach][0] = "jmp";
                else
                    newInstructions[approach][0] = "nop";

                if (newInstructions[approach][0] == instructions[approach][0])
                    break;

                try
                {
                    return Compute(newInstructions);
                }
                catch { }
            }

            return -1;
        }

        public static void Operation(ref int a, char operation, int number)
        {
            if (operation == '+')
                a += number;
            else if (operation == '-')
                a -= number;
        }

        public static int Compute(List<string[]> instructions)
        {
            var isExecuted = new List<int>(new int[instructions.Count]);
            var ptr = 0;
            var accumulator = 0;

            while (ptr < instructions.Count)
            {
                if (isExecuted[ptr] == 5)
                {
                    isExecuted = new List<int>(new int[instructions.Count]);
                    throw new Exception();
                }
                else
                {
                    isExecuted[ptr]++;
                }

                var command = instructions[ptr][0];
                var value = instructions[ptr][1];

                if (command == "nop")
                {
                    ptr++;
                    continue;
                }

                var operation = value[0];
                var number = int.Parse(value[1..]);

                if (command == "acc")
                {
                    Operation(ref accumulator, operation, number);
                    ptr++;
                }
                else if (command == "jmp")
                {
                    Operation(ref ptr, operation, number);
                }
            }

            return accumulator;
        }
    }
}