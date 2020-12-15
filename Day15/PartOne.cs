using System.IO;
using System.Linq;

namespace AoC2020.Day15
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day15/data.txt");
            var spokenNumbers = sr.ReadToEnd()
                                  .Split(",")
                                  .Select(x => int.Parse(x))
                                  .ToList();

            while (spokenNumbers.Count < 2020)
            {
                var lastNumber = spokenNumbers[^1];
                var withoutLastTurn = spokenNumbers.Take(spokenNumbers.Count - 1).ToList();
                var isSpokenBefore = withoutLastTurn.Any(x => x == lastNumber);

                if (isSpokenBefore)
                {
                    var lastIndex = withoutLastTurn.LastIndexOf(lastNumber) + 1;
                    var currNumber = spokenNumbers.Count - lastIndex;
                    spokenNumbers.Add(currNumber);
                }
                else
                {
                    spokenNumbers.Add(0);
                }
            }

            return spokenNumbers[^1];
        }
    }
}