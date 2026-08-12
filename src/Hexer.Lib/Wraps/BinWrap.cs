using System.Collections.Generic;
using Hexer.Core;
using System;
using System.IO;
using Hexer.Tools;
using F = Hexer.Core.Found;
using AI = Hexer.Core.AppInfo;

namespace Hexer.Wraps
{
    public static class BinWrap
    {
        public static void ExtractFiles(AI ai, long pvaSize, long elfSize, byte[] array, F anchor, string dir)
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

        public static IEnumerable<BskLine[]> SplitSections(IEnumerable<BskLine> lines)
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
    }
}