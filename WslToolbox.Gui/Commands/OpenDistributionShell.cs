using WslToolbox.Core;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Commands
{
    public class OpenDistributionShell : GenericCommand
    {
        public OpenDistributionShell(MainViewModel mainViewModel)
        {
            IsExecutableDefault = o =>
                mainViewModel.SelectedDistribution is {State: DistributionClass.StateRunning};
            IsExecutable = IsExecutableDefault;
        }

        public override void Execute(object parameter)
        {
            ToolboxClass.ShellDistribution((DistributionClass) parameter);
        }
    }
}