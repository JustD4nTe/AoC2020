using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day24
{
    public static class PartTwo
    {
        private enum Directions
        {
            East,
            SouthEast,
            SouthWest,
            West,
            NorthWest,
            NorthEast
        }

        private const int _n = 150;

        public static int Solve()
        {
            using var sr = new StreamReader("Day24/data.txt");
            var data = sr.ReadToEnd()
                         .Split("\n");

            var instructions = new List<List<Directions>>();

            foreach (var instruction in data)
            {
                var temp = new List<Directions>();

                for (var i = 0; i < instruction.Length; i++)
                {
                    if (instruction[i] == 'e')
                    {
                        temp.Add(Directions.East);
                    }
                    else if (instruction[i] == 's')
                    {
                        i++;

                        if (instruction[i] == 'e')
                            temp.Add(Directions.SouthEast);
                        else if (instruction[i] == 'w')
                            temp.Add(Directions.SouthWest);
                    }
                    else if (instruction[i] == 'n')
                    {
                        i++;

                        if (instruction[i] == 'e')
                            temp.Add(Directions.NorthEast);
                        else if (instruction[i] == 'w')
                            temp.Add(Directions.NorthWest);
                    }
                    else if (instruction[i] == 'w')
                    {
                        temp.Add(Directions.West);
                    }
                }

                instructions.Add(temp);
            }

            var floor = new bool[_n][];

            for (var i = 0; i < _n; i++)
                floor[i] = new bool[_n];

            foreach (var instruction in instructions)
            {
                var refTitle = (x: _n / 2, y: _n / 2);

                foreach (var direction in instruction)
                {
                    var isShifted = refTitle.y % 2 == 1;

                    switch (direction)
                    {
                        case Directions.East:
                            refTitle.x++;
                            break;

                        case Directions.SouthEast:
                            if (isShifted)
                                refTitle.x++;
                            refTitle.y++;
                            break;

                        case Directions.SouthWest:
                            if (!isShifted)
                                refTitle.x--;
                            refTitle.y++;
                            break;

                        case Directions.West:
                            refTitle.x--;
                            break;

                        case Directions.NorthWest:
                            if (!isShifted)
                                refTitle.x--;
                            refTitle.y--;
                            break;

                        case Directions.NorthEast:
                            if (isShifted)
                                refTitle.x++;
                            refTitle.y--;
                            break;
                    }
                }

                floor[refTitle.y][refTitle.x] = !floor[refTitle.y][refTitle.x];
            }

            Console.WriteLine($"Day {0}: {floor.Sum(x => x.Count(y => y))}");

            for (var i = 0; i < 100; i++)
            {
                var nextDayFlood = new bool[_n][];

                for (var j = 0; j < _n; j++)
                    nextDayFlood[j] = floor[j].Clone() as bool[];

                for (var y = 0; y < _n; y++)
                {
                    for (var x = 0; x < _n; x++)
                    {
                        var counter = CountBlack(ref floor, x, y);

                        // Black
                        if (floor[y][x] && (counter > 2 || counter == 0))
                            nextDayFlood[y][x] = false;

                        // White
                        if (!floor[y][x] && counter == 2)
                            nextDayFlood[y][x] = true;
                    }
                }

                for (var j = 0; j < _n; j++)
                    floor[j] = nextDayFlood[j].Clone() as bool[];

                Console.WriteLine($"Day {i + 1}: {floor.Sum(x => x.Count(y => y))}");
            }

            return floor.Sum(x => x.Count(y => y));
        }

        private static int CountBlack(ref bool[][] floor, int x, int y)
        {
            var isShifted = y % 2 == 1;

            var counter = 0;

            // West
            if (x - 1 >= 0 && floor[y][x - 1])
                counter++;

            // East
            if (x + 1 < _n && floor[y][x + 1])
                counter++;

            // South East
            if (isShifted && x + 1 < _n && y + 1 < _n && floor[y + 1][x + 1])
                counter++;
            else if (!isShifted && y + 1 < _n && floor[y + 1][x])
                counter++;

            // South West
            if (!isShifted && x - 1 >= 0 && y + 1 < _n && floor[y + 1][x - 1])
                counter++;
            else if (isShifted && y + 1 < _n && floor[y + 1][x])
                counter++;

            // North East
            if (isShifted && x + 1 < _n && y - 1 >= 0 && floor[y - 1][x + 1])
                counter++;
            else if (!isShifted && y - 1 >= 0 && floor[y - 1][x])
                counter++;

            // North West
            if (!isShifted && x - 1 >= 0 && y - 1 >= 0 && floor[y - 1][x - 1])
                counter++;
            else if (isShifted && y - 1 >= 0 && floor[y - 1][x])
                counter++;

            return counter;
        }
    }
}