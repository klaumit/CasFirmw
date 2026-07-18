using CommandLine;
using Hexer.Core;

namespace Hexer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var parser = Parser.Default;
            parser.ParseArguments<Options>(args).WithParsed(o =>
            {
                if (o.NoNulls)
                {
                    AntiNuller.Run(o);
                }
            });
        }
    }
}