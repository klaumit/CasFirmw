namespace Hexer.Core
{
    public interface INameable
    {
        string? Name { get; }
    }

    public sealed class AppInfo : INameable
    {
        public int Offset { get; set; }
        public int Header { get; set; }
        public int Size { get; set; }
        public string? Name { get; set; }
    }
}