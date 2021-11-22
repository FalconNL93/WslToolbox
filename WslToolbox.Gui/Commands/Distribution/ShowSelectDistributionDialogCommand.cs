using System.Windows.Controls;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Collections.Dialogs;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class ShowSelectDistributionDialogCommand : GenericCommand
    {
        private readonly MainViewModel _viewModel;

        public ShowSelectDistributionDialogCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public override async void Execute(object parameter)
        {
            DistributionClass selectedDistribution = null;
            var selectDistribution = DialogHelper.ShowContentDialog(
                "Select Distribution",
                InstallDistributionDialogCollection.Items(_viewModel),
                "Install", null, "Cancel");

            var result = await selectDistribution.Dialog.ShowAsync();

            if (result != ContentDialogResult.Primary) return;

            var stack = (StackPanel) selectDistribution.Content;

            foreach (Control item in stack.Children)
            {
                if (item.Name != "InstallDistributionList") continue;

                var comboBox = (ComboBox) item;
                selectedDistribution = (DistributionClass) comboBox.SelectedItem;
            }

            Core.Commands.Distribution.OpenShellDistributionCommand.Execute(selectedDistribution);
        }
    }
}