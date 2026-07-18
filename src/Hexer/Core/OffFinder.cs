using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Hexer.Core
{
    public static class OffFinder
    {
        public static void Run(Options o)
        {
            var inputDir = Path.GetFullPath(o.Input!);
            var outputFile = Path.GetFullPath(o.Output!);
            Console.WriteLine("Reading binary files, finding offsets...");

            const SearchOption so = SearchOption.AllDirectories;
            var files = Directory.EnumerateFiles(inputDir, "*.xxd", so);
            foreach (var file in files)
            {
                Console.WriteLine($" * {file}");

                var enc = Encoding.UTF8;
                var lines = File.ReadLines(file, enc);
                foreach (var line in HexReader.Read(lines))
                {
                    if (string.IsNullOrWhiteSpace(line.Hex))
                        continue;
                    if (line.Txt.Contains("CASIOPVOS200U") || line.Txt.Contains("PVOSAPL"))
                    {
                        Console.WriteLine($" {line}");
                    }
                }
            }

            Console.WriteLine("Done.");
        }
    }
}