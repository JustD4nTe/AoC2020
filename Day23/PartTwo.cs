using System;
using System.Linq;

namespace AoC2020.Day23
{
    public static class PartTwo
    {
        public static ulong Solve()
        {
            const string input = "963275481";
            // const string input = "389125467";
            const int maxValue = 1_000_000;

            var arr = input.Select(x => int.Parse(x.ToString())).ToArray();

            var cups = new CupCircle(arr[0]);

            for (var i = 1; i < arr.Length; i++)
                cups.Insert(arr[i]);

            for (var i = 10; i <= 1_000_000; i++)
                cups.Insert(i);

            for (var i = 0; i < 10_000_000; i++)
            {
                var currCup = cups.Head;

                // Step 1
                var firstRemoved = currCup.Next;
                var lastRemoved = firstRemoved.Next.Next;

                currCup.Next = lastRemoved.Next;
                currCup.Next.Previous = currCup;

                // Step 2
                var destCupValue = currCup.Value - 1;

                if (destCupValue == 0)
                    destCupValue = maxValue;

                while (firstRemoved.Value == destCupValue
                    || firstRemoved.Next.Value == destCupValue
                    || lastRemoved.Value == destCupValue)
                {
                    destCupValue--;

                    if (destCupValue == 0)
                        destCupValue = maxValue;
                }

                var destCup = cups.Find(destCupValue);

                // Step 3
                var buff = destCup.Next;

                destCup.Next = firstRemoved;
                firstRemoved.Previous = destCup;

                lastRemoved.Next = buff;
                buff.Previous = lastRemoved;

                // Step 4
                cups.Head = cups.Head.Next;
                cups.Tail = cups.Head.Previous;
            }

            var foo = cups.Find(1).Next;

            Console.WriteLine(foo.Value);
            Console.WriteLine(foo.Next.Value);

            return (ulong)foo.Value * (ulong)foo.Next.Value;
        }
    }
}