using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Commands
{
    public class CheckForUpdateCommand : GenericCommand
    {
        public CheckForUpdateCommand()
        {
            IsExecutable = _ => AppConfiguration.AppConfigurationUpdateXml != null;
        }

        public override void Execute(object parameter)
        {
            if (AppConfiguration.AppConfigurationUpdateXml == null) return;

            IsExecutable = _ => false;
            UpdateHandler.ShowDialog();
            IsExecutable = _ => true;
        }
    }
}