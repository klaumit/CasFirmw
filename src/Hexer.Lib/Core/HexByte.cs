using System.Linq;
using Hexer.Tools;

namespace Hexer.Core
{
    public sealed class HexByte(string Adr, string Hex)
    {
        public uint Off => TextExt.ParseUInt(Adr);
        public byte Raw => TextExt.ParseArray(Hex).Single();

        public override string ToString()
        {
            return $"{Adr}: {Hex}";
        }
    }
}