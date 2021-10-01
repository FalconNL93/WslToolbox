using System.Collections.Generic;

namespace WslToolbox.Gui.Configurations
{
    public static class FileDialogConfiguration
    {
        public const bool AddExtension = true;
        public const string DefaultExtension = "tar";
        public const int FilterIndex = 1;
        public const bool RestoreDirectory = true;
        public const bool OverwritePrompt = true;

        public static readonly Dictionary<string, string> Filter = new()
        {
            {"Tarball", "*.tar"},
            {"All files", "*.*"}
        };
    }
}