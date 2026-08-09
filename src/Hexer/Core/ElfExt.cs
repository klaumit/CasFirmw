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
    }
}