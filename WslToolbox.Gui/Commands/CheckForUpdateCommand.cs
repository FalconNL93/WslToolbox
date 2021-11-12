using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Commands
{
    public class CheckForUpdateCommand : GenericCommand
    {
        public CheckForUpdateCommand()
        {
            IsExecutable = _ => UpdateHandler.IsAvailable();
        }

        public override void Execute(object parameter)
        {
            if (!UpdateHandler.IsAvailable()) return;

            IsExecutable = _ => false;
            UpdateHandler.Handle();
            IsExecutable = _ => true;
        }
    }
}