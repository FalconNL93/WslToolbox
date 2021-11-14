using System.Diagnostics;
using WslToolbox.Core.Commands.Service;

namespace WslToolbox.Gui.Commands.Service
{
    public class StopWslServiceCommand : GenericCommand
    {
        public StopWslServiceCommand()
        {
            IsExecutableDefault = _ => false;
            IsExecutable = IsExecutableDefault;

            IsExecutable = _ => Process.GetProcessesByName("wslhost").Length > 0;
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = IsExecutableDefault;
            await StopServiceCommand.Execute();
        }
    }
}