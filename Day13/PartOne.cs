using System.IO;
using System.Linq;

namespace AoC2020.Day13
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day13/data.txt");

            var timestamp = int.Parse(sr.ReadLine());
            var ids = sr.ReadLine()
                        .Split(",")
                        .Where(x => x != "x")
                        .Select(x => int.Parse(x));

            var earliestTime = int.MaxValue;
            var earliestBus = 0;

            foreach (var bus in ids)
            {
                // how many bus should arrive to this timestamp?
                var numberOfArrives = timestamp / bus;
                // bcs we can't get float number (bus can't arrive 7.5 times)
                // we have to increase int number to get next earliest after timestamp value
                numberOfArrives++;
                var busTime = numberOfArrives * bus;

                if (busTime < earliestTime)
                {
                    earliestTime = busTime;
                    earliestBus = bus;
                }
            }

            return (earliestTime - timestamp) * earliestBus;
        }
    }
}