using System.Diagnostics;
using WslToolbox.Core;

namespace WslToolbox.Gui.Commands.Service
{
    public class StartWslServiceCommand : GenericCommand
    {
        public StartWslServiceCommand()
        {
            IsExecutableDefault = o => false;
            IsExecutable = IsExecutableDefault;

            IsExecutable = o => Process.GetProcessesByName("wslhost").Length <= 0;
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = IsExecutableDefault;
            await ToolboxClass.StartWsl().ConfigureAwait(true);
        }
    }
}