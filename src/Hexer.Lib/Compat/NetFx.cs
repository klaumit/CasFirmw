using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Hexer.Compat
{
    public static class NetFx
    {
        public static string[] Split(string line, string sep, int count)
        {
            if (line == null) throw new ArgumentNullException(nameof(line));
            if (sep == null) throw new ArgumentNullException(nameof(sep));
            return line.Split([sep], count, StringSplitOptions.None);
        }

        public static string[] Split(string line, char sep, int count)
        {
            if (line == null) throw new ArgumentNullException(nameof(line));
            return line.Split([sep], count);
        }

        public static byte[] FromHexString(string txt)
        {
            if (txt == null)
                throw new ArgumentNullException(nameof(txt));
            if (txt.Length % 2 != 0)
                throw new FormatException("Hex must have an even number of chars!");
            var result = new byte[txt.Length / 2];
            for (var idx = 0; idx < result.Length; idx++)
            {
                var item = txt.Substring(idx * 2, 2);
                result[idx] = byte.Parse(item, NumberStyles.HexNumber);
            }
            return result;
        }

        public static string ToHexStringLower(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            var bld = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
                bld.AppendFormat("{0:x2}", b);
            return bld.ToString();
        }

        public static IEnumerable<T[]> ChunkX<T>(this IEnumerable<T> items, int size)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));
            var buffer = new T[size];
            var count = 0;
            foreach (var item in items)
            {
                buffer[count++] = item;
                if (count == size)
                {
                    yield return buffer;
                    buffer = new T[size];
                    count = 0;
                }
            }
            if (count > 0)
            {
                var last = new T[count];
                Array.Copy(buffer, last, count);
                yield return last;
            }
        }
    }
}