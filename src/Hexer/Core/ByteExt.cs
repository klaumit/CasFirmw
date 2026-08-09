using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Hexer.Core
{
    public static class ByteExt
    {
        public static IEnumerable<int> IndicesOf(this byte[] haystack, byte[] pattern)
        {
            var start = 0;
            int idx;
            while ((idx = haystack.IndexOf(pattern, start)) >= 0)
            {
                yield return idx;
                start = idx + pattern.Length;
            }
        }

        public static int IndexOf(this byte[] haystack, byte[] pattern, int startIndex = 0)
        {
            if (pattern == null || pattern.Length == 0)
                return -1;
            if (haystack == null || haystack.Length < pattern.Length)
                return -1;

            var end = haystack.Length - pattern.Length;
            for (var i = startIndex; i <= end; i++)
            {
                var match = true;
                for (var j = 0; j < pattern.Length; j++)
                {
                    if (haystack[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                    return i;
            }
            return -1;
        }

        public static string ToHex(this byte?[] bytes)
        {
            var bld = new StringBuilder();
            foreach (var bit in bytes)
            {
                if (bit == null)
                    bld.Append($"__");
                else
                    bld.Append($"{bit:X2}");
            }
            return bld.ToString();
        }

        public static byte?[] ToBytes(this IDictionary<int, ISet<byte>> dict)
        {
            var max = dict.Count == 0 ? 0 : dict.Keys.Max() + 1;
            var array = new byte?[max];
            for (var i = 0; i < array.Length; i++)
            {
                var bits = dict[i];
                byte? bit = bits.Count == 1 ? bits.Single() : null;
                array[i] = bit;
            }
            return array;
        }

        public static void WriteTo(this IDictionary<int, ISet<byte>> dict, byte[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var bit = array[i];
                if (!dict.TryGetValue(i, out var set))
                    dict[i] = set = new SortedSet<byte>();
                set.Add(bit);
            }
        }
    }
}