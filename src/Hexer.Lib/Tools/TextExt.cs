using System.Globalization;
using System.Linq;
using Hexer.Compat;

namespace Hexer.Tools
{
    public static class TextExt
    {
        public static uint ParseUInt(string txt)
        {
            var num = uint.Parse(txt, NumberStyles.HexNumber);
            return num;
        }

        public static byte[] ParseArray(string hex)
        {
            var txt = hex.Replace(" ", "");
            var arr = NetFx.FromHexString(txt);
            return arr;
        }

        public static string ToHex(byte[] array, int? got = null)
        {
            return string.Join(" ", array.Take(got ?? array.Length)
                .ChunkX(2).Select(NetFx.ToHexStringLower));
        }
    }
}