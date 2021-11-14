using WslToolbox.Core.Commands.Service;

namespace WslToolbox.Gui.Commands.Service
{
    public class UpdateWslServiceCommand : GenericCommand
    {
        public UpdateWslServiceCommand()
        {
            IsExecutable = _ => true;
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            await UpdateServiceCommand.Execute();
            IsExecutable = _ => true;
        }
    }
}