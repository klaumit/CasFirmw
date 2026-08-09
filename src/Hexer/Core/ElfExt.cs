using System.Collections.Generic;
using System.Linq;

namespace Hexer.Core
{
    public static class ElfExt
    {
        public sealed class Found
        {
            public int I { get; set; }
            public int P { get; set; }
            public int R { get; set; }
            public int D { get; set; }
            public int? N { get; set; }

            public override string ToString()
            {
                return $"#{I:D2}|P={P:D7}|R={R:D7}|D={D:x3}|N={N:x5}".TrimEnd('=', 'N', '|');
            }
        }

        public static IEnumerable<Found> FindAnchors(int[] pvaIdArray, int[] rldIdArray)
        {
            var allowedDiffs = new[] { 168, 336, 416 };
            Found? last = null;
            var i = 0;
            foreach (var pIdx in pvaIdArray)
            {
                var rIdx = rldIdArray.FirstOrDefault(r => r > pIdx);
                var diff = rIdx - pIdx;
                if (!allowedDiffs.Contains(diff))
                    continue;
                var next = pIdx - last?.P;
                var found = new Found { I = ++i, P = pIdx, R = rIdx, D = diff, N = next };
                yield return found;
                last = found;
            }
        }

        private static uint ReadU32Be(byte[] data, int elfStart, int fieldOffset)
        {
            var p = elfStart + fieldOffset;
            return (uint)(data[p] << 24 | data[p + 1] << 16 | data[p + 2] << 8 | data[p + 3]);
        }

        private static ushort ReadU16Be(byte[] data, int elfStart, int fieldOffset)
        {
            var p = elfStart + fieldOffset;
            return (ushort)(data[p] << 8 | data[p + 1]);
        }

        public static long GetElfFileSize(byte[] data, int elfStart)
        {
            var elfShOff = ReadU32Be(data, elfStart, 32);
            var elfShEntSize = ReadU16Be(data, elfStart, 46);
            var elfShNum = ReadU16Be(data, elfStart, 48);

            var totalSize = elfShOff + (long)elfShEntSize * elfShNum;
            return totalSize;
        }

        public static (long pvaSize, long elfSize) GetSizes(this Found anchor, byte[] array)
        {
            var elfSize = GetElfFileSize(array, anchor.R);
            var pvaSize = anchor.D + elfSize;
            return (pvaSize, elfSize);
        }
    }
}