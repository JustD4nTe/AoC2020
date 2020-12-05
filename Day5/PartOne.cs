using System;
using System.IO;

namespace AoC2020.Day5
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day5/data.txt");
            var boardingPasses = sr.ReadToEnd().Split("\r\n");

            var maxSeatId = 0;

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

                if (maxSeatId < seatId)
                    maxSeatId = seatId;
            }

            return maxSeatId;
        }
    }
}