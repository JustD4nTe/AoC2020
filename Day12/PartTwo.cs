using System;
using System.IO;
using System.Linq;

namespace AoC2020.Day12
{
    public static class PartTwo
    {
        private static int[] shipPos;
        private static int[] waypointPos;

        public static int Solve()
        {
            using var sr = new StreamReader("Day12/data.txt");
            var instructions = sr.ReadToEnd().Split("\n");

            //                N  E  S  W
            shipPos = new[] { 0, 0, 0, 0 };
            waypointPos = new[] { 1, 10, 0, 0 };

            foreach (var instruction in instructions)
            {
                // eg. instruction = L90 => direction = L; units = 90;
                var direction = instruction[..1];
                var units = int.Parse(instruction[1..]);

                switch (direction)
                {
                    case "L":
                        RotateWaypoint(units / 90, false);
                        break;
                    case "R":
                        RotateWaypoint(units / 90, true);
                        break;
                    case "F":
                        // Move the ship relative to the waypoint
                        if (waypointPos[0] > 0)
                            Move(DirectionsEnum.North, units * waypointPos[0], ref shipPos);
                        else if (waypointPos[2] > 0)
                            Move(DirectionsEnum.South, units * waypointPos[2], ref shipPos);

                        if (waypointPos[1] > 0)
                            Move(DirectionsEnum.East, units * waypointPos[1], ref shipPos);
                        else if (waypointPos[3] > 0)
                            Move(DirectionsEnum.West, units * waypointPos[3], ref shipPos);
                        break;

                    default:
                        // Move the waypoint
                        Move(GetDirection(direction), units, ref waypointPos);
                        break;
                }
            }

            return shipPos.Sum();
        }

        private static void Move(DirectionsEnum direction, int units, ref int[] objToMove)
        {
            switch (direction)
            {
                case DirectionsEnum.North:
                    AdjustOppositeDirections(ref objToMove[0], ref objToMove[2], units);
                    break;

                case DirectionsEnum.East:
                    AdjustOppositeDirections(ref objToMove[1], ref objToMove[3], units);
                    break;

                case DirectionsEnum.South:
                    AdjustOppositeDirections(ref objToMove[2], ref objToMove[0], units);
                    break;

                case DirectionsEnum.West:
                    AdjustOppositeDirections(ref objToMove[3], ref objToMove[1], units);
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

        private static DirectionsEnum GetDirection(string direction)
        => direction switch
        {
            "N" => DirectionsEnum.North,
            "E" => DirectionsEnum.East,
            "S" => DirectionsEnum.South,
            "W" => DirectionsEnum.West
        };

        // Shift array by given value
        private static void RotateWaypoint(int times, bool isClockwise)
        {
            var temp = new int[waypointPos.Length];

            for (var i = 0; i < waypointPos.Length; i++)
            {
                // when we are moving clockwise,
                // we have to add length of array to prevent negative numbers
                if (isClockwise)
                    temp[i] = waypointPos[(i - times + waypointPos.Length) % waypointPos.Length];
                else
                    temp[i] = waypointPos[(i + times) % waypointPos.Length];
            }

            waypointPos = temp;
        }
    }
}