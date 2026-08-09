using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Hexer.Tools;

namespace Hexer.Core
{
    public static class SizeFinder
    {
        public static void Run(Options o)
        {
            var inputDir = Path.GetFullPath(o.Input!);
            var outputFile = Path.GetFullPath(o.Output!);
            Console.WriteLine("Reading binary files, finding sizes...");



            Console.WriteLine("Done.");
        }
    }
}