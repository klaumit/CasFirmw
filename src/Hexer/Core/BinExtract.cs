using System;
using System.Collections.Generic;
using System.IO;
using Hexer.Tools;
using System.Linq;
using ByteSizeLib;

namespace Hexer.Core
{
    public static class BinExtract
    {
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
            public SortedDictionary<int, AppInfo>? Apps { get; set; }
        }

        private static IEnumerable<string> ListBinFiles(string folder)
        {
            var files = FileExt.FindFiles(folder);
            files.TryGetValue(".bin", out var binFiles);
            return binFiles ?? [];
        }

        public static void Run(Options o)
        {
            var inputDir = Path.GetFullPath(o.Input!);
            var included = Path.GetFullPath(o.Include!);
            var outputFile = Path.GetFullPath(o.Output!);
            Console.WriteLine($" Input => {inputDir}");
            Console.WriteLine($" Extra => {included}");
            Console.WriteLine($"Output => {outputFile}");
            Console.WriteLine("Extracting binary blobs...");

            var fileSizes = JsonExt.Read<SortedDictionary<int, string>>(included)!;
            var results = new SortedDictionary<string, BinInfo>();
            var binFiles = ListBinFiles(inputDir);
            var pvaBytes = Consts.PvaMarkB;
            var rldBytes = Consts.RldMarkB;
            foreach (var file in binFiles.OrderBy(x => x))
            {
                var array = File.ReadAllBytes(file);
                var pvaIdx = array.IndicesOf(pvaBytes).ToArray();
                var rldIdx = array.IndicesOf(rldBytes).ToArray();
                if (!(pvaIdx.Length >= 1 && rldIdx.Length >= 1))
                    continue;
                var local = FileExt.GetLocal(file, inputDir);
                var anchors = ElfExt.FindAnchors(pvaIdx, rldIdx).ToArray();
                if (anchors.Length < 1)
                    continue;
                var hSize = ByteSize.FromBytes(array.Length);
                Console.WriteLine($" * {local,-27} {hSize,9}");
                var obj = new SortedDictionary<int, AppInfo>();
                foreach (var anchor in anchors)
                {
                    var (pvaSize, elfSize) = anchor.GetSizes(array);
                    fileSizes.TryGetValue((int)pvaSize, out var pvaName);
                    Console.WriteLine($"    * {anchor,-37} --> {elfSize:D6} --> {pvaSize:D6} '{pvaName}'");
                    var ai = new AppInfo
                    {
                        Offset = anchor.P, Header = anchor.D, Size = (int)pvaSize, Name = pvaName
                    };
                    obj[anchor.I] = ai;
                }
                results[local] = new BinInfo { Size = array.Length, Apps = obj };
            }

            JsonExt.Write(outputFile, results);
            Console.WriteLine("Done.");
        }
    }
}