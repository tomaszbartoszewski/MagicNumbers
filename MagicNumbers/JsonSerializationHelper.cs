using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MagicNumbers
{
    public static class JsonSerializationHelper
    {
        public static string ToJson<T>(T data)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, data);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        public static T Deserialize<T>(string json)
        {
            Stream jsonSource = GenerateStreamFromString(json);
            var s = new DataContractJsonSerializer(typeof(T));
            return (T)s.ReadObject(jsonSource);
        }

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}
