using System.Text.Json.Serialization;
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

        [JsonIgnore] public string ConfigurationFile { get; }

        public bool EnableSystemTray { get; set; }
        public bool MinimizeToTray { get; set; }
        public bool MinimizeOnStartup { get; set; }
        public bool MinimizeOnClose { get; set; }
        public bool HideDockerDistributions { get; set; } = true;
        public bool HideUnsupportedOsMessage { get; set; }
        public bool ShowMinimumOsMessage { get; set; }
        public bool EnableServicePolling { get; set; }
        public int ServicePollingInterval { get; set; } = 5000;
        public bool DisableDeleteCommand { get; set; }
        public bool DisableShortcuts { get; set; }
        public LogEventLevel MinimumLogLevel { get; set; } = LogConfiguration.MinimumLevel;
        public GridConfiguration GridConfiguration { get; set; } = new();
        public ExperimentalConfiguration ExperimentalConfiguration { get; set; } = new();
        public AppearanceConfiguration AppearanceConfiguration { get; set; } = new();
    }
}