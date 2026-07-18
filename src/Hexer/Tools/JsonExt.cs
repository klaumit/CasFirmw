using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Hexer.Tools
{
    public static class JsonExt
    {
        public static void Write(string file, object obj)
        {
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
            File.WriteAllText(file, json, Encoding.UTF8);
        }
    }
}