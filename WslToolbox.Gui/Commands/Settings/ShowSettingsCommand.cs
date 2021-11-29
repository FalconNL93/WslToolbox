using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.Commands.Settings
{
    public class ShowSettingsCommand : GenericCommand
    {
        private readonly ConfigurationHandler _config;
        private readonly KeyboardShortcutHandler _keyboardShortcutHandler;
        private readonly OsHandler _osHandler;

        public ShowSettingsCommand(ConfigurationHandler config, OsHandler osHandler,
            KeyboardShortcutHandler keyboardShortcutHandler)
        {
            _config = config;
            _osHandler = osHandler;
            _keyboardShortcutHandler = keyboardShortcutHandler;
        }

        public override void Execute(object parameter)
        {
            SettingsView settingsWindow = new(_config.Configuration, _config, _osHandler, _keyboardShortcutHandler);
            settingsWindow.ShowDialog();

            var saveSettingsCommand = new SaveSettingsCommand(_config);
            if (settingsWindow.DialogResult != null && (bool) settingsWindow.DialogResult)
                saveSettingsCommand.Execute(null);

            settingsWindow.Close();
        }
    }
}