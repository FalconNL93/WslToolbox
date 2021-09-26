using System;
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

        public event EventHandler ConfigUpdatedSuccessfully;
        public event EventHandler ConfigUpdatedFailed;

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

        public void Save()
        {
            try
            {
                File.WriteAllText(ConfigurationFile, JsonSerializer.Serialize(Configuration));
                ConfigUpdatedSuccessfully?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                ConfigUpdatedFailed?.Invoke(this, EventArgs.Empty);
                throw new Exception(e.Message);
            }
        }

        public void Reset() => Configuration = new();
    }
}