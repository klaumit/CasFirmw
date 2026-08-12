namespace Hexer.Core
{
    public sealed class AppInfo
    {
        public int Offset { get; set; }
        public int Header { get; set; }
        public int Size { get; set; }
        public string? Name { get; set; }
    }
}