using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day7
{
    public static class PartOne
    {
        public static int Solve()
        {
            using var sr = new StreamReader("Day7/data.txt");
            var rules = sr.ReadToEnd()
                            .Replace(".", "")
                            .Replace("bags", "bag")
                            .Split("\n");

            var bags = new List<string>();
            var graph = new List<List<int>>();

            ParseData(ref rules, ref bags, ref graph);

            var bar = new HashSet<int>();
            NumberOfBagsWhichContainsGivenId(ref graph, bags.IndexOf("shiny gold"), ref bar);

            return bar.Count;
        }

        private static void ParseData(ref string[] rules, ref List<string> bags, ref List<List<int>> graph)
        {
            const string contain = "contain";

            foreach (var rule in rules)
            {
                // get name of bag 
                var bagName = rule[..rule.IndexOf("bag")].Trim();

                // add new bag into graph
                if (!bags.Any(x => x == bagName))
                {
                    bags.Add(bagName);
                    graph.Add(new List<int>(new int[rules.Length]));
                }

                // get bags which contains main bag in rule
                // (without left space)
                var contains = rule[(rule.IndexOf(contain) + contain.Length + 1)..].Split(", ");

                if (contains[0] != "no other bag")
                {
                    foreach (var subBags in contains)
                    {
                        // get amount of sub bags (optional)
                        var subBagCount = int.Parse(subBags[..subBags.IndexOf(" ")]);
                        // get name (words before "bag") and remove 
                        var subBagName = subBags[subBags.IndexOf(" ")..subBags.IndexOf("bag")].Trim();

                        // add sub bag into graph
                        if (!bags.Any(x => x == subBagName))
                        {
                            bags.Add(subBagName);
                            graph.Add(new List<int>(new int[rules.Length]));
                        }

                        // save weight into graph
                        graph[bags.IndexOf(bagName)][bags.IndexOf(subBagName)] = subBagCount;
                    }
                }
            }
        }

        private static void NumberOfBagsWhichContainsGivenId(ref List<List<int>> graph, int idToSearch, ref HashSet<int> bar)
        {
            if (!graph.Any(x => x[idToSearch] > 0))
                return;

            var ids = new List<int>();

            for (var i = 0; i < graph.Count; i++)
            {
                if (graph[i][idToSearch] > 0)
                    ids.Add(i);
            }

            bar.UnionWith(ids);

            foreach (var id in ids)
                NumberOfBagsWhichContainsGivenId(ref graph, id, ref bar);
        }
    }
}