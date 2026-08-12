using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ByteSizeLib;
using Hexer.Config;
using Hexer.Core;
using Hexer.Tools;
using AI = Hexer.Core.AppInfo;
using static Hexer.Wraps.BinWrap;

namespace Hexer.Actions
{
    public static class BinExtract
    {
        private static IEnumerable<string> ListBinFiles(string folder)
        {
            var files = FileExt.FindFiles(folder);
            files.TryGetValue(".bin", out var binFiles);
            files.TryGetValue(".xxd", out var xxdFiles);
            return binFiles?.Concat(xxdFiles ?? []) ?? [];
        }

        public static void Run(Options o)
        {
            var inputDir = Path.GetFullPath(o.Input!);
            var included = Path.GetFullPath(o.Include!);
            var outputFile = Path.GetFullPath(o.Output!);
            var outputDir = Path.GetDirectoryName(outputFile)!;
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
                var fi = new FileInfo(file);
                var hSize = ByteSize.FromBytes(fi.Length);
                var local = FileExt.GetLocal(file, inputDir);
                Console.WriteLine($" * {local,-28} {hSize,9}");
                var lines = Baskets.Read(file);
                var sections = SplitSections(lines);
                var obj = new SortedDictionary<int, AI>();
                foreach (var section in sections)
                {
                    var off = section[0].Adr;
                    var array = section.SelectMany(s => s.Raw).ToArray();
                    var pvaIdx = array.IndicesOf(pvaBytes).ToArray();
                    var rldIdx = array.IndicesOf(rldBytes).ToArray();
                    if (!(pvaIdx.Length >= 1 && rldIdx.Length >= 1))
                        continue;
                    var anchors = ElfExt.FindAnchors(pvaIdx, rldIdx).ToArray();
                    if (anchors.Length < 1)
                        continue;
                    var localName = Path.GetFileNameWithoutExtension(local);
                    var localDir = FileExt.CreateDir(Path.Combine(outputDir, localName));
                    var aSize = ByteSize.FromBytes(array.Length);
                    Console.WriteLine($"   * {off:x8} ({aSize})");
                    foreach (var anchor in anchors)
                    {
                        var es = anchor.GetSizes(array);
                        var (pvaSize, elfSize) = (es.pvaSize, es.elfSize);
                        fileSizes.TryGetValue((int)pvaSize, out var pvaName);
                        Console.WriteLine($"     * {anchor,-37} --> {elfSize:D6} --> {pvaSize:D6} '{pvaName}'");
                        var ai = new AI
                        {
                            Offset = (int)(off + anchor.P), Header = anchor.D, Size = (int)pvaSize, Name = pvaName
                        };
                        obj[anchor.I] = ai;
                        ExtractFiles(ai, es, array, anchor, localDir, true);
                    }
                }
                results[local] = new BinInfo { Size = (int)fi.Length, Apps = obj };
            }

            JsonExt.Write(outputFile, results);
            Console.WriteLine("Done.");
        }
    }
}