using System.IO;
using System.Linq;

namespace AoC2020.Day13
{
    public static class PartTwo
    {
        public static long Solve()
        {
            using var sr = new StreamReader("Day13/data.txt");

            // we'll use only second line
            sr.ReadLine();
            var ids = sr.ReadLine()
                        .Replace("x", "0")
                        .Split(",")
                        .Select(x => long.Parse(x))
                        .ToArray();

            var modules = ids.Where(x => x != 0).ToArray();
            var m = modules.Aggregate((a, x) => a * x);
            var rem = new long[modules.Length];

            for (int i = 0, j = 0; i < ids.Length; i++)
            {
                if (ids[i] != 0)
                {
                    // current number is ahead from 't' by 'i' iterations
                    rem[j] = modules[j] - i;
                    j++;
                }
            }

            // https://crypto.stanford.edu/pbc/notes/numbertheory/crt.html
            // https://brilliant.org/wiki/chinese-remainder-theorem/

            long x = 0;

            for (var i = 0; i < modules.Length; i++)
            {
                var b = m / modules[i];
                var bPrim = ExtendedEuclidAlgorithm(b, modules[i]);

                x += rem[i] * b * bPrim;
            }

            return x % m;
        }

        // https://brilliant.org/wiki/extended-euclidean-algorithm/
        private static long ExtendedEuclidAlgorithm(long a, long b)
        {
            long m0 = b;
            long x = 0, y = 1;
            long u = 1, v = 0;

            while (a != 0)
            {
                long q = b / a;
                long r = b % a;

                long m = x - (u * q);
                long n = y - (v * q);

                b = a;
                a = r;

                x = u;
                y = v;

                u = m;
                v = n;
            }

            if (x < 0)
                x += m0;

            return x;
        }
    }
}