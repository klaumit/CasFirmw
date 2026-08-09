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
            var results = new SortedDictionary<int, SortedSet<string>>();
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
                foreach (var anchor in anchors)
                {
                    var (pvaSize, elfSize) = anchor.GetSizes(array);
                    fileSizes.TryGetValue((int)pvaSize, out var pvaName);
                    Console.WriteLine($"    * {anchor,-37} --> {elfSize:D6} --> {pvaSize:D6} '{pvaName}'");
                }
            }

            JsonExt.Write(outputFile, results);
            Console.WriteLine("Done.");
        }
    }
}