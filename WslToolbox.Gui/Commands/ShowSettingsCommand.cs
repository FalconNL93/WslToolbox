using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.Commands
{
    public class ShowSettingsCommand : GenericCommand
    {
        private readonly ConfigurationHandler _config;

        public ShowSettingsCommand(ConfigurationHandler config)
        {
            _config = config;
        }

        public override void Execute(object parameter)
        {
            SettingsView settingsWindow = new(_config.Configuration, _config);
            settingsWindow.ShowDialog();

            var saveSettingsCommand = new SaveSettingsCommand(_config);
            if (settingsWindow.DialogResult != null && (bool) settingsWindow.DialogResult)
                saveSettingsCommand.Execute(null);

            settingsWindow.Close();
        }
    }
}