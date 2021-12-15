using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Converters;
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
            _shortcuts = settingsViewModel.KeyboardShortcutHandler.KeyboardShortcuts;
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                ElementHelper.AddToggleSwitch(nameof(DefaultConfiguration.KeyboardShortcutConfiguration.Enabled),
                    "Enable keyboard shortcuts", "Configuration.KeyboardShortcutConfiguration.Enabled", Source,
                    header: null),
                new Separator(),
                ElementHelper.ItemsControlGroup(ShortcutControls(), source: Source,
                    requires: "Configuration.KeyboardShortcutConfiguration.Enabled")
            };
        }

        private CompositeCollection ShortcutControls()
        {
            var converter = new ShortcutConverter();
            var keyboardChecks = new CompositeCollection();

            foreach (var shortcut in _shortcuts)
            {
                var shortcutKey = string.Empty;
                var shortcutKeyConverter = converter.Convert(shortcut.Key, null, null, null);

                if (shortcut.Modifier != ModifierKeys.None)
                    shortcutKey = $"{shortcut.Modifier.ToString()} + ";

                shortcutKey = $"{shortcutKey}{shortcutKeyConverter}";

                var shortcutLine = new StackPanel {Orientation = Orientation.Horizontal};
                shortcutLine.Children.Add(ElementHelper.AddToggleSwitch(shortcut.Configuration,
                    $"{shortcut.Name}",
                    $"Configuration.KeyboardShortcutConfiguration.{shortcut.Configuration}", Source,
                    header: null));
                shortcutLine.Children.Add(ElementHelper.Separator(marginLeft: 10));
                shortcutLine.Children.Add(new TextBox
                {
                    Text = shortcutKey,
                    IsReadOnly = true,
                    IsEnabled = false
                });

                keyboardChecks.Add(shortcutLine);
            }

            return keyboardChecks;
        }
    }
}