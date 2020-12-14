using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020.Day14
{
    public static class PartTwo
    {
        public static ulong Solve()
        {
            using var sr = new StreamReader("Day14/data.txt");
            var program = sr.ReadToEnd().Split("\n").ToArray();
            var memory = new Dictionary<ulong, ulong>();
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
                    var id = uint.Parse(program[line][4..rightBracketId]);
                    var val = uint.Parse(program[line][(rightBracketId + 4)..]);

                    // convert uint value to bin string
                    var idStr = Convert.ToString(id, 2).ToCharArray();
                    foreach (var memoryId in ApplyMask(mask, idStr))
                        memory[memoryId] = val;
                }
            }

            return Sum(memory.Select(x => x.Value));
        }

        private static List<ulong> ApplyMask(string mask, char[] id)
        {
            var newValue = new char[mask.Length];
            // on given place
            for (int i = mask.Length - 1, j = id.Length - 1; i >= 0; i--, j--)
            {
                // when mask[i] contain '0'
                if (mask[i] == '0')
                {
                    // try copy value from id
                    if (j >= 0)
                        newValue[i] = id[j];
                    // otherwise write '0'
                    else
                        newValue[i] = '0';
                }
                else
                {
                    newValue[i] = mask[i];
                }
            }

            var memoryIds = new List<ulong>();

            // replace every 'X'  with '0' and '1'
            foreach (var item in FillFloatingBits(newValue))
            {
                //              from binary string => long value
                memoryIds.Add(Convert.ToUInt64(new string(item), 2));
            }

            return memoryIds;
        }

        private static List<char[]> FillFloatingBits(char[] id)
        => FillFloatingBits(new List<char[]>() { id });

        private static List<char[]> FillFloatingBits(List<char[]> ids)
        {
            var idOfX = Array.IndexOf(ids[0], 'X');

            // return when there is no 'X' in array
            if (idOfX == -1)
                return ids;

            var copy = new List<char[]>();

            foreach (var item in ids)
            {
                copy.Add((char[])item.Clone());
            }

            // replace first left 'X' with '1' and '0'
            for (var i = 0; i < ids.Count; i++)
            {
                ids[i][idOfX] = '0';
                copy[i][idOfX] = '1';
            }

            // join two lists
            ids.AddRange(copy);

            return FillFloatingBits(ids);
        }

        private static ulong Sum(IEnumerable<ulong> memory)
        {
            ulong sum = 0;
            foreach (var item in memory)
            {
                sum += item;
            }
            return sum;
        }
    }
}