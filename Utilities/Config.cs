using Newtonsoft.Json;
using System.IO;

namespace ModBuilder.Utilities
{
    public static class Config
    {
        public static void Save<T>(T Data, string FilePath)
        {
            string Json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(FilePath, Json);
        }

        public static T Load<T>(string FilePath)
        {
            string Json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<T>(Json);
        }
    }
}