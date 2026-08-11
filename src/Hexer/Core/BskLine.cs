using Hexer.Tools;

namespace Hexer.Core
{
    public record BskLine(uint Adr, byte[] Raw)
    {
        public override string ToString()
        {
            return $"{Adr:x8}: {TextExt.ToHex(Raw)}  ";
        }
    }
}