using System.IO;
using System.Linq;

namespace AoC2020.Day9
{
    public static class PartOne
    {
        public static ulong Solve()
        {
            using var sr = new StreamReader("Day9/data.txt");
            var numbers = sr.ReadToEnd().Split("\n").Select(x => ulong.Parse(x)).ToList();

            const int preamble = 25;

            for (var i = preamble; i < numbers.Count; i++)
            {
                var currNumber = numbers[i];
                var isHaveNotPair = true;

                for (var j = 0; j < preamble; j++)
                {
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
                    return numbers[i];
            }

            return 0;
        }
    }
}