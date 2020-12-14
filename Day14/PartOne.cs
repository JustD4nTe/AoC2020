using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day14
{
    public static class PartOne
    {
        public static long Solve()
        {
            using var sr = new StreamReader("Day14/data.txt");
            var program = sr.ReadToEnd().Split("\n").ToArray();
            var memory = new Dictionary<uint, long>();
            var mask = "";

            for (var line = 0; line < program.Length; line++)
            {
                if (program[line][..4] == "mask")
                {
                    mask = program[line][7..];
                }
                else
                {
                    var rightBracketId = program[line].IndexOf("]");
                    // read id of memory which is between brackets
                    var memoryId = uint.Parse(program[line][4..rightBracketId]);
                    var value = uint.Parse(program[line][(rightBracketId + 4)..]);
                    //                                  convert uint value to bin string
                    memory[memoryId] = ApplyMask(mask, Convert.ToString(value, 2).ToCharArray());
                }
            }

            return memory.Sum(x => x.Value);
        }

        private static long ApplyMask(string mask, char[] value)
        {
            var newValue = new char[mask.Length];
            // on given place
            for (int i = mask.Length - 1, j = value.Length - 1; i >= 0; i--, j--)
            {
                // when mask contain 'X'
                if (mask[i] == 'X')
                {
                    // write value
                    if (j >= 0)
                        newValue[i] = value[j];
                    // otherwise fill with '0'
                    else
                        newValue[i] = '0';
                }
                // when mask doesn't contain 'X'
                // write this value
                else
                {
                    newValue[i] = mask[i];
                }
            }
            // from binary string => long value
            return Convert.ToInt64(new string(newValue), 2);
        }
    }
}