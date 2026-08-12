using Hexer.Tools;

namespace Hexer.Core
{
    public sealed class HexLine
    {
        public string Adr { get; set; }
        public string Hex { get; set; }
        public string Txt { get; set; }

        public HexLine(string adr, string hex, string txt)
        {
            Adr = adr;
            Hex = hex;
            Txt = txt;
        }

        public uint Off => TextExt.ParseUInt(Adr);
        public byte[] Raw => TextExt.ParseArray(Hex);

        public override string ToString()
        {
            return $"{Adr}: {Hex}  {Txt}";
        }
    }
}