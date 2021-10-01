using System.IO;
using System.Reflection;
using Serilog.Events;

namespace WslToolbox.Gui.Configurations
{
    public class DefaultConfiguration
    {
        public DefaultConfiguration()
        {
            ConfigurationFile =
                $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\{ConfigurationFileName}";
        }

        public string ConfigurationFile { get; }
        public string ConfigurationFileName { get; } = "settings.json";
        public bool EnableSystemTray { get; set; }
        public bool MinimizeToTray { get; set; }
        public bool MinimizeOnStartup { get; set; }
        public bool HideDockerDistributions { get; set; } = true;
        public bool PollServiceStatus { get; set; }
        public ThemeConfiguration.Styles SelectedStyle { get; set; } = ThemeConfiguration.Styles.Auto;
        public Logging Logging { get; set; } = new();
    }

    public class Logging
    {
        public LogEventLevel MinimumLevel { get; set; } = LogEventLevel.Error;
    }
}