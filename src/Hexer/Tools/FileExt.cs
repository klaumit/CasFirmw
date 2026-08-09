using System.Collections.Generic;
using System.IO;

namespace Hexer.Tools
{
    public static class FileExt
    {
        public static IDictionary<string, SortedSet<string>> FindFiles(string folder, string pattern = "*.*")
        {
            var dict = new SortedDictionary<string, SortedSet<string>>();
            const SearchOption so = SearchOption.AllDirectories;
            var files = Directory.EnumerateFiles(folder, pattern, so);
            foreach (var file in files)
            {
                var ext = Path.GetExtension(file).ToLowerInvariant();
                if (!dict.TryGetValue(ext, out var list))
                    dict[ext] = list = new SortedSet<string>();
                list.Add(file);
            }
            return dict;
        }
    }
}