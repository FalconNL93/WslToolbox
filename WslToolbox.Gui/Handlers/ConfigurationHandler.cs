using System.IO;
using System.Text.Json;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Handlers
{
    public class ConfigurationHandler
    {
        private readonly string ConfigurationFile;
        private readonly string ConfigurationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        private readonly string ConfigurationFileName = "settings.json";

        public ConfigurationHandler()
        {
            ConfigurationFile = $"{ConfigurationPath}/{ConfigurationFileName}";
            Configuration = new();

            if (File.Exists(ConfigurationFile))
            {
                Read();
            }
        }

        public DefaultConfiguration Configuration { get; set; }

        public void Read() => Configuration = JsonSerializer.Deserialize<DefaultConfiguration>(File.ReadAllText(ConfigurationFile));

        public void Save() => File.WriteAllText(ConfigurationFile, JsonSerializer.Serialize(Configuration));

        public void Reset() => Configuration = new();
    }
}