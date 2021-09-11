using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day21
{
    public static class PartTwo
    {
        record Food(string[] Ingredients, string[] Allergens);
        private static Food[] foodList;
        public static int Solve()
        {
            using var sr = new StreamReader("Day21/data.txt");
            var data = sr.ReadToEnd()
                         .Replace(")", "")
                         .Split("\n");

            foodList = new Food[data.Length];

            for (var i = 0; i < data.Length; i++)
            {
                var temp = data[i].Split(" (contains ");
                var ingred = temp[0].Split(" ");
                var aller = temp[1].Split(", ");

                foodList[i] = new(ingred, aller);
            }

            var allergens = foodList.SelectMany(x => x.Allergens).ToHashSet().ToArray();

            var foo = new Dictionary<string, string[][]>();

            for (var i = 0; i < allergens.Length; i++)
            {
                foo[allergens[i]] = foodList.Where(x => x.Allergens.Contains(allergens[i]))
                                            .Select(x => x.Ingredients)
                                            .ToArray();
            }

            var bar = new Dictionary<string, List<string>>();

            foreach (var (key, value) in foo)
            {
                bar[key] = value.Skip(1)
                                .Aggregate(new HashSet<string>(value[0]), (h, e) => { h.IntersectWith(e); return h; })
                                .ToList();
            }

            foreach (var (key, item) in bar)
            {
                Console.WriteLine($"{key}: {string.Join(", ", item)}");
            }

            while (bar.Any(x => x.Value.Count != 1))
            {
                foreach (var (key, value) in bar)
                {
                    if (value.Count != 1)
                        continue;

                    var temp = bar.Where(x => x.Key != key && x.Value.Contains(value[0]))
                                  .Select(x => x.Key)
                                  .ToArray();

                    for (var i = 0; i < temp.Length; i++)
                        bar[temp[i]].Remove(value[0]);
                }
            }

            Console.WriteLine(string.Join(",", bar.OrderBy(x => x.Key).Select(x => x.Value[0])));

            return -1;
        }
    }
}