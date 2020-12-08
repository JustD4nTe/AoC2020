using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day8
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day8/data.txt");
            var instructions = sr.ReadToEnd()
                                 .Split("\n")
                                 .Select(x => x.Split(" "))
                                 .ToList();

            var accumulator = 0;
            var isExecuted = new List<bool>(new bool[instructions.Count]);
            var ptr = 0;

            while (ptr < instructions.Count)
            {
                if (isExecuted[ptr])
                    break;
                else
                    isExecuted[ptr] = true;

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

        public static void Operation(ref int a, char operation, int number)
        {
            if (operation == '+')
                a += number;
            else if (operation == '-')
                a -= number;
        }
    }
}