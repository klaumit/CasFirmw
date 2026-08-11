using CommandLine;

// ReSharper disable ClassNeverInstantiated.Global

namespace Hexer.Config
{
    public class Options
    {
        [Option('n', "noNulls", HelpText = "Write binary without nulls.")]
        public bool NoNulls { get; set; }
        
        [Option('f', "findAddr", HelpText = "Find some binary addresses.")]
        public bool FindAddr { get; set; }

        [Option('s', "findSize", HelpText = "Find some binary sizes.")]
        public bool FindSize { get; set; }

        [Option('e', "extract", HelpText = "Extract the binary objs.")]
        public bool ExtractIt { get; set; }

        [Option('j', "include", HelpText = "Set include path.")]
        public string? Include { get; set; }

        [Option('i', "input", HelpText = "Set input path.")]
        public string? Input { get; set; }
        
        [Option('o', "output", HelpText = "Set output path.")]
        public string? Output { get; set; }
    }
}