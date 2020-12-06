using System.Collections.Generic;
using System.IO;

namespace AoC2020.Day6
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day6/data.txt");
            var answers = sr.ReadToEnd().Split("\r\n");

            var sum = 0;

            var collectionOfSingleAnswers = new HashSet<char>();

            foreach (var groupAnswers in answers)
            {
                if (string.IsNullOrWhiteSpace(groupAnswers))
                {
                    sum += collectionOfSingleAnswers.Count;
                    collectionOfSingleAnswers.Clear();
                }
                else
                {
                    collectionOfSingleAnswers.UnionWith(groupAnswers);
                }
            }

            sum += collectionOfSingleAnswers.Count;

            return sum;
        }
    }
}