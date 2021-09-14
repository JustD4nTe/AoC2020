using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day24
{
    public static class PartOne
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

            var floor = new bool[100][];

            for (var i = 0; i < 100; i++)
                floor[i] = new bool[100];

            foreach (var instruction in instructions)
            {
                var refTitle = (x: 50, y: 50);

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

            return floor.Sum(x => x.Count(y => y));
        }
    }
}