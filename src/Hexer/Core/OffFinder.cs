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

                var numIdx = FindBytes(file, [0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0xf1])
                    .ToHex();
                Console.WriteLine($"    => NUM: {numIdx.ToStr()}");

                var extIdx = FindBytes(file, ".PVA\0"u8.ToArray()).ToHex();
                Console.WriteLine($"    => EXT: {extIdx.ToStr()}");

                var bepIdx = FindBytes(file, "<<  BEEP  >>"u8.ToArray()).ToHex();
                Console.WriteLine($"    => BEP: {bepIdx.ToStr()}");

                var bifIdx = FindBytes(file, "_binaryinfo"u8.ToArray()).ToHex();
                Console.WriteLine($"    => BIF: {bifIdx.ToStr()}");

                var bsrIdx = FindBytes(file, "BSrl"u8.ToArray()).ToHex();
                Console.WriteLine($"    => BSR: {bsrIdx.ToStr()}");

                var busIdx = FindBytes(file, "BUsb"u8.ToArray()).ToHex();
                Console.WriteLine($"    => BUS: {busIdx.ToStr()}");

                var bnaIdx = FindBytes(file, "BUSINESS NAVIGATOR"u8.ToArray()).ToHex();
                Console.WriteLine($"    => BNA: {bnaIdx.ToStr()}");

                var cpfIdx = FindBytes(file, "CASIOPVFILE"u8.ToArray()).ToHex();
                Console.WriteLine($"    => CPF: {cpfIdx.ToStr()}");

                var cpiIdx = FindBytes(file, "CASIO PV Internal"u8.ToArray()).ToHex();
                Console.WriteLine($"    => CPI: {cpiIdx.ToStr()}");

                var cssIdx = FindBytes(file, "CASIOSUBSYS"u8.ToArray()).ToHex();
                Console.WriteLine($"    => CSS: {cssIdx.ToStr()}");

                var pvsIdx = FindBytes(file, "PV-S1600"u8.ToArray()).ToHex();
                Console.WriteLine($"    => PVS: {pvsIdx.ToStr()}");

                var vldIdx = FindBytes(file, "VALIDAREA"u8.ToArray()).ToHex();
                Console.WriteLine($"    => VLD: {vldIdx.ToStr()}");

                var z00Idx = FindBytes(file, "Z000ExtKeyDriver"u8.ToArray()).ToHex();
                Console.WriteLine($"    => Z00: {z00Idx.ToStr()}");

                var key = Path.GetFileNameWithoutExtension(file);
                var vals = binIdx.Select((x, i) => (a: x, b: $"bin_{i + 1:D2}"))
                    .Concat(aplIdx.Select((x, i) => (a: x, b: $"apl_{i + 1:D2}")))
                    .Concat(pvaIdx.Select((x, i) => (a: x, b: $"pva_{i + 0:D2}")))
                    .Concat(rldIdx.Select((x, i) => (a: x, b: $"rld_{i + 1:D2}")))
                    .Concat(numIdx.Select((x, i) => (a: x, b: $"num_{i + 1:D2}")))
                    .Concat(extIdx.Select((x, i) => (a: x, b: $"ext_{i + 1:D2}")))
                    .Concat(bepIdx.Select((x, i) => (a: x, b: $"bep_{i + 1:D2}")))
                    .Concat(bifIdx.Select((x, i) => (a: x, b: $"bif_{i + 1:D2}")))
                    .Concat(bsrIdx.Select((x, i) => (a: x, b: $"bsr_{i + 1:D2}")))
                    .Concat(busIdx.Select((x, i) => (a: x, b: $"bus_{i + 1:D2}")))
                    .Concat(bnaIdx.Select((x, i) => (a: x, b: $"bna_{i + 1:D2}")))
                    .Concat(cpfIdx.Select((x, i) => (a: x, b: $"cpf_{i + 1:D2}")))
                    .Concat(cpiIdx.Select((x, i) => (a: x, b: $"cpi_{i + 1:D2}")))
                    .Concat(cssIdx.Select((x, i) => (a: x, b: $"css_{i + 1:D2}")))
                    .Concat(pvsIdx.Select((x, i) => (a: x, b: $"pvs_{i + 1:D2}")))
                    .Concat(vldIdx.Select((x, i) => (a: x, b: $"vld_{i + 1:D2}")))
                    .Concat(z00Idx.Select((x, i) => (a: x, b: $"z00_{i + 1:D2}")))
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