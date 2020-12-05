using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2020.Day5
{
    public static class PartTwo
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day5/data.txt");

            var seatId = 0;

            var occupiedSeats = GetListOfSeatsId(sr.ReadToEnd().Split("\r\n"));

            occupiedSeats.Sort();
            var emptySeats = new List<int>();

            for (var i = 0; i < occupiedSeats[^1]; i++)
            {
                if (!occupiedSeats.Contains(i))
                    emptySeats.Add(i);
            }

            for (var i = 1; i < emptySeats.Count - 1; i++)
            {
                if (emptySeats[i - 1] + 1 != emptySeats[i]
                    && emptySeats[i + 1] - 1 != emptySeats[i])
                {
                    seatId = i;
                }
            }

            if (seatId == 0)
                seatId = emptySeats[^1];

            return seatId;
        }

        private static List<int> GetListOfSeatsId(string[] boardingPasses)
        {
            var collectionOfSeatsId = new List<int>();

            foreach (var boardingPass in boardingPasses)
            {
                var yMin = 0;
                var yMax = 127;

                for (var i = 0; i < 7; i++)
                {
                    var diff = (int)Math.Round((yMax - yMin) / 2d);

                    if (boardingPass[i] == 'F')
                        yMax -= diff;
                    else if (boardingPass[i] == 'B')
                        yMin += diff;
                }

                var row = boardingPass[6] == 'F' ? yMin : yMax;

                var xMin = 0;
                var xMax = 7;

                for (var i = 0; i < 3; i++)
                {
                    var diff = (int)Math.Round((xMax - xMin) / 2d);

                    if (boardingPass[i + 7] == 'L')
                        xMax -= diff;
                    else if (boardingPass[i + 7] == 'R')
                        xMin += diff;
                }

                var column = boardingPass[9] == 'L' ? xMin : xMax;

                var seatId = (row * 8) + column;

                collectionOfSeatsId.Add(seatId);
            }
            return collectionOfSeatsId;
        }
    }
}