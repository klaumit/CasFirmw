using System;
using System.IO;
using System.Text;

namespace Hexer.Core
{
    public static class AntiNuller
    {
        public static void Run(Options o)
        {
            var inputFile = Path.GetFullPath(o.Input!);
            var outputFile = Path.GetFullPath(o.Output!);
            Console.WriteLine("Writing binary files, ignoring offsets...");

            var localName = Path.GetFileName(inputFile);
            Console.WriteLine($" * {localName}");
            Console.WriteLine("    => " + outputFile);

            var enc = Encoding.UTF8;
            var lines = File.ReadLines(inputFile, enc);
            using var off = File.Create(outputFile);
            long count = 0;
            foreach (var line in HexReader.Read(lines))
            {
                var bytes = line.Raw;
                // Console.WriteLine($"        {line.Adr} ({line.Off}) --> {bytes.Length} bytes");
                off.Write(bytes);
                count += bytes.Length;
            }

            var cTxt = ByteSizeLib.ByteSize.FromBytes(count);
            Console.WriteLine($"    --> {cTxt} written!");
            off.Flush();
            Console.WriteLine("Done.");
        }
    }
}