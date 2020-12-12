using System.IO;

namespace AoC2020.Day12
{
    public static class PartOne
    {
        private static int north;
        private static int east;
        private static int south;
        private static int west;

        public static int Solve()
        {
            using var sr = new StreamReader("Day12/data.txt");
            var instructions = sr.ReadToEnd().Split("\n");

            var shipDirection = DirectionsEnum.East;

            foreach (var instruction in instructions)
            {
                // eg. instruction = L90 => direction = L; units = 90;
                var direction = instruction[..1];
                var units = int.Parse(instruction[1..]);

                switch (direction)
                {
                    case "L":
                        // rotate ship by given direction and degree
                        shipDirection = (DirectionsEnum)((byte)(shipDirection - (byte)(units / 90)) % 4);
                        break;
                    case "R":
                        shipDirection = (DirectionsEnum)((byte)(shipDirection + (byte)(units / 90)) % 4);
                        break;
                    case "F":
                        Move(shipDirection, units);
                        break;
                    default:
                        Move(GetDirection(direction), units);
                        break;
                }
            }

            return GetManhattanDistance();
        }

        private static void Move(DirectionsEnum direction, int units)
        {
            switch (direction)
            {
                case DirectionsEnum.North:
                    AdjustOppositeDirections(ref north, ref south, units);
                    break;

                case DirectionsEnum.East:
                    AdjustOppositeDirections(ref east, ref west, units);
                    break;

                case DirectionsEnum.South:
                    AdjustOppositeDirections(ref south, ref north, units);
                    break;

                case DirectionsEnum.West:
                    AdjustOppositeDirections(ref west, ref east, units);
                    break;
            }
        }

        private static void AdjustOppositeDirections(ref int directionTo, ref int oppositeDirection, int units)
        {
            if (oppositeDirection > units)
            {
                oppositeDirection -= units;
            }
            else
            {
                units -= oppositeDirection;
                oppositeDirection = 0;
                directionTo += units;
            }
        }

        private static int GetManhattanDistance()
            => (north, east, south, west) switch
            {
                (_, _, 0, 0) => north + east,
                (_, 0, 0, _) => north + west,
                (0, 0, _, _) => south + west,
                (0, _, _, 0) => south + east,

                _ => -1
            };

        private static DirectionsEnum GetDirection(string direction)
        => direction switch
        {
            "N" => DirectionsEnum.North,
            "E" => DirectionsEnum.East,
            "S" => DirectionsEnum.South,
            "W" => DirectionsEnum.West
        };
    }
}