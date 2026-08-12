using System.Collections.Generic;

namespace Hexer.Core
{
    public sealed class BinInfo
    {
        public int Size { get; set; }
        public SortedDictionary<int, AppInfo>? Apps { get; set; }
    }
}