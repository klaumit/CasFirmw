using System.Collections.Generic;

namespace Hexer.Core
{
    public static class HexReader
    {
        public static IEnumerable<HexLine> Read(IEnumerable<string> lines)
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
    }
}