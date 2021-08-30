using System.IO;
using System.Text.Json;
using WslToolbox.Configurations;

namespace WslToolbox.Handlers
{
    internal class ConfigurationHandler
    {
        private readonly string FileName;

        public ConfigurationHandler()
        {
            FileName = "settings.json";
            Configuration = new();
            Read();
        }

        public DefaultConfiguration Configuration { get; set; }

        public void Read() => Configuration = JsonSerializer.Deserialize<DefaultConfiguration>(File.ReadAllText(FileName));

        public void Save() => File.WriteAllText(FileName, JsonSerializer.Serialize(Configuration));
    }
}