using Serilog.Events;

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
        public bool HideUnsupportedOsMessage { get; set; }
        public bool ShowMinimumOsMessage { get; set; }
        public ThemeConfiguration.Styles SelectedStyle { get; set; } = ThemeConfiguration.Styles.Auto;
        public LogEventLevel MinimumLogLevel { get; set; } = LogConfiguration.MinimumLevel;
        public GridConfiguration GridConfiguration { get; set; } = new();
    }
}