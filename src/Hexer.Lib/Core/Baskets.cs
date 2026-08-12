using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Hexer.Compat;
using Hexer.Tools;

namespace Hexer.Core
{
    public static class Baskets
    {
        public static IEnumerable<BskLine> Read(string file)
        {
            var ext = Path.GetExtension(file).ToLowerInvariant();
            switch (ext)
            {
                case ".xxd": return ReadXxd(file);
                case ".bin": return ReadBin(file);
                default: throw new InvalidOperationException(ext);
            }
        }

        private static IEnumerable<BskLine> ReadBin(string file)
        {
            using var stream = File.OpenRead(file);
            long adr = 0;
            while (stream.ReadSome(16) is { } bytes)
            {
                yield return new BskLine((uint)adr, bytes);
                adr += bytes.Length;
            }
        }

        private static IEnumerable<BskLine> ReadXxd(string file)
        {
            using var reader = File.OpenText(file);
            while (reader.ReadLine() is { } line)
            {
                var tmp = NetFx.Split(line, ':', 2);
                var adr = uint.Parse(tmp[0], NumberStyles.HexNumber);
                var two = NetFx.Split(tmp[1], "  ", 2);
                var hex = two[0].Replace(" ", "").Trim();
                var bytes = NetFx.FromHexString(hex);
                yield return new BskLine(adr, bytes);
            }
        }
    }
}