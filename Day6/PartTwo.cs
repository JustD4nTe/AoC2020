using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day6
{
    public static class PartTwo
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day6/data.txt");
            var answers = sr.ReadToEnd().Split("\r\n");

            var sum = 0;

            var singleAnswers = new Dictionary<char, int>();
            var numberOfAnswers = 0;

            foreach (var groupAnswers in answers)
            {
                if (string.IsNullOrWhiteSpace(groupAnswers))
                {
                    sum += singleAnswers.Count(d => d.Value == numberOfAnswers);
                    numberOfAnswers = 0;
                    singleAnswers.Clear();
                }
                else
                {
                    foreach (var answer in groupAnswers)
                    {
                        if (singleAnswers.ContainsKey(answer))
                        {
                            singleAnswers[answer]++;
                        }
                        else
                        {
                            singleAnswers[answer] = 1;
                        }
                    }

                    numberOfAnswers++;
                }
            }

            sum += singleAnswers.Count(d => d.Value == numberOfAnswers);

            return sum;
        }
    }
}