using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;
using ksc = WslToolbox.Gui.Collections.Settings.KeyboardShortcutSettingsGenericCollection;

namespace WslToolbox.Gui.Collections.Settings
{
    public class KeyboardShortcutSettingsGenericCollection : GenericCollection
    {
        private readonly List<KeyboardShortcut> _shortcuts;

        public KeyboardShortcutSettingsGenericCollection(object source) : base(source)
        {
            var settingsViewModel = (SettingsViewModel) source;
            var shortcutHandler =
                new KeyboardShortcutHandler(settingsViewModel.Configuration.KeyboardShortcutConfiguration);

            _shortcuts = new List<KeyboardShortcut>();

            _shortcuts.AddRange(shortcutHandler.AppShortcuts);
            _shortcuts.AddRange(shortcutHandler.GridShortcuts);
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.KeyboardShortcutConfiguration.Enabled),
                    "Enable keyboard shortcuts",
                    "Configuration.KeyboardShortcutConfiguration.Enabled",
                    Source),
                new Separator(),
                ElementHelper.ItemsControlGroup(ShortcutControls(),
                    source: Source,
                    requires: "Configuration.KeyboardShortcutConfiguration.Enabled")
            };
        }

        private CompositeCollection ShortcutControls()
        {
            var keyboardChecks = new CompositeCollection();

            foreach (var shortcut in _shortcuts)
            {
                var shortCutKey = string.Empty;

                if (shortcut.Modifier != ModifierKeys.None)
                    shortCutKey = $"{shortcut.Modifier.ToString()} + ";

                shortCutKey = $"{shortCutKey}{shortcut.Key}";

                keyboardChecks.Add(ElementHelper.AddCheckBox(shortcut.Configuration,
                    $"{shortcut.Name}\t\t[{shortCutKey}]",
                    $"Configuration.KeyboardShortcutConfiguration.{shortcut.Configuration}",
                    Source, enabled: shortcut.Modifiable)
                );
            }

            return keyboardChecks;
        }
    }
}