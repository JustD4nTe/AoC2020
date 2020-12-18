using System.IO;
using System.Linq;

namespace AoC2020.Day18
{
    public static class PartOne
    {
        public static ulong Solve()
        {
            using var sr = new StreamReader("Day18/data.txt");
            return sr.ReadToEnd()
                     .Replace(" ", "")
                     .Split("\n")
                     .ToArray()
                     .Select(x => Calculate(x).result)
                     .Aggregate((a, x) => a + x);
        }

        private static (ulong result, int i) Calculate(string expression, int symbolId = 0)
        {
            // number is always next to symbol
            int rightNumber = symbolId + 1;
            ulong res;

            // save first number
            if (ulong.TryParse(expression[symbolId].ToString(), out ulong number))
            {
                res = number;
                // move 'pointers'
                symbolId++;
                rightNumber++;
            }
            // but when it's "(" move 'pointer' to next character
            else
            {
                var (result, i) = Calculate(expression, symbolId + 1);
                res = result;

                // move 'pointers' by returned value
                symbolId = i;
                rightNumber = i + 1;
            }
            for (; symbolId < expression.Length || rightNumber < expression.Length; symbolId += 2, rightNumber += 2)
            {
                if (symbolId < 0)
                    return (res, symbolId);

                // end function on ")"
                if (expression[symbolId] == ')')
                    return (res, symbolId + 1);

                var symbol = expression[symbolId];

                // try get value
                if (ulong.TryParse(expression[rightNumber].ToString(), out ulong secondNumber))
                {
                    res = Compute(res, secondNumber, symbol);
                }
                // otherwise it's "("
                else
                {
                    // we have to move symbolId by 2,
                    // because we want symbol after left bracket
                    // and rightNumber indicate left bracket
                    var (result, i) = Calculate(expression, symbolId + 2);

                    if (i == -1)
                        return (Compute(res, result, symbol), -1);

                    // at the end of loop
                    // these will be incremented 
                    // which is unwanted in this situation
                    symbolId = i - 2;
                    rightNumber = i - 1;
                    res = Compute(res, result, symbol);
                }
            }

            return (res, -1);
        }

        private static ulong Compute(ulong a, ulong b, char symbol)
        => symbol switch
        {
            '+' => a + b,
            '*' => a * b,
            _ => 0
        };
    }
}
