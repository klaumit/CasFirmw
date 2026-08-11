using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ByteSizeLib;
using Hexer.Config;
using Hexer.Core;
using Hexer.Tools;
using F = Hexer.Core.Found;
using AI = Hexer.Core.AppInfo;

namespace Hexer.Actions
{
    public static class BinExtract
    {
        private static void ExtractFiles(AI ai, long pvaSize, long elfSize, byte[] array, F anchor, string dir)
        {
            var name = ai.Name!;
            var exExt = Path.GetExtension(name).Trim('.').ToLower();

            var exDir = FileExt.CreateDir(Path.Combine(dir, exExt));
            var exFile = Path.Combine(exDir, name);
            var pB = new byte[pvaSize];
            Array.Copy(array, anchor.P, pB, 0, pB.Length);
            File.WriteAllBytes(exFile, pB);

            var erDir = FileExt.CreateDir(Path.Combine(dir, "rld"));
            var erName = $"{Path.GetFileNameWithoutExtension(name)}.RLD";
            var erFile = Path.Combine(erDir, erName);
            var pR = new byte[elfSize];
            Array.Copy(array, anchor.R, pR, 0, pR.Length);
            File.WriteAllBytes(erFile, pR);
        }

        private static IEnumerable<string> ListBinFiles(string folder)
        {
            var files = FileExt.FindFiles(folder);
            files.TryGetValue(".bin", out var binFiles);
            files.TryGetValue(".xxd", out var xxdFiles);
            return binFiles?.Concat(xxdFiles ?? []) ?? [];
        }

        private static IEnumerable<BskLine[]> SplitSections(IEnumerable<BskLine> lines)
        {
            var list = new List<BskLine>();
            uint? lastIdx = null;
            foreach (var line in lines)
            {
                var diff = line.Adr - lastIdx;
                if (diff == 0)
                {
                    // Doubled address!
                }
                else if (diff is null or 16)
                {
                    // Sequential
                    list.Add(line);
                }
                else
                {
                    // New section!
                    if (list.Count >= 1) yield return list.ToArray();
                    list.Clear();
                    list.Add(line);
                }
                lastIdx = line.Adr;
            }
            if (list.Count >= 1) yield return list.ToArray();
            list.Clear();
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
                var array = File.ReadAllBytes(file);
                var pvaIdx = array.IndicesOf(pvaBytes).ToArray();
                var rldIdx = array.IndicesOf(rldBytes).ToArray();
                if (!(pvaIdx.Length >= 1 && rldIdx.Length >= 1))
                    continue;
                var local = FileExt.GetLocal(file, inputDir);
                var anchors = ElfExt.FindAnchors(pvaIdx, rldIdx).ToArray();
                if (anchors.Length < 1)
                    continue;
                var localName = Path.GetFileNameWithoutExtension(local);
                var localDir = FileExt.CreateDir(Path.Combine(outputDir, localName));
                var hSize = ByteSize.FromBytes(array.Length);
                Console.WriteLine($" * {local,-27} {hSize,9}");
                var obj = new SortedDictionary<int, AI>();
                foreach (var anchor in anchors)
                {
                    var (pvaSize, elfSize) = anchor.GetSizes(array);
                    fileSizes.TryGetValue((int)pvaSize, out var pvaName);
                    Console.WriteLine($"    * {anchor,-37} --> {elfSize:D6} --> {pvaSize:D6} '{pvaName}'");
                    var ai = new AI
                    {
                        Offset = anchor.P, Header = anchor.D, Size = (int)pvaSize, Name = pvaName
                    };
                    obj[anchor.I] = ai;
                    ExtractFiles(ai, pvaSize, elfSize, array, anchor, localDir);
                }
                results[local] = new BinInfo { Size = array.Length, Apps = obj };
            }

            JsonExt.Write(outputFile, results);
            Console.WriteLine("Done.");
        }
    }
}