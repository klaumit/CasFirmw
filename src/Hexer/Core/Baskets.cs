using System;
using System.Globalization;
using System.IO;

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
            using var reader = File.OpenRead(file);
            var i = 0;
            var adr = 0;
            var bytes = new byte[16];
            while (reader.Read(bytes) is var got and >= 1)
            {
                if (i++ >= 3) break;
                if (bytes.Length != got)
                    Array.Resize(ref bytes, got);
                Console.WriteLine($" {bytes.Length} | {adr:x8} ");
                adr += got;
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