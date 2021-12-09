using WslToolbox.Gui.Exceptions;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Commands.Settings
{
    public class SaveSettingsCommand : GenericCommand
    {
        private readonly ConfigurationHandler _config;

        public SaveSettingsCommand(ConfigurationHandler config)
        {
            _config = config;
        }

        public override void Execute(object parameter)
        {
            try
            {
                IsExecutable = _ => false;
                _config.Save();
            }
            catch (ConfigurationFileNotSavedException e)
            {
                LogHandler.Log().Error(e, "Settings error");
            }

            IsExecutable = _ => true;
        }
    }
}