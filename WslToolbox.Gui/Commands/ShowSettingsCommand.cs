using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.Commands
{
    public class ShowSettingsCommand : GenericCommand
    {
        private readonly ConfigurationHandler _config;
        private readonly OsHandler _osHandler;

        public ShowSettingsCommand(ConfigurationHandler config, OsHandler osHandler)
        {
            _config = config;
            _osHandler = osHandler;
        }

        public override void Execute(object parameter)
        {
            SettingsView settingsWindow = new(_config.Configuration, _config, _osHandler);
            settingsWindow.ShowDialog();

            var saveSettingsCommand = new SaveSettingsCommand(_config);
            if (settingsWindow.DialogResult != null && (bool) settingsWindow.DialogResult)
                saveSettingsCommand.Execute(null);

            settingsWindow.Close();
        }
    }
}