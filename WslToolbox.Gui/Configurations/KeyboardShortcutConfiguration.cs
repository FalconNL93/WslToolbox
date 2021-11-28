using System.Windows.Input;

namespace WslToolbox.Gui.Configurations
{
    public class KeyboardShortcutConfiguration
    {
        public readonly Key AppSettingsExit = Key.Q;

        public readonly Key AppSettingsKey = Key.OemComma;
        public readonly Key GridDeleteKey = Key.Delete;
        public readonly Key GridRefreshKey = Key.F5;

        public readonly Key GridRenameKey = Key.F2;
        public bool Enabled { get; set; } = true;
        public bool AppSettingsEnabled { get; set; } = true;
        public bool AppExitEnabled { get; set; } = true;
        public bool GridRenameEnabled { get; set; } = true;
        public bool GridDeleteEnabled { get; set; } = true;
        public bool GridRefreshEnabled { get; set; } = true;
    }
}