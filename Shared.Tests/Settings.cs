using System.IO;
using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet.Test
{
    public class Settings
    {
        private readonly JToken Config;

        public Settings(string basePath)
        {
            string configPath = Path.Combine(basePath, "appsettings.json");
            Config = JObject.Parse(File.ReadAllText(configPath))["AccountSettings"];
        } 

        public string CloudName => Config["CloudName"].Value<string>();
        public string ApiKey => Config["ApiKey"].Value<string>();
        public string ApiSecret => Config["ApiSecret"].Value<string>();
        public string ApiBaseAddress => Config["ApiBaseAddress"].Value<string>();
    }
}