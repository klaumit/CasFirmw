using System;
using System.Collections.Generic;
using System.IO;
using Hexer.Tools;
using System.Linq;
using ByteSizeLib;
using F = Hexer.Core.Found;
using AI = Hexer.Core.AppInfo;

namespace Hexer.Core
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
    
    public sealed class AppInfo
    {
        public int Offset { get; set; }
        public int Header { get; set; }
        public int Size { get; set; }
        public string? Name { get; set; }
    }

    public sealed class BinInfo
    {
        public int Size { get; set; }
        public SortedDictionary<int, AI>? Apps { get; set; }
    }
}