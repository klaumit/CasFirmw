using System;
using System.IO;
using System.Linq;

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
            var buffer = new byte[16];
            while (reader.Read(buffer) is var got and >= 1)
            {
                if (i++ >= 3) break;
                var hex = string.Join(" ",
                    buffer.Take(got).Chunk(2).Select(Convert.ToHexStringLower)
                );
                Console.WriteLine($"{adr:x8}: {hex}");
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
                Console.WriteLine(line);
            }
        }
    }
}