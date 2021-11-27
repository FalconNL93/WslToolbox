using System.Collections.Generic;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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

            _shortcuts = shortcutHandler.Shortcuts;
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
                keyboardChecks.Add(ElementHelper.AddCheckBox(shortcut.Configuration,
                    $"{shortcut.Name}\t\t\t\t[{shortcut.Key}] ",
                    $"Configuration.KeyboardShortcutConfiguration.{shortcut.Configuration}",
                    Source)
                );
            }

            return keyboardChecks;
        }
    }
}