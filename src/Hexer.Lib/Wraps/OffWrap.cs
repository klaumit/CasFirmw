using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Hexer.Core;
using Hexer.Tools;

namespace Hexer.Wraps
{
    public static class OffWrap
    {
        private static IEnumerable<HexByte> ReadFile(string file)
        {
            var enc = Encoding.UTF8;
            var lines = File.ReadLines(file, enc);
            return lines.Read().Read();
        }

        public static IEnumerable<uint> FindBytes(string file, byte[] mask)
        {
            var i = 0;
            uint start = 0;
            foreach (var line in ReadFile(file))
            {
                if (line.Raw == mask[i])
                {
                    if (i == 0)
                        start = line.Off;
                    i++;
                    if (i >= mask.Length)
                    {
                        yield return start;
                        i = 0;
                    }
                }
                else
                {
                    i = 0;
                }
            }
        }

        public static string[] ToHex(this IEnumerable<uint> items)
        {
            return items.Select(i => i.ToString("x8")).ToArray();
        }

        public static string ToStr(this IEnumerable<string> items)
        {
            return string.Join(", ", items);
        }

        public static void FindDiffs(IDictionary<string, string> dict)
        {
            var lastKey = "0";
            foreach (var pair in dict)
            {
                var key = pair.Key;
                var val = pair.Value;
                var k1 = TextExt.ParseUInt(key);
                var k2 = TextExt.ParseUInt(lastKey);
                var len = k1 - k2;
                dict[key] = $"{val}|+{len}";
                lastKey = key;
            }
        }
    }
}