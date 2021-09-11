using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day22
{
    public static class PartTwo
    {
        private const int P1_ID = 0;
        private const int P2_ID = 1;

        public static int Solve()
        {
            using var sr = new StreamReader("Day22/data.txt");
            var data = sr.ReadToEnd()
                         .Split("\n");

            var board = new Queue<int>[2] { new(), new() };
            var playerId = 0;

            for (var i = 1; i < data.Length; i++)
            {
                if (data[i].Contains("Player"))
                {
                    playerId = int.Parse(data[i][^2..^1]) - 1;
                    continue;
                }

                if (data[i].Length == 0)
                    continue;

                board[playerId].Enqueue(int.Parse(data[i]));
            }

            var winner = PlayGame(board).Single(x => x.Count > 0);

            var points = 0;

            for (var i = winner.Count; i > 0; i--)
                points += i * winner.Dequeue();

            return points;
        }

        private static Queue<int>[] PlayGame(Queue<int>[] board)
        {
            while (board[P1_ID].Count > 0 && board[P2_ID].Count > 0)
            {
                var p1 = board[P1_ID].Dequeue();
                var p2 = board[P2_ID].Dequeue();

                var winnerId = -1;

                if (board[P1_ID].Count >= p1 && board[P2_ID].Count >= p2)
                {
                    var subBoard = new Queue<int>[2];

                    var temp = new List<int>(board[P1_ID]);
                    subBoard[P1_ID] = new(temp.ToArray()[..p1]);

                    temp = new List<int>(board[P2_ID]);
                    subBoard[P2_ID] = new(temp.ToArray()[..p2]);

                    winnerId = PlaySubGame(subBoard);
                }

                if (winnerId == P1_ID || (winnerId == -1 && p1 > p2))
                {
                    board[P1_ID].Enqueue(p1);
                    board[P1_ID].Enqueue(p2);
                }
                else if (winnerId == P2_ID || (winnerId == -1 && p1 < p2))
                {
                    board[P2_ID].Enqueue(p2);
                    board[P2_ID].Enqueue(p1);
                }
            }

            return board;
        }

        private static int PlaySubGame(Queue<int>[] board)
        {
            var history = new List<Queue<int>[]>();

            while (board[P1_ID].Count > 0 && board[P2_ID].Count > 0)
            {
                if (history.Any(x => x[P1_ID].SequenceEqual(board[P1_ID]) && x[P2_ID].SequenceEqual(board[P2_ID])))
                    return P1_ID;

                history.Add(CopyBoard(board));

                var p1 = board[P1_ID].Dequeue();
                var p2 = board[P2_ID].Dequeue();

                var winnerId = -1;

                if (board[P1_ID].Count >= p1 && board[P2_ID].Count >= p2)
                {
                    var subBoard = new Queue<int>[2];

                    var temp = new List<int>(board[P1_ID]);
                    subBoard[P1_ID] = new(temp.ToArray()[..p1]);

                    temp = new List<int>(board[P2_ID]);
                    subBoard[P2_ID] = new(temp.ToArray()[..p2]);

                    winnerId = PlaySubGame(subBoard);
                }

                if (winnerId == P1_ID || (winnerId == -1 && p1 > p2))
                {
                    board[P1_ID].Enqueue(p1);
                    board[P1_ID].Enqueue(p2);
                }
                else if (winnerId == P2_ID || (winnerId == -1 && p1 < p2))
                {
                    board[P2_ID].Enqueue(p2);
                    board[P2_ID].Enqueue(p1);
                }
            }

            return board[P1_ID].Count == 0 ? P2_ID : P1_ID;
        }

        private static Queue<int>[] CopyBoard(Queue<int>[] board)
        {
            var copyOfBoard = new Queue<int>[2];

            copyOfBoard[P1_ID] = new(board[P1_ID]);
            copyOfBoard[P2_ID] = new(board[P2_ID]);

            return copyOfBoard;
        }
    }
}