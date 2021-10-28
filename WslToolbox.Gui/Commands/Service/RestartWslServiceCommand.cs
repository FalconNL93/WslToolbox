using WslToolbox.Core;

namespace WslToolbox.Gui.Commands.Service
{
    public class RestartWslServiceCommand : GenericCommand
    {
        public RestartWslServiceCommand()
        {
            IsExecutable = o => true;
        }

        public override async void Execute(object parameter)
        {
            var serviceIsRunning = await ToolboxClass.ServiceIsRunning();

            if (serviceIsRunning) new StopWslServiceCommand().Execute(null);
            if (!serviceIsRunning) new StartWslServiceCommand().Execute(null);
        }
    }
}