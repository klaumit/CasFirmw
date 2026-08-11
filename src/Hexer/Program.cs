using CommandLine;
using Hexer.Actions;
using Hexer.Config;

namespace Hexer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var parser = Parser.Default;
            parser.ParseArguments<Options>(args).WithParsed(o =>
            {
                if (o.FindAddr)
                {
                    OffFinder.Run(o);
                    return;
                }
                if (o.FindSize)
                {
                    SizeFinder.Run(o);
                    return;
                }
                if (o.ExtractIt)
                {
                    BinExtract.Run(o);
                    return;
                }
                if (o.NoNulls)
                {
                    AntiNuller.Run(o);
                }
            });
        }
    }
}