using System.Windows.Input;

namespace WslToolbox.Gui.Configurations.Sections
{
    public sealed class KeyboardShortcutConfiguration
    {
        public readonly Key AppImportKey = Key.I;
        public readonly Key AppRefreshKey = Key.F5;
        public readonly Key AppSettingsExit = Key.Q;
        public readonly Key AppSettingsKey = Key.OemComma;
        public bool Enabled { get; set; } = true;
        public bool AppSettingsEnabled { get; set; } = true;
        public bool AppExitEnabled { get; set; } = true;
        public bool AppImportEnabled { get; set; } = true;
        public bool AppRefreshEnabled { get; set; } = true;
    }
}