using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Hexer.Core
{
    public static class AntiNuller
    {
        public static void Run(Options o)
        {
            var inputFile = Path.GetFullPath(o.Input!);

            var localName = Path.GetFileName(inputFile);
            Console.WriteLine(" * " + localName);

            var enc = Encoding.UTF8;
            var lines = File.ReadLines(inputFile, enc);
            foreach (var line in lines.Take(10))
            {
                Console.WriteLine("'" + line + "'");
            }

            Console.WriteLine("Done.");
        }
    }
}