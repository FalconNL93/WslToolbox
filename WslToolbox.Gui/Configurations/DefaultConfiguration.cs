using Serilog.Events;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Configurations
{
    public class DefaultConfiguration
    {
        public DefaultConfiguration()
        {
            ConfigurationFile =
                $"{AppConfiguration.AppExecutableDirectory}\\{AppConfiguration.AppConfigurationFileName}";
        }

        public string ConfigurationFile { get; }
        public bool EnableSystemTray { get; set; }
        public bool MinimizeToTray { get; set; }
        public bool MinimizeOnStartup { get; set; }
        public bool HideDockerDistributions { get; set; } = true;
        public bool PollServiceStatus { get; set; }
        public ThemeConfiguration.Styles SelectedStyle { get; set; } = ThemeConfiguration.Styles.Auto;
        public Logging Logging { get; set; } = new();
        public bool HideUnsupportedOsMessage { get; set; }
    }

    public class Logging
    {
        public LogEventLevel MinimumLevel { get; set; } = AppConfiguration.AppDefaultLogLevel();
    }
}