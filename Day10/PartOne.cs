using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day10
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day10/data.txt");
            var adapters = sr.ReadToEnd().Split("\n").Select(x => int.Parse(x)).ToList();
            adapters.Sort();

            var diffJoints = new Dictionary<int, int>
            {
                [1] = 1,
                [3] = 1
            };

            for (var i = 1; i < adapters.Count; i++)
            {
                diffJoints[adapters[i] - adapters[i - 1]]++;
            }

            return diffJoints[1] * diffJoints[3];
        }
    }
}