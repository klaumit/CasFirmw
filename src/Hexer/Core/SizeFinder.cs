using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ByteSizeLib;
using Hexer.Tools;

namespace Hexer.Core
{
    public static class SizeFinder
    {
        private static IEnumerable<string> ListBinFiles(string folder)
        {
            var files = FileExt.FindFiles(folder);
            files.TryGetValue(".pva", out var pvaFiles);
            files.TryGetValue(".sys", out var sysFiles);
            return sysFiles?.Concat(pvaFiles ?? []) ?? [];
        }

        public static void Run(Options o)
        {
            var inputDir = Path.GetFullPath(o.Input!);
            var outputFile = Path.GetFullPath(o.Output!);
            Console.WriteLine($" Input => {inputDir}");
            Console.WriteLine($"Output => {outputFile}");
            Console.WriteLine("Reading binary files, finding sizes...");

            var fileSizes = new SortedDictionary<int, SortedSet<string>>();
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
                Console.WriteLine($" * {local,-26} {hSize,9}");
                if (anchors.Length != 1)
                    continue;
                var fSize = array.Length;
                if (!fileSizes.TryGetValue(fSize, out var set))
                    fileSizes[fSize] = set = new SortedSet<string>();
                var val = Path.GetFileName(file);
                if (val.StartsWith("00"))
                    continue;
                set.Add(val);
            }

            JsonExt.Write(outputFile, fileSizes.ToDictionary(k =>
                k.Key, v => v.Value.Single()));
            Console.WriteLine("Done.");
        }
    }
}