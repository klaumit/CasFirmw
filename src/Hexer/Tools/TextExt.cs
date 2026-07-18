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
    }
}