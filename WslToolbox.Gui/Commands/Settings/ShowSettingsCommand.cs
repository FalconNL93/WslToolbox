using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.Commands.Settings
{
    public class ShowSettingsCommand : GenericCommand
    {
        private readonly ConfigurationHandler _config;
        private readonly KeyboardShortcutHandler _keyboardShortcutHandler;

        public ShowSettingsCommand(ConfigurationHandler config, KeyboardShortcutHandler keyboardShortcutHandler)
        {
            _config = config;
            _keyboardShortcutHandler = keyboardShortcutHandler;
        }

        public override void Execute(object parameter)
        {
            SettingsView settingsWindow = new(_config.Configuration, _config, _keyboardShortcutHandler);
            settingsWindow.ShowDialog();

            var saveSettingsCommand = new SaveSettingsCommand(_config);
            if (settingsWindow.DialogResult != null && (bool) settingsWindow.DialogResult)
                saveSettingsCommand.Execute(null);

            settingsWindow.Close();
        }
    }
}