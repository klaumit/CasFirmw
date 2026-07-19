using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hexer.Core
{
    public static class OffFinder
    {
        private static IEnumerable<HexByte> ReadFile(string file)
        {
            var enc = Encoding.UTF8;
            var lines = File.ReadLines(file, enc);
            return lines.Read().Read();
        }

        private static IEnumerable<uint> FindBytes(string file, byte[] mask)
        {
            var i = 0;
            uint start = 0;
            foreach (var line in ReadFile(file))
            {
                if (line.Raw == mask[i])
                {
                    if (i == 0)
                        start = line.Off;
                    i++;
                    if (i >= mask.Length)
                    {
                        yield return start;
                        i = 0;
                    }
                }
                else
                {
                    i = 0;
                }
            }
        }

        private static string ToHex(this IEnumerable<uint> items)
        {
            return string.Join(", ", items.Select(i => i.ToString("x8")));
        }

        public static void Run(Options o)
        {
            var inputDir = Path.GetFullPath(o.Input!);
            var outputFile = Path.GetFullPath(o.Output!);
            Console.WriteLine("Reading binary files, finding offsets...");

            const SearchOption so = SearchOption.AllDirectories;
            var files = Directory.EnumerateFiles(inputDir, "*.xxd", so);
            foreach (var file in files.Take(1))
            {
                Console.WriteLine($" * {file}");

                var binIdx = FindBytes(file, "CASIOPVOS200U"u8.ToArray()).ToHex();
                Console.WriteLine($"    => BIN: {binIdx}");

                var aplIdx = FindBytes(file, "PVOSAPL"u8.ToArray()).ToHex();
                Console.WriteLine($"    => APL: {aplIdx}");

                var pvaIdx = FindBytes(file, "PVAPLHEDV20"u8.ToArray()).ToHex();
                Console.WriteLine($"    => PVA: {pvaIdx}");

                var rldIdx = FindBytes(file, [0x7f, 0x45, 0x4c, 0x46, 0x01, 0x02, 0x01, 0x00]).ToHex();
                Console.WriteLine($"    => RLD: {rldIdx}");
            }

            Console.WriteLine("Done.");
        }
    }
}