using System;
using System.IO;
using System.Text.Json;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Exceptions;

namespace WslToolbox.Gui.Handlers
{
    public class ConfigurationHandler
    {
        public ConfigurationHandler()
        {
            Configuration = new DefaultConfiguration();

            if (!File.Exists(Configuration.ConfigurationFile)) return;
            ConfigurationExists = File.Exists(Configuration.ConfigurationFile);
            Read();
        }

        public DefaultConfiguration Configuration { get; set; }
        public bool ConfigurationExists { get; }
        public event EventHandler ConfigurationUpdatedSuccessfully;

        private void Read()
        {
            Configuration =
                JsonSerializer.Deserialize<DefaultConfiguration>(File.ReadAllText(Configuration.ConfigurationFile));
        }

        public void Save()
        {
            try
            {
                File.WriteAllText(Configuration.ConfigurationFile, JsonSerializer.Serialize(Configuration));
                ConfigurationUpdatedSuccessfully?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                throw new ConfigurationFileNotSavedException(e.Message, e);
            }
        }

        public void Reset()
        {
            Configuration = new DefaultConfiguration();
        }
    }
}