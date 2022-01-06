using System;
using System.IO;
using System.Text.Json.Serialization;
using Serilog.Events;
using WslToolbox.Gui.Configurations.Sections;

namespace WslToolbox.Gui.Configurations
{
    public class DefaultConfiguration
    {
        [JsonIgnore] public readonly string UserBasePath;

        public DefaultConfiguration()
        {
            ConfigurationFile =
                $"{AppConfiguration.AppExecutableDirectory}\\{AppConfiguration.AppConfigurationFileName}";

            UserBasePath = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), "WSL");
        }

        [JsonIgnore] public string ConfigurationFile { get; }

        public GeneralConfiguration GeneralConfiguration { get; set; } = new();
        public LogEventLevel MinimumLogLevel { get; set; } = LogConfiguration.MinimumLevel;
        public GridConfiguration GridConfiguration { get; set; } = new();
        public AppearanceConfiguration AppearanceConfiguration { get; set; } = new();
        public NotificationConfiguration NotificationConfiguration { get; set; } = new();
        public KeyboardShortcutConfiguration KeyboardShortcutConfiguration { get; set; } = new();
    }
}