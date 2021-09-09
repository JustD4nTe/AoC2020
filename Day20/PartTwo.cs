using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020.Day20
{
    public static class PartTwo
    {
        private const int n = 10;
        private const int boardSize = 12;
        private const int imageSize = (n - 2) * boardSize;
        public static int Solve()
        {            
            var tiles = new List<Element>();

            using var sr = new StreamReader("Day20/data.txt");
            var data = sr.ReadToEnd()
                         .Replace("\n\n", "\n")
                         .Split("\n");

            for (var i = 0; i < data.Length;)
            {
                var id = ulong.Parse(data[i++][5..^1]);
                var rows = new string[n];

                for (var j = 0; j < n; j++)
                    rows[j] = data[i++];

                tiles.Add(new Element(rows, id));
            }

            var board = new Element[boardSize][];
            for (var i = 0; i < boardSize; i++)
                board[i] = new Element[boardSize];

            var solution = Backtracking(0, board, tiles.ToList());

            var temp = solution.Select(x => x.Select(y => y.GetStringImage()).ToArray()).ToArray();

            var (image, waterRoughness) = GetImage(temp);

            var (numberOfMonsters, monsterVisiblePartCount) = SearchMonsters(image);

            return waterRoughness - (monsterVisiblePartCount * numberOfMonsters);
        }

        private static bool IsValidDownUp(Element up, Element down)
        {
            if (up.Edge.Down != down.Edge.Up)
                return false;

            for (var i = 0; i < n; i++)
            {
                if (up.Image[n - 1][i] != down.Image[0][i])
                    return false;
            }

            return true;
        }

        private static bool IsValidRightLeft(Element left, Element right)
        {
            if (left.Edge.Right != right.Edge.Left)
                return false;

            for (var i = 0; i < n; i++)
            {
                if (left.Image[i][n - 1] != right.Image[i][0])
                    return false;
            }

            return true;
        }

        private static Element[][] Backtracking(int xy, Element[][] board, List<Element> puzzles)
        {
            if (xy == boardSize * boardSize)
                return board;

            int x = xy / boardSize;
            int y = xy % boardSize;

            for (var i = 0; i < puzzles.Count; i++)
            {
                var variations = puzzles[i].GetAllVariantions();

                var freePuzzles = puzzles.ToList();
                freePuzzles.Remove(puzzles[i]);

                for (var j = 0; j < variations.Count; j++)
                {
                    if (x > 0 && !IsValidDownUp(board[x - 1][y], variations[j]))
                        continue;

                    if (y > 0 && !IsValidRightLeft(board[x][y - 1], variations[j]))
                        continue;

                    board[x][y] = variations[j];
                    var newBoard = board.Clone() as Element[][];

                    var solution = Backtracking(xy + 1, newBoard, freePuzzles.ToList());
                    if (solution is not null)
                        return solution;
                }
            }

            return null;
        }

        private static (char[][], int) GetImage(string[][][] board)
        {
            var image = new char[imageSize][];

            for (var i = 0; i < imageSize; i++)
                image[i] = string.Concat(board[i / (n - 2)].Select(x => x[i % (n - 2)])).ToArray();

            return (image, image.Sum(x => x.Count(y => y == '#')));
        }

        private static (int, int) SearchMonsters(char[][] image)
        {
            var monster = new[]{
                "..................#.",
                "#....##....##....###",
                ".#..#..#..#..#..#..."
            };

            var monsterPattern = monster.Select(x => new Regex(x)).ToArray();

            var monsterWidth = monster[0].Length;
            var monsterHeight = monster.Length;

            var rotate90 = MatrixHelper.Rotate(image, imageSize);
            var rotate180 = MatrixHelper.Rotate(rotate90, imageSize);
            var rotate270 = MatrixHelper.Rotate(rotate180, imageSize);

            var refl1 = MatrixHelper.Flip(image, imageSize);
            var refl2 = MatrixHelper.Flip(rotate90, imageSize);
            var refl3 = MatrixHelper.Flip(rotate180, imageSize);
            var refl4 = MatrixHelper.Flip(rotate270, imageSize);

            var orientations = new List<char[][]>(){
                image,
                rotate90,
                rotate180,
                rotate270,
                refl1,
                refl2,
                refl3,
                refl4
            };

            var monsterCount = 0;

            foreach (var orientation in orientations)
            {
                for (var i = 0; i < imageSize - monsterHeight + 1; i++)
                {
                    for (var j = 0; j < imageSize - monsterWidth + 1; j++)
                    {
                        var isMonster = monsterPattern[0].IsMatch(new string(orientation[i][j..(j + monsterWidth)]))
                                     && monsterPattern[1].IsMatch(new string(orientation[i + 1][j..(j + monsterWidth)]))
                                     && monsterPattern[2].IsMatch(new string(orientation[i + 2][j..(j + monsterWidth)]));

                        if (isMonster)
                            monsterCount++;
                    }
                }
            }

            return (monsterCount, monster.Sum(x => x.Count(y => y == '#')));
        }
    }
}