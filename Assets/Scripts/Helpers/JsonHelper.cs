using Newtonsoft.Json;

namespace Helpers
{
    public static class JsonHelper
    {
        public static string ToJson(object obj) => JsonConvert.SerializeObject(obj);

        public static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}