namespace Hexer.Core
{
    public sealed class Found
    {
        public int I { get; set; }
        public int P { get; set; }
        public int R { get; set; }
        public int D { get; set; }
        public int? N { get; set; }

        public override string ToString()
        {
            return $"#{I:D2}|P={P:D7}|R={R:D7}|D={D:x3}|N={N:x5}".TrimEnd('=', 'N', '|');
        }
    }
}