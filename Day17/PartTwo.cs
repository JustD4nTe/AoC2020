using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day17
{
    public static class PartTwo
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day17/data.txt");

            var initialState = sr.ReadToEnd()
                                 .Split("\n")
                                 .Select(x => x.ToCharArray().ToList())
                                 .ToList();

            // add initial state
            var pocketDimension = new List<List<List<List<char>>>>
            {
                new List<List<List<char>>>(){
                    Copy2D(initialState)
                }
            };

            // boot up
            for (var cycle = 1; cycle <= 6; cycle++)
            {
                // 1. Expand dimensions
                ExpandIn4D(ref pocketDimension);

                // 2. Execute cycle
                pocketDimension = ExecuteCycle(pocketDimension);

                // 3. (optional) write each cycle to file 
                WritePocketDimension(pocketDimension, cycle);
            }

            // count numbers of active cubes
            return pocketDimension.SelectMany(x => x.SelectMany(x => x.SelectMany(x => x))).Count(x => x == '#');
        }

        // generate list size x size, filled with '.' (inactive cube)
        private static List<List<char>> GenerateEmptySpace(int size)
        {
            var emptySpace = new List<List<char>>();

            for (var i = 0; i < size; i++)
            {
                emptySpace.Add(new List<char>());

                for (var j = 0; j < size; j++)
                    emptySpace[i].Add('.');
            }

            return emptySpace;
        }

        // generate 3d dimensions, filled by empty space
        private static List<List<List<char>>> GenerateEmptyDimension(int count, int insideSize)
        {
            var temp = new List<List<List<char>>>();

            for (var i = 0; i < count; i++)
                temp.Add(GenerateEmptySpace(insideSize));

            return temp;
        }

        // fill '.' around 2d dimension
        private static List<List<char>> ExpandIn2D(List<List<char>> dimension)
        {
            // fill at the top 
            dimension.Insert(0, Enumerable.Range(0, dimension[0].Count + 2)
                                            .Select(_ => '.')
                                            .ToList());

            // fill at the left and right
            for (var y = 1; y < dimension.Count; y++)
            {
                dimension[y].Insert(0, '.');
                dimension[y].Add('.');
            }

            // fill at the bottom 
            dimension.Add(Enumerable.Range(0, dimension[0].Count)
                                    .Select(_ => '.')
                                    .ToList());

            return dimension;
        }

        // fill '.' around 3d dimension
        private static List<List<List<char>>> ExpandIn3D(List<List<List<char>>> dimension)
        {
            // expand every exists dimension in 2d
            for (var z = 0; z < dimension.Count; z++)
                dimension[z] = ExpandIn2D(dimension[z]);

            // add empty space on both sides
            dimension.Insert(0, GenerateEmptySpace(dimension[0].Count));
            dimension.Add(GenerateEmptySpace(dimension[0].Count));

            return dimension;
        }

        // fill '.' around 4d dimension
        private static void ExpandIn4D(ref List<List<List<List<char>>>> dimension)
        {
            // expand every 3d dimension for each time dimension
            for (var w = 0; w < dimension.Count; w++)
                dimension[w] = ExpandIn3D(dimension[w]);

            // add empty dimension for every in time (??)
            dimension.Insert(0, GenerateEmptyDimension(dimension[0].Count, dimension[0][0].Count));
            dimension.Add(GenerateEmptyDimension(dimension[0].Count, dimension[0][0].Count));
        }

        private static List<List<char>> Copy2D(List<List<char>> dimension)
        {
            var copy = new List<List<char>>();

            for (var y = 0; y < dimension.Count; y++)
                copy.Add(new List<char>(dimension[y]));

            return copy;
        }

        private static List<List<List<char>>> Copy3D(List<List<List<char>>> dimension)
        {
            var copy = new List<List<List<char>>>();

            for (var z = 0; z < dimension.Count; z++)
                copy.Add(Copy2D(dimension[z]));

            return copy;
        }

        private static List<List<List<List<char>>>> Copy4D(List<List<List<List<char>>>> dimension)
        {
            var copy = new List<List<List<List<char>>>>();

            for (var w = 0; w < dimension.Count; w++)
                copy.Add(Copy3D(dimension[w]));

            return copy;
        }

        // count the active neighbors around given cube
        private static int CountActiveNeighbors(List<List<List<List<char>>>> dimension,
                                                int cubeX, int cubeY,
                                                int cubeZ, int cubeW)
        {
            // coords difference of cubes should be only 1
            var diff = new[] { -1, 0, 1 };

            var numberOfActiveCubes = 0;

            foreach (var w in diff)
            {
                var currW = w + cubeW;
                if (currW < 0 || currW == dimension.Count)
                    continue;

                foreach (var z in diff)
                {
                    var currZ = z + cubeZ;
                    if (currZ < 0 || currZ == dimension[currW].Count)
                        continue;

                    foreach (var y in diff)
                    {
                        var currY = y + cubeY;
                        if (currY < 0 || currY == dimension[currW][currZ].Count)
                            continue;

                        foreach (var x in diff)
                        {
                            var currX = x + cubeX;
                            if (currX < 0 || currX == dimension[currW][currZ][currY].Count)
                                continue;

                            // we don't have to count given cube
                            if (x == 0 && y == 0 && z == 0 && w == 0)
                                continue;

                            if (dimension[currW][currZ][currY][currX] == '#')
                                numberOfActiveCubes++;
                        }
                    }
                }
            }

            return numberOfActiveCubes;
        }

        // try change state for every cube based on neighborhood
        private static List<List<List<List<char>>>> ExecuteCycle(List<List<List<List<char>>>> dimension)
        {
            // bcs cycle should execute simultaneously 
            // we have to store changes in different object
            var dimensionClone = Copy4D(dimension);

            for (var w = 0; w < dimension.Count; w++)
            {
                for (var z = 0; z < dimension[w].Count; z++)
                {
                    for (var y = 0; y < dimension[w][z].Count; y++)
                    {
                        for (var x = 0; x < dimension[w][z][y].Count; x++)
                        {
                            var activeNeighborsCount = CountActiveNeighbors(dimension, x, y, z, w);

                            if (!(dimension[w][z][y][x] == '#' && (activeNeighborsCount == 2 || activeNeighborsCount == 3)))
                                dimensionClone[w][z][y][x] = '.';
                            if (dimension[w][z][y][x] == '.' && activeNeighborsCount == 3)
                                dimensionClone[w][z][y][x] = '#';
                        }
                    }
                }
            }

            return dimensionClone;
        }

        private static void WritePocketDimension(List<List<List<List<char>>>> dimension, int cycle)
        {
            using var sw = new StreamWriter($"Day17/Cycle{cycle}.txt");

            var w = -(dimension.Count - 1) / 2;

            for (var i = 0; i < dimension.Count; i++, w++)
            {
                var z = -(dimension[i].Count - 1) / 2;

                for (var j = 0; j < dimension[i].Count; j++, z++)
                {
                    // z == 0, w == 0 is 'initial state'/center
                    sw.WriteLine($"z= {z}, w= {w}");

                    for (var y = 0; y < dimension[i][j].Count; y++)
                    {
                        for (var x = 0; x < dimension[i][j][y].Count; x++)
                            sw.Write(dimension[i][j][y][x]);

                        sw.WriteLine();
                    }

                    sw.WriteLine();
                }

                sw.WriteLine();
            }
        }
    }
}