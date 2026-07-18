using System.Collections.Generic;

namespace Hexer.Core
{
    public static class HexReader
    {
        public static IEnumerable<HexLine> Read(this IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                var tmp = line.Split(':', 2);
                var adr = tmp[0].Trim();
                tmp = tmp[1].Split("  ", 2);
                var hex = tmp[0].Trim();
                var txt = tmp[1].Trim();
                yield return new HexLine(adr, hex, txt);
            }
        }

        public static IEnumerable<HexByte> Read(this IEnumerable<HexLine> lines)
        {
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line.Hex))
                    continue;
                var off = line.Off;
                for (var i = 0; i < line.Raw.Length; i++)
                {
                    var bit = line.Raw[i];
                    yield return new HexByte($"{off + i:x8}", $"{bit:x2}");
                }
            }
        }
    }
}