using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WslToolbox.Gui.Configurations.Sections
{
    public sealed class GeneralConfiguration : INotifyPropertyChanged
    {
        private bool _enableSystemTray;

        private bool _importStartDistribution;

        public bool EnableSystemTray
        {
            get => _enableSystemTray;
            set
            {
                if (value == _enableSystemTray) return;
                _enableSystemTray = value;
                OnPropertyChanged(nameof(EnableSystemTray));
            }
        }

        public bool MinimizeToTray { get; set; }
        public bool MinimizeOnStartup { get; set; }
        public bool ShowDistributionsInSystemTray { get; set; } = true;
        public bool MinimizeOnClose { get; set; }
        public bool HideDockerDistributions { get; set; } = true;

        public bool ImportCreateFolder { get; set; } = true;

        public bool ImportStartDistribution
        {
            get => _importStartDistribution;
            set
            {
                if (value == _importStartDistribution) return;
                _importStartDistribution = value;
                OnPropertyChanged(nameof(ImportStartDistribution));
            }
        }

        public bool ImportStartTerminal { get; set; }

        public bool HideUnsupportedOsMessage { get; set; }
        public bool ShowMinimumOsMessage { get; set; }
        public bool AutoCheckUpdates { get; set; } = true;
        public bool HideExportWarning { get; set; }
        public bool HideMoveWarning { get; set; }
        public bool CopyOnMove { get; set; } = true;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}