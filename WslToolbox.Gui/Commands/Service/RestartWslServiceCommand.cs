using WslToolbox.Core.Commands.Service;

namespace WslToolbox.Gui.Commands.Service
{
    public class RestartWslServiceCommand : GenericCommand
    {
        public RestartWslServiceCommand()
        {
            IsExecutable = _ => true;
        }

        public override async void Execute(object parameter)
        {
            var serviceIsRunning = await StatusServiceCommand.ServiceIsRunning();

            if (serviceIsRunning) new StopWslServiceCommand().Execute(null);
            if (!serviceIsRunning) new StartWslServiceCommand().Execute(null);
        }
    }
}