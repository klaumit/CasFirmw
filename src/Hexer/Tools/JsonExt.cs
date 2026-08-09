using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hexer.Tools
{
    public static class JsonExt
    {
        public static T? Read<T>(string file)
        {
            var json = File.ReadAllText(file, Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(json, GetConfig());
        }

        public static void Write(string file, object obj)
        {
            var json = JsonConvert.SerializeObject(obj, GetConfig());
            File.WriteAllText(file, json, Encoding.UTF8);
        }

        private static JsonSerializerSettings GetConfig()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                Converters = { new StringEnumConverter() }
            };
        }
    }
}