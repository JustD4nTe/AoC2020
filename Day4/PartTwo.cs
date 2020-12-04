using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020.Day4
{
    public static class PartTwo
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
                    var validFields = new[] { 0, 0, 0, 0, 0, 0, 0 };

                    var fields = document.Replace("\n", " ")
                                        .Split(" ")
                                        .Select(x => x.Split(":"))
                                        .ToDictionary(x => x[0], x => x[1]);

                    if (fields["byr"].Length == 4)
                    {
                        var birthYear = int.Parse(fields["byr"]);
                        if (birthYear >= 1920 && birthYear <= 2002)
                            validFields[0] = 1;
                    }

                    if (fields["iyr"].Length == 4)
                    {
                        var issueYear = int.Parse(fields["iyr"]);
                        if (issueYear >= 2010 && issueYear <= 2020)
                            validFields[1] = 1;
                    }

                    if (fields["eyr"].Length == 4)
                    {
                        var expYear = int.Parse(fields["eyr"]);
                        if (expYear >= 2020 && expYear <= 2030)
                            validFields[2] = 1;
                    }

                    if (fields["hgt"][^2..] == "cm") // get last 2 char
                    {
                        var height = int.Parse(fields["hgt"][..^2]); // get all expect 2 last char
                        if (height >= 150 && height <= 193)
                            validFields[3] = 1;
                    }
                    else if (fields["hgt"][^2..] == "in")
                    {
                        var height = int.Parse(fields["hgt"][..^2]);
                        if (height >= 59 && height <= 76)
                            validFields[3] = 1;
                    }

                    if (fields["hcl"][0] == '#' && fields["hcl"].Length == 7)
                    {
                        var reg = new Regex("[a-f0-9]");
                        if (reg.IsMatch(fields["hcl"][1..]))
                            validFields[4] = 1;
                    }

                    if (fields["ecl"].Length == 3)
                    {
                        var reg = new Regex("amb|blu|brn|gry|grn|hzl|oth");
                        if (reg.IsMatch(fields["ecl"]))
                            validFields[5] = 1;
                    }

                    if (fields["pid"].Length == 9)
                    {
                        var reg = new Regex("[0-9]");
                        if (reg.IsMatch(fields["pid"][1..]))
                            validFields[6] = 1;
                    }

                    if (IsFieldValid(validFields))
                        validPassportCount++;
                }
            }

            return validPassportCount;
        }

        private static bool IsFieldValid(int[] validFields)
            => !validFields.Any(x => x == 0);
    }
}