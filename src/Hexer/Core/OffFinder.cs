using System;
using System.IO;

namespace Hexer.Core
{
    public static class OffFinder
    {
        public static void Run(Options o)
        {
            var inputFile = Path.GetFullPath(o.Input!);
            var outputFile = Path.GetFullPath(o.Output!);
            Console.WriteLine("Reading binary files, finding offsets...");

            Console.WriteLine("Done.");
        }
    }
}