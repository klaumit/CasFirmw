using System;
using System.Globalization;

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
            var arr = Convert.FromHexString(txt);
            return arr;
        }
    }
}