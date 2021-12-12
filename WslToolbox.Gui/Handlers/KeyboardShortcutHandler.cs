using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Handlers
{
    public class KeyboardShortcut
    {
        public KeyboardShortcut(Key key, ModifierKeys modifier, string name, string configuration,
            bool modifiable = true, Action action = null, bool enabled = true)
        {
            Key = key;
            Modifier = modifier;
            Name = name;
            Configuration = configuration;
            Modifiable = modifiable;
            Enabled = enabled;
            Action = action;
        }

        public Key Key { get; }
        public ModifierKeys Modifier { get; }
        public string Configuration { get; }
        public string Name { get; }
        public bool Modifiable { get; }
        public Action Action { get; }
        public bool Enabled { get; }
    }

    public class KeyboardShortcutHandler
    {
        private readonly KeyboardShortcutConfiguration _config;
        private readonly MainViewModel _model;
        public readonly List<KeyboardShortcut> KeyboardShortcuts = new();

        public KeyboardShortcutHandler(KeyboardShortcutConfiguration config, MainViewModel model)
        {
            _config = config;
            _model = model;

            KeyboardShortcuts.AddRange(AppShortcuts);
        }

        private List<KeyboardShortcut> AppShortcuts => new()
        {
            new KeyboardShortcut(_config.AppSettingsKey, ModifierKeys.Control, "Show settings",
                nameof(_config.AppSettingsEnabled), false, () => _model.ShowSettings.Execute(null),
                _config.AppSettingsEnabled),

            new KeyboardShortcut(_config.AppSettingsExit, ModifierKeys.Control, "Exit application",
                nameof(_config.AppExitEnabled), false, () => _model.ExitApplication.Execute(null),
                _config.AppExitEnabled)
        };

        public void Add(Key key, ModifierKeys modifierKey, string name, string configuration, bool modifiable,
            Action action)
        {
            KeyboardShortcuts.Add(new KeyboardShortcut(key, modifierKey, name, configuration, modifiable, action));
        }

        public KeyboardShortcut ShortcutByKey(Key key, ModifierKeys modifierKey = ModifierKeys.None)
        {
            var shortcut = KeyboardShortcuts
                .Where(x => x.Key == key)
                //.Where(x => x.Enabled)
                .FirstOrDefault(x => x.Modifier == modifierKey);

            return shortcut;
        }
    }
}