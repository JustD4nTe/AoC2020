using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020.Day23
{
    public static class PartOne
    {
        public static void Solve()
        {
            const string input = "963275481";
            // const string input = "389125467";

            var cups = input.Select(x => int.Parse(x.ToString())).ToList();

            var currentId = 0;

            for (var i = 0; i < 100; i++)
            {
                Console.WriteLine($"-- move {i + 1} --");
                Console.Write("cups: ");

                for (var j = 0; j < cups.Count; j++)
                {
                    if (j == currentId)
                        Console.Write($"({cups[j]}) ");
                    else
                        Console.Write($"{cups[j]} ");
                }
                Console.WriteLine();

                // The crab picks up the three cups that are immediately clockwise of the current cup. 
                // They are removed from the circle; cup spacing is adjusted as necessary to maintain the circle.

                // Step 1
                var removedCups = GetRemovedCups(cups, currentId);

                Console.WriteLine($"pick up: {string.Join(", ", removedCups)}");

                // The crab selects a destination cup: the cup with a label equal to the current cup's label minus one. 
                // If this would select one of the cups that was just picked up, the crab will keep subtracting one 
                // until it finds a cup that wasn't just picked up. If at any point in this process the value goes below 
                // the lowest value on any cup's label, it wraps around to the highest value on any cup's label instead.

                // Step 2
                var destCup = cups[currentId] - 1;

                if (destCup == 0)
                    destCup = 9;

                while (removedCups.Contains(destCup))
                {
                    destCup--;

                    if (destCup == 0)
                        destCup = 9;
                }

                var destId = cups.IndexOf(destCup);

                Console.WriteLine($"destination: {destCup}\n");

                // The crab places the cups it just picked up so that they are immediately clockwise of the destination cup. 
                // They keep the same order as when they were picked up.

                // Step 3

                var foo = new List<int>
                {
                    cups[destId]
                };

                foo.AddRange(removedCups);

                for (var j = 0; j < cups.Count; j++)
                {
                    var id = (j + destId) % cups.Count;

                    if (foo.Contains(cups[id]))
                        continue;

                    foo.Add(cups[id]);
                }

                // The crab selects a new current cup: the cup which is immediately clockwise of the current cup.

                // Step 4
                currentId = (foo.IndexOf(cups[currentId]) + 1) % cups.Count;

                cups = new(foo);

                // currentId = cups.IndexOf(newCurrCup);
            }

            Console.WriteLine("-- final --");

            var oneId = (cups.IndexOf(1) + 1) % cups.Count;

            for (var i = 0; i < cups.Count - 1; i++)
                Console.Write(cups[(oneId + i) % cups.Count]);
            Console.WriteLine();
        }

        private static List<int> GetRemovedCups(List<int> cups, int currId)
        {
            var res = new List<int>();

            for (var i = 1; i <= 3; i++)
                res.Add(cups[(currId + i) % cups.Count]);

            return res;
        }
    }
}