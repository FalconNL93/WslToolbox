using System.Collections.Generic;
using System.Windows.Input;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Handlers
{
    public class KeyboardShortcut
    {
        public Key Key { get; set; }
        public string Configuration { get; set; }
        public string Name { get; set; }

        public KeyboardShortcut(Key key, string configuration, string name)
        {
            Key = key;
            Configuration = configuration;
            Name = name;
        }
    }

    public class KeyboardShortcutHandler
    {
        private readonly KeyboardShortcutConfiguration _config;

        public KeyboardShortcutHandler(KeyboardShortcutConfiguration config)
        {
            _config = config;
        }

        public List<KeyboardShortcut> Shortcuts => new()
        {
            new KeyboardShortcut(_config.GridRenameKey, nameof(_config.GridRenameEnabled), "Rename"),
            new KeyboardShortcut(_config.GridDeleteKey, nameof(_config.GridDeleteEnabled), "Delete"),
            new KeyboardShortcut(_config.GridRenameKey, nameof(_config.GridRefreshEnabled), "Refresh"),
        };
    }
}