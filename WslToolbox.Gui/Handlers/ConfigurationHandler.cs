using System;
using System.IO;
using System.Text.Json;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Exceptions;

namespace WslToolbox.Gui.Handlers
{
    public class ConfigurationHandler
    {
        private readonly string ConfigurationFile;
        private readonly string ConfigurationPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        private readonly string ConfigurationFileName = "settings.json";

        public event EventHandler ConfigurationUpdatedSuccessfully;
        public event EventHandler ConfigurationUpdatedFailed;

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
                ConfigurationUpdatedSuccessfully?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                throw new ConfigurationNotSaved(e.Message, e);
            }
        }

        public void Reset() => Configuration = new();
    }
}