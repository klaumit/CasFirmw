using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Hexer.Tools;

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

        private static string[] ToHex(this IEnumerable<uint> items)
        {
            return items.Select(i => i.ToString("x8")).ToArray();
        }

        private static string ToStr(this IEnumerable<string> items)
        {
            return string.Join(", ", items);
        }

        private static void FindDiffs(IDictionary<string, string> dict)
        {
            var lastKey = "0";
            foreach (var (key, val) in dict)
            {
                var k1 = TextExt.ParseUInt(key);
                var k2 = TextExt.ParseUInt(lastKey);
                var len = k1 - k2;
                dict[key] = $"{val}|+{len}";
                lastKey = key;
            }
        }

        public static void Run(Options o)
        {
            var inputDir = Path.GetFullPath(o.Input!);
            var outputFile = Path.GetFullPath(o.Output!);
            Console.WriteLine("Reading binary files, finding offsets...");

            const SearchOption so = SearchOption.AllDirectories;
            var files = Directory.EnumerateFiles(inputDir, "*.xxd", so);
            foreach (var file in files.OrderBy(f => f))
            {
                Console.WriteLine($" * {file}");

                var binIdx = FindBytes(file, "CASIOPVOS200U"u8.ToArray()).ToHex();
                Console.WriteLine($"    => BIN: {binIdx.ToStr()}");

                var aplIdx = FindBytes(file, "PVOSAPL"u8.ToArray()).ToHex();
                Console.WriteLine($"    => APL: {aplIdx.ToStr()}");

                var pvaIdx = FindBytes(file, "PVAPLHEDV20"u8.ToArray()).ToHex();
                Console.WriteLine($"    => PVA: {pvaIdx.ToStr()}");

                var rldIdx = FindBytes(file, [0x7f, 0x45, 0x4c, 0x46, 0x01, 0x02, 0x01, 0x00]).ToHex();
                Console.WriteLine($"    => RLD: {rldIdx.ToStr()}");

                var key = Path.GetFileNameWithoutExtension(file);
                var vals = binIdx.Select((x, i) => (a: x, b: $"bin_{i + 1:D2}"))
                    .Concat(aplIdx.Select((x, i) => (a: x, b: $"apl_{i + 1:D2}")))
                    .Concat(pvaIdx.Select((x, i) => (a: x, b: $"pva_{i + 0:D2}")))
                    .Concat(rldIdx.Select((x, i) => (a: x, b: $"rld_{i + 1:D2}")))
                    .OrderBy(x => x.a)
                    .ToDictionary(k => k.a, v => v.b);

                FindDiffs(vals);
                const string end = ".json";
                if (vals.Count < 1)
                    continue;
                JsonExt.Write($"{outputFile.Replace(end, "")}_{key}{end}", vals);
            }

            Console.WriteLine("Done.");
        }
    }
}