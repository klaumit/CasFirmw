using System;
using System.Collections.Generic;
using System.IO;
using Hexer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Hexer.Core
{
    public static class BinExtract
    {
        public static void Run(Options o)
        {
            var inputDir = Path.GetFullPath(o.Input!);
            var included = Path.GetFullPath(o.Include!);
            var outputFile = Path.GetFullPath(o.Output!);
            Console.WriteLine($" Input => {inputDir}");
            Console.WriteLine($" Extra => {included}");
            Console.WriteLine($"Output => {outputFile}");
            Console.WriteLine("Extracting binary blobs...");

            var fileSizes = JsonExt.Read<SortedDictionary<int, string>>(included);
            var results = new SortedDictionary<int, SortedSet<string>>();

            Console.WriteLine(JsonConvert.SerializeObject(fileSizes));

            JsonExt.Write(outputFile, results);
            Console.WriteLine("Done.");
        }
    }
}