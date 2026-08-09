using System;
using System.Collections.Generic;
using System.IO;
using Hexer.Tools;

namespace Hexer.Core
{
    public static class BinExtract
    {
        public static void Run(Options o)
        {
            var inputDir = Path.GetFullPath(o.Input!);
            var outputFile = Path.GetFullPath(o.Output!);
            Console.WriteLine($" Input => {inputDir}");
            Console.WriteLine($"Output => {outputFile}");
            Console.WriteLine("Extracting binary blobs...");

            var results = new SortedDictionary<int, SortedSet<string>>();

            JsonExt.Write(outputFile, results);
            Console.WriteLine("Done.");
        }
    }
}