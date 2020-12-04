using System.IO;
using System.Linq;

namespace AoC2020.Day4
{
    public static class PartOne
    {
        public static int Solve()
        {
            var sr = new StreamReader("Day4/data.txt");
            var data = sr.ReadToEnd().Split("\n\n");

            var validPassportCount = 0;

            foreach (var document in data)
            {
                if (document.Contains("byr")
                    && document.Contains("iyr")
                    && document.Contains("eyr")
                    && document.Contains("hgt")
                    && document.Contains("hcl")
                    && document.Contains("ecl")
                    && document.Contains("pid"))
                {
                    validPassportCount++;
                }
            }

            return validPassportCount;
        }
    }
}