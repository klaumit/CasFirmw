namespace Hexer.Core
{
    public struct ElfSize
    {
        public readonly long pvaSize;
        public readonly long elfSize;

        public ElfSize(long pvaSize, long elfSize)
        {
            this.pvaSize = pvaSize;
            this.elfSize = elfSize;
        }
    }
}