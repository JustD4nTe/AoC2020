using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day22
{
    public static class PartOne
    {
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

            while (board[0].Count > 0 && board[1].Count > 0)
            {
                var p1 = board[0].Dequeue();
                var p2 = board[1].Dequeue();

                if (p1 > p2)
                {
                    board[0].Enqueue(p1);
                    board[0].Enqueue(p2);
                }
                else
                {
                    board[1].Enqueue(p2);
                    board[1].Enqueue(p1);
                }
            }

            var winner = board.Single(x => x.Count > 0);

            var points = 0;

            for (var i = winner.Count; i > 0; i--)
                points += i * winner.Dequeue();

            return points;
        }
    }
}