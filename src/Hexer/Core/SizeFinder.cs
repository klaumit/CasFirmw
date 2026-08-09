using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hexer.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using D = System.IO.Directory;

namespace Hexer.Core
{
    public static class SizeFinder
    {
        private static IEnumerable<string> ListBinFiles(string inputDir)
        {
            var files = FileExt.FindFiles(inputDir);
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

            var binFiles = ListBinFiles(inputDir);
            foreach (var file in binFiles.OrderBy(x => x))
            {
                var local = FileExt.GetLocal(file, inputDir);
                Console.WriteLine($" * {local}");


            }

            Console.WriteLine("Done.");
        }
    }
}