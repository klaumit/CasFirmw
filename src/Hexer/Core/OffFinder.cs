using System;
using System.IO;

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
            }

            Console.WriteLine("Done.");
        }
    }
}