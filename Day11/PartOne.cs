using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day11
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day11/test.txt");
            var seatLayout = sr.ReadToEnd()
                               .Split("\n")
                               .ToList()
                               .ConvertAll(x => x.ToCharArray());

            var newLayout = new List<char[]>();

            for (var i = 0; i < seatLayout.Count; i++)
                newLayout.Add((char[])seatLayout[i].Clone());

            do
            {
                seatLayout.Clear();

                // save new Layout
                for (var i = 0; i < newLayout.Count; i++)
                    seatLayout.Add((char[])newLayout[i].Clone());

                for (var row = 0; row < seatLayout.Count; row++)
                {
                    for (var col = 0; col < seatLayout[row].Length; col++)
                    {
                        if (seatLayout[row][col] == 'L')
                        {
                            // take seat only when no one is around
                            if (GetNumberOfOccupiedSeatsAround(row, col, seatLayout) == 0)
                                newLayout[row][col] = '#';
                        }
                        else if (seatLayout[row][col] == '#')
                        {
                            // leave seat only when 4 or more seats are occupied
                            if (GetNumberOfOccupiedSeatsAround(row, col, seatLayout) >= 4)
                                newLayout[row][col] = 'L';
                        }
                    }
                }

                // PrintLayout(newLayout);
            } while (!IsLayoutsEqual(seatLayout, newLayout));

            return seatLayout.SelectMany(x => x).Count(x => x == '#');
        }

        // # # #
        // # & #
        // # # # => gives 8, and '&' represents current seat/position
        private static uint GetNumberOfOccupiedSeatsAround(int row, int col, List<char[]> seatLayout)
        {
            uint occupiedSeats = 0;
            for (var y = -1; y <= 1; y++)
            {
                for (var x = -1; x <= 1; x++)
                {
                    if (x == 0 && y == 0) continue;
                    if (y + row < 0 || y + row >= seatLayout.Count) continue;
                    if (x + col < 0 || x + col >= seatLayout[y + row].Length) continue;

                    if (seatLayout[y + row][x + col] == '#')
                        occupiedSeats++;
                }
            }

            return occupiedSeats;
        }

        // Compare every row from old layout with parallel curr's rows 
        private static bool IsLayoutsEqual(List<char[]> old, List<char[]> curr)
        {
            for (var rowId = 0; rowId < old.Count; rowId++)
            {
                if (!old[rowId].SequenceEqual(curr[rowId]))
                    return false;
            }

            return true;
        }

        private static void PrintLayout(List<char[]> seatLayout)
        {
            for (var i = 0; i < seatLayout.Count; i++)
                System.Console.WriteLine(seatLayout[i]);

            System.Console.WriteLine();
        }
    }
}