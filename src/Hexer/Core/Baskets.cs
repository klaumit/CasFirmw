using System;
using System.Globalization;
using System.IO;
using Hexer.Tools;

namespace Hexer.Core
{
    public static class Baskets
    {
        public static void Read(string file)
        {
            var ext = Path.GetExtension(file).ToLowerInvariant();
            switch (ext)
            {
                case ".xxd": ReadXxd(file); break;
                case ".bin": ReadBin(file); break;
                default: throw new InvalidOperationException(ext);
            }
        }

        private static void ReadBin(string file)
        {
            using var stream = File.OpenRead(file);
            var i = 0;
            var adr = 0;
            while (stream.ReadSome(16) is { } bytes)
            {
                if (i++ >= 3) break;
                Console.WriteLine($" {bytes.Length} | {adr:x8} ");
                adr += bytes.Length;
            }
        }

        private static void ReadXxd(string file)
        {
            using var reader = File.OpenText(file);
            var i = 0;
            while (reader.ReadLine() is { } line)
            {
                if (i++ >= 3) break;
                var tmp = line.Split(':', 2);
                var adr = uint.Parse(tmp[0], NumberStyles.HexNumber);
                var two = tmp[1].Split("  ", 2);
                var hex = two[0].Replace(" ", "").Trim();
                var bytes = Convert.FromHexString(hex);
                Console.WriteLine($" {bytes.Length} | {adr:x8} ");
            }
        }
    }
}