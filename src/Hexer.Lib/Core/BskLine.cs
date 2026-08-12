using Hexer.Tools;

namespace Hexer.Core
{
    public sealed class BskLine
    {
        public byte[] Raw { get; set; }
        public uint Adr { get; set; }
        
        public BskLine(uint adr, byte[] raw)
        {
            Adr = adr;
            Raw = raw;
        }

        public override string ToString()
        {
            return $"{Adr:x8}: {TextExt.ToHex(Raw)}  ";
        }
    }
}