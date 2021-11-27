using System.Windows.Input;

namespace WslToolbox.Gui.Configurations
{
    public class KeyboardShortcutConfiguration
    {
        public bool Enabled { get; set; } = true;
        public bool GridRenameEnabled { get; set; } = true;
        public bool GridDeleteEnabled { get; set; } = true;
        public bool GridRefreshEnabled { get; set; } = true;

        public readonly Key GridRenameKey = Key.F2;
        public readonly Key GridDeleteKey = Key.Delete;
        public readonly Key GridRefreshKey = Key.F5;
    }
}