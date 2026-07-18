using CommandLine;

// ReSharper disable ClassNeverInstantiated.Global

namespace Hexer.Core
{
    public class Options
    {
        [Option('n', "noNulls", HelpText = "Write binary without nulls.")]
        public bool NoNulls { get; set; }
        
        [Option('f', "findAddr", HelpText = "Find some binary addresses.")]
        public bool FindAddr { get; set; }

        [Option('i', "input", HelpText = "Set input path.")]
        public string? Input { get; set; }
        
        [Option('o', "output", HelpText = "Set output path.")]
        public string? Output { get; set; }
    }
}