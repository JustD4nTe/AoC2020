using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day20
{
    public static class PartOne
    {
        private const int n = 10;
        private const int boardSize = 12;
        public static ulong Solve()
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
            for(var i = 0; i < boardSize; i++)
                board[i] = new Element[boardSize];

            var solution = Backtracking(0, board, tiles.ToList());

            return solution[0][0].Id
                   * solution[0][boardSize - 1].Id
                   * solution[boardSize - 1][0].Id
                   * solution[boardSize - 1][boardSize - 1].Id;
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
    }
}