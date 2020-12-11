using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day11
{
    public static class PartTwo
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day11/data.txt");
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
                            if (GetNumberOfOccupiedSeatsAround(row, col, seatLayout) >= 5)
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

            // Up
            for (var y = row - 1; y >= 0; y--)
            {
                if (seatLayout[y][col] == '#')
                {
                    occupiedSeats++;
                    break;
                }

                if (seatLayout[y][col] == 'L')
                    break;
            }

            // Down
            for (var y = row + 1; y < seatLayout.Count; y++)
            {
                if (seatLayout[y][col] == '#')
                {
                    occupiedSeats++;
                    break;
                }

                if (seatLayout[y][col] == 'L')
                    break;
            }

            // Left
            for (var x = col - 1; x >= 0; x--)
            {
                if (seatLayout[row][x] == '#')
                {
                    occupiedSeats++;
                    break;
                }

                if (seatLayout[row][x] == 'L')
                    break;
            }

            // Right
            for (var x = col + 1; x < seatLayout[row].Length; x++)
            {
                if (seatLayout[row][x] == '#')
                {
                    occupiedSeats++;
                    break;
                }

                if (seatLayout[row][x] == 'L')
                    break;
            }

            // Up Left
            for (int y = row - 1, x = col - 1; y >= 0 && x >= 0; y--, x--)
            {
                if (seatLayout[y][x] == '#')
                {
                    occupiedSeats++;
                    break;
                }

                if (seatLayout[y][x] == 'L')
                    break;
            }

            // Up Right
            for (int y = row - 1, x = col + 1; y >= 0 && x < seatLayout[0].Length; y--, x++)
            {
                if (seatLayout[y][x] == '#')
                {
                    occupiedSeats++;
                    break;
                }

                if (seatLayout[y][x] == 'L')
                    break;
            }

            // Down Right
            for (int y = row + 1, x = col + 1; y < seatLayout.Count && x < seatLayout[0].Length; y++, x++)
            {
                if (seatLayout[y][x] == '#')
                {
                    occupiedSeats++;
                    break;
                }

                if (seatLayout[y][x] == 'L')
                    break;
            }

            // Down Left
            for (int y = row + 1, x = col - 1; y < seatLayout.Count && x >= 0; y++, x--)
            {
                if (seatLayout[y][x] == '#')
                {
                    occupiedSeats++;
                    break;
                }

                if (seatLayout[y][x] == 'L')
                    break;
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