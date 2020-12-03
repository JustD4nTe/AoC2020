using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AoC2020.Day3
{
    public static class PartTwo
    {
        public static ulong Solve()
        {
            using var sr = new StreamReader("Day3/data.txt");

            var map = sr.ReadToEnd().Split("\r\n");

            ulong res = 1;
            var travelSteps = new Collection<(int, int)> {
                (1,1), (3,1), (5,1), (7,1), (1,2)
            };

            foreach (var item in travelSteps)
                res *= TreeCount(item.Item1, item.Item2, ref map);

            return res;
        }

        private static ulong TreeCount(int xStep, int yStep, ref string[] map)
        {
            ulong treeCount = 0;

            for (int y = 0, x = 0; y < map.Length; y += yStep, x += xStep)
            {
                // patterns repeats
                // so we need to "reset" x
                // to continue on left side
                x %= map[y].Length;

                if (map[y][x] == '#')
                    treeCount++;
            }

            return treeCount;
        }
    }
}