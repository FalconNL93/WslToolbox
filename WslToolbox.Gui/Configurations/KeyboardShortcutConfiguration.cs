using System.Windows.Input;

namespace WslToolbox.Gui.Configurations
{
    public class KeyboardShortcutConfiguration
    {
        public readonly Key AppSettingsExit = Key.Q;
        public readonly Key AppSettingsKey = Key.OemComma;
        public bool Enabled { get; set; } = true;
        public bool AppSettingsEnabled { get; set; } = true;
        public bool AppExitEnabled { get; set; } = true;
    }
}