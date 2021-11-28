using System.Collections.Generic;
using System.Windows.Input;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Handlers
{
    public class KeyboardShortcut
    {
        public KeyboardShortcut(Key key, string configuration, string name, bool modifiable = true,
            ModifierKeys modifier = ModifierKeys.None)
        {
            Key = key;
            Configuration = configuration;
            Name = name;
            Modifiable = modifiable;
            Modifier = modifier;
        }

        public Key Key { get; set; }
        public string Configuration { get; set; }
        public string Name { get; set; }
        public bool Modifiable { get; set; }
        public ModifierKeys Modifier { get; set; }
    }

    public class KeyboardShortcutHandler
    {
        private readonly KeyboardShortcutConfiguration _config;

        public KeyboardShortcutHandler(KeyboardShortcutConfiguration config)
        {
            _config = config;
        }

        public List<KeyboardShortcut> AppShortcuts => new()
        {
            new KeyboardShortcut(_config.AppSettingsKey, nameof(_config.AppSettingsEnabled), "Show settings", false,
                ModifierKeys.Control),
            new KeyboardShortcut(_config.AppSettingsExit, nameof(_config.AppExitEnabled), "Exit application", false,
                ModifierKeys.Control)
        };

        public List<KeyboardShortcut> GridShortcuts => new()
        {
            new KeyboardShortcut(_config.GridRenameKey, nameof(_config.GridRenameEnabled), "Rename"),
            new KeyboardShortcut(_config.GridDeleteKey, nameof(_config.GridDeleteEnabled), "Delete"),
            new KeyboardShortcut(_config.GridRenameKey, nameof(_config.GridRefreshEnabled), "Refresh")
        };
    }
}