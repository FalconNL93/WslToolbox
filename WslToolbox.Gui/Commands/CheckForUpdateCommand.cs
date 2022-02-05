using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Commands
{
    public class CheckForUpdateCommand : GenericCommand
    {
        private readonly UpdateHandler _updateHandler;

        public CheckForUpdateCommand(UpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;

            IsExecutable = _ => UpdateHandler.IsAvailable();
        }

        public override void Execute(object parameter)
        {
            IsExecutable = _ => false;
            var showPrompt = (bool) parameter;

            if (!UpdateHandler.IsAvailable()) return;

            _updateHandler.CheckForUpdates(showPrompt);
            IsExecutable = _ => true;
        }
    }
}