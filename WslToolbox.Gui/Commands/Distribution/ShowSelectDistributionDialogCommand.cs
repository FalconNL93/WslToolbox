using WslToolbox.Core;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class ShowSelectDistributionDialogCommand : GenericCommand
    {
        public ShowSelectDistributionDialogCommand()
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public override void Execute(object parameter)
        {
            SelectDistributionView selectDistributionWindow = new();

            var selectedDistributionView = selectDistributionWindow.ShowDialog();

            if (!(bool) selectedDistributionView) return;

            var selectedDistribution = (DistributionClass) selectDistributionWindow.AvailableDistributions.SelectedItem;
            ToolboxClass.ShellDistribution(selectedDistribution);
        }
    }
}