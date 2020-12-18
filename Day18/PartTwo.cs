using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day18
{
    public static class PartTwo
    {
        public static ulong Solve()
        {
            using var sr = new StreamReader("Day18/data.txt");
            return sr.ReadToEnd()
                     .Replace("(", "( ")
                     .Replace(")", " )")
                     .Split("\n")
                     .Select(x => x.Split(" ").ToArray())
                     .Select(x => CalculateRPN(InToRPN(x)))
                     .Aggregate((a, x) => a + x);
        }

        // Infix notation (normal) to Reverse Polish Notation
        private static List<string> InToRPN(string[] expression)
        {
            var operatorPriority = new Dictionary<string, byte>
            {
                ["("] = 0,
                ["*"] = 1,
                [")"] = 1,
                ["+"] = 2
            };

            var heap = new List<string>();
            var output = new List<string>();
            var isNotAdded = true;

            // go through each digit/number in the equation
            for (var i = 0; i < expression.Length; i++)
            {
                // when element is a number
                // put it in the output
                if (ulong.TryParse(expression[i], out _))
                {
                    output.Add(expression[i]);
                }
                // add to the beginning of the heap
                else if (expression[i] == "(")
                {
                    heap.Insert(0, expression[i]);
                }
                else if (expression[i] == ")")
                {
                    var temp = GetSymbolsBetweenBrackets(heap);
                    heap = temp.Item1;
                    output.AddRange(temp.Item2);
                }
                else if (heap.Count == 0)
                {
                    heap.Add(expression[i]);
                }
                else if (operatorPriority[expression[i]] > operatorPriority[heap[0]])
                {
                    heap.Insert(0, expression[i]);
                }
                else
                {
                    for (var j = 0; j < heap.Count; j++)
                    {
                        if (heap[0] == "(")
                        {
                            heap.Insert(0, expression[i]);
                            isNotAdded = false;
                            break;
                        }
                        else if (operatorPriority[expression[i]] <= operatorPriority[heap[0]])
                        {
                            output.Add(heap[0]);
                            heap.Remove(heap[0]);
                        }
                    }

                    if (isNotAdded)
                        heap.Insert(0, expression[i]);
                    else
                        isNotAdded = true;
                }
            }

            // add the remainder in the output
            output.AddRange(heap);
            return output;
        }

        private static (List<string>, List<string>) GetSymbolsBetweenBrackets(List<string> heap)
        {
            var output = new List<string>();

            // get all symbols between brackets
            // and return other symbols (without left bracket)

            while (true)
            {
                if (heap[0] != "(")
                {
                    output.Add(heap[0]);
                    heap.Remove(heap[0]);
                }
                else
                {
                    heap.Remove(heap[0]);
                    return (heap, output);
                }
            }
        }

        private static ulong CalculateRPN(List<string> equation)
        {
            var heap = new Stack<string>();

            for (var i = 0; i < equation.Count; i++)
            {
                // push number on heap
                if (ulong.TryParse(equation[i], out _))
                {
                    heap.Push(equation[i]);
                }
                // when current element is symbol
                // get two numbers and calculate result
                // then push result on heap
                else if (equation[i] == "+" || equation[i] == "*")
                {
                    var left = ulong.Parse(heap.Pop());
                    var right = ulong.Parse(heap.Pop());
                    heap.Push(Compute(left, right, equation[i]));
                }
            }

            return ulong.Parse(heap.Pop());
        }

        private static string Compute(ulong a, ulong b, string symbol)
        => symbol switch
        {
            "+" => (a + b).ToString(),
            "*" => (a * b).ToString(),
            _ => "0"
        };
    }
}