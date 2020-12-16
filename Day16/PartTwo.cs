using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day16
{
    public static class PartTwo
    {
        public static ulong Solve()
        {
            using var sr = new StreamReader("Day16/data.txt");
            var fields = new Dictionary<string, List<int>>();
            // store information about column id for each field
            var fieldsCol = new Dictionary<string, List<int>>();

            // read all rules
            for (var i = 0; i < 20; i++)
            {
                var rule = sr.ReadLine();

                var colonId = rule.IndexOf(":");
                var fieldName = rule[..colonId];

                fieldsCol[fieldName] = new List<int>();

                rule = rule[(colonId + 2)..];

                // parse ranges
                var ranges = rule.Split(" or ")
                              .Select(x => x.Split("-")
                                            .Select(x => int.Parse(x))
                                            .ToArray())
                               .ToArray();

                fields[fieldName] = GetRanges(ranges);
            }

            // avoid next 2 lines
            sr.ReadLine();
            sr.ReadLine();

            var myTicket = sr.ReadLine()
                             .Split(",")
                             .Select(x => ulong.Parse(x))
                             .ToArray();

            // avoid next 2 lines
            sr.ReadLine();
            sr.ReadLine();

            // get all tickets
            var tickets = sr.ReadToEnd()
                            .Split("\n")
                            .Select(x => x.Split(",")
                                          .Select(x => int.Parse(x))
                                          .ToArray())
                            .ToList();

            var validTickets = new List<int[]>();

            for (var i = 0; i < tickets.Count; i++)
                validTickets.Add(tickets[i].Clone() as int[]);

            // remove invalid tickets
            for (var i = tickets.Count - 1; i >= 0; i--)
            {
                for (var j = tickets[i].Length - 1; j >= 0; j--)
                {
                    if (!fields.Any(x => x.Value.Contains(tickets[i][j])))
                    {
                        validTickets.RemoveAt(i);
                        break;
                    }
                }
            }

            // get information at which column fields are valid
            foreach (var field in fields)
            {
                for (var i = 0; i < validTickets[0].Length; i++)
                {
                    if (validTickets.Select(x => x[i]).Except(field.Value).Any())
                        continue;

                    fieldsCol[field.Key].Add(i);
                }
            }

            // get all columns of departure fields
            var departuresCol = UniqueFieldsCol(fieldsCol)
                                    .Where(x => x.Key.Contains("departure"))
                                    .Select(x => x.Value[0])
                                    .ToArray();

            ulong res = 1;

            for (var i = 0; i < departuresCol.Length; i++)
                res *= myTicket[departuresCol[i]];

            return res;
        }

        // return list with numbers by given ranges
        private static List<int> GetRanges(int[][] numbers)
        {
            var foo = new List<int>();

            for (var i = 0; i < numbers.Length; i++)
            {
                for (var j = numbers[i][0]; j <= numbers[i][1]; j++)
                    foo.Add(j);
            }

            return foo;
        }

        // get unique id for every field
        private static Dictionary<string, List<int>> UniqueFieldsCol(Dictionary<string, List<int>> fields)
        {
            // return when all fields have only 1 column
            if (fields.Select(x => x.Value).All(x => x.Count == 1))
                return fields;

            var keys = fields.Keys.ToArray();

            for (var i = 0; i < keys.Length; i++)
            {
                // when current field have only 1 column
                // we have to remove this column in other fields
                if (fields[keys[i]].Count == 1)
                {
                    for (var j = 0; j < keys.Length; j++)
                    {
                        if (j != i)
                            fields[keys[j]].Remove(fields[keys[i]][0]);
                    }
                }
            }

            return UniqueFieldsCol(fields);
        }
    }
}