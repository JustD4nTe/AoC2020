using System;
using System.IO;
using System.Linq;

namespace AoC2020.Day25
{
    public static class PartOne
    {
        public static ulong Solve()
        {
            using var sr = new StreamReader("Day25/data.txt");
            var data = sr.ReadToEnd()
                         .Split("\n")
                         .Select(x => ulong.Parse(x))
                         .ToArray();

            var cardPublicKey = data[0];
            var doorPublicKey = data[1];

            const ulong subjectNumber = 7;

            ulong doorLoopSize = 0, cardLoopSize = 0;

            ulong value = 1;

            for (ulong i = 0; doorLoopSize == 0 || cardLoopSize == 0; i++)
            {
                value *= subjectNumber;
                value %= 20201227;

                if (value == cardPublicKey)
                    cardLoopSize = i + 1;

                if (value == doorPublicKey)
                    doorLoopSize = i + 1;
            }

            return TransformSubjectNumber(doorPublicKey, cardLoopSize);
        }

        private static ulong TransformSubjectNumber(ulong subjectNumber, ulong loopSize)
        {
            ulong value = 1;

            for (ulong i = 0; i < loopSize; i++)
            {
                value *= subjectNumber;
                value %= 20201227;
            }

            return value;
        }
    }
}