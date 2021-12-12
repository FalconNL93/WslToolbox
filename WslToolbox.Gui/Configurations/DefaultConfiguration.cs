using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Serilog.Events;

namespace WslToolbox.Gui.Configurations
{
    public class DefaultConfiguration : INotifyPropertyChanged
    {
        private readonly string _defaultBasePath = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "WSL");

        private bool _enableSystemTray;

        private string _userBasePath;

        public DefaultConfiguration()
        {
            ConfigurationFile =
                $"{AppConfiguration.AppExecutableDirectory}\\{AppConfiguration.AppConfigurationFileName}";
        }

        [JsonIgnore] public string ConfigurationFile { get; }

        public bool HideActionWindows { get; set; }

        public bool EnableSystemTray
        {
            get => _enableSystemTray;
            set
            {
                if (value == _enableSystemTray) return;
                _enableSystemTray = value;
                OnPropertyChanged(nameof(_enableSystemTray));
            }
        }

        public bool MinimizeToTray { get; set; }
        public bool MinimizeOnStartup { get; set; }
        public bool ShowDistributionsInSystemTray { get; set; }
        public bool MinimizeOnClose { get; set; }
        public bool HideDockerDistributions { get; set; } = true;

        public string UserBasePath
        {
            get => _userBasePath ?? _defaultBasePath;
            set
            {
                if (value == _userBasePath) return;
                _userBasePath = value;
                OnPropertyChanged(nameof(_userBasePath));
            }
        }

        public bool HideUnsupportedOsMessage { get; set; }
        public bool ShowMinimumOsMessage { get; set; }
        public bool AutoCheckUpdates { get; set; } = true;
        public bool HideExportWarning { get; set; }
        public LogEventLevel MinimumLogLevel { get; set; } = LogConfiguration.MinimumLevel;
        public GridConfiguration GridConfiguration { get; set; } = new();
        public AppearanceConfiguration AppearanceConfiguration { get; set; } = new();
        public NotificationConfiguration NotificationConfiguration { get; set; } = new();
        public KeyboardShortcutConfiguration KeyboardShortcutConfiguration { get; set; } = new();
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}