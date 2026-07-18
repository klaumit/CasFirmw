using Hexer.Tools;

namespace Hexer.Core
{
    public record HexLine(string Adr, string Hex, string Txt)
    {
        public uint Off => TextExt.ParseUInt(Adr);

        public override string ToString()
        {
            return $"{Adr}: {Hex}  {Txt}";
        }
    }
}