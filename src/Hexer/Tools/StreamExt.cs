using System;
using System.IO;

namespace Hexer.Tools
{
    public static class StreamExt
    {
        public static byte[]? ReadSome(this Stream stream, int size)
        {
            var bytes = new byte[size];
            var got = stream.Read(bytes);
            if (got < 1)
                return null;
            if (bytes.Length != got)
                Array.Resize(ref bytes, got);
            return bytes;
        }
    }
}