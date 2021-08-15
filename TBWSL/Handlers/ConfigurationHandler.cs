using System.IO;
using System.Text.Json;
using System.Reflection;
using WslToolbox.Configurations;
namespace WslToolbox.Handlers
{
    class ConfigurationHandler
    {
        private readonly string FileName;
        public DefaultConfiguration Configuration { get; set; }

        public ConfigurationHandler()
        {
            FileName = "settings.json";
            Configuration = new();
            Read();
        }

        public void Save()
        {
            File.WriteAllText(FileName, JsonSerializer.Serialize(Configuration));
        }

        public void Read()
        {
            Configuration = JsonSerializer.Deserialize<DefaultConfiguration>(File.ReadAllText(FileName));
        }
    }
}
