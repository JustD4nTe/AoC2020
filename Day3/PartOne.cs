using System.IO;

namespace AoC2020.Day3
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day3/data.txt");

            var map = sr.ReadToEnd().Split("\r\n");

            var treeCount = 0;

            for (int y = 0, x = 0; y < map.Length; y++, x += 3)
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