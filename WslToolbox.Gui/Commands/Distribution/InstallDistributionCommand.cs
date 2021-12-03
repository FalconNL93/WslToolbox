using System.Windows.Controls;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Core.EventArguments;
using WslToolbox.Core.Helpers;
using WslToolbox.Gui.Collections.Dialogs;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class InstallDistributionCommand : GenericCommand
    {
        private readonly MainViewModel _viewModel;

        public InstallDistributionCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;

            var waitDialog = DialogHelper.ShowMessageBoxInfo(
                "Please wait",
                "Fetching online distribution list...");
            var waitDialogAsync = waitDialog.ShowAsync();

            DistributionFetcherHelper.FetchSuccessful += (_, _) => { waitDialog.Hide(); };
            DistributionFetcherHelper.FetchStarted += (_, _) =>
            {
                waitDialog.IsPrimaryButtonEnabled = false;
                waitDialog.IsSecondaryButtonEnabled = false;
                waitDialog.CloseButtonText = null;
            };

            DistributionFetcherHelper.FetchFailed += (_, e) =>
            {
                var args = (FetchEventArguments) e;
                waitDialog.Hide();
                DialogHelper.ShowMessageBoxInfo(
                    "Error",
                    $"Could not fetch online distribution list.\n\n{args.Message}").ShowAsync();
            };

            var installable = await DistributionClass.ListAvailableDistributions();
  
            DistributionClass selectedDistribution = null;
            var selectDistribution = DialogHelper.ShowContentDialog(
                "Install Distribution",
                InstallDistributionDialogCollection.Items(installable),
                "Install", null, "Cancel");

            var result = await selectDistribution.Dialog.ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                IsExecutable = _ => false;
                return;
            }

            var stack = (StackPanel) selectDistribution.Content;

            foreach (Control item in stack.Children)
            {
                if (item.Name != "InstallDistributionList") continue;

                var comboBox = (ComboBox) item;
                selectedDistribution = (DistributionClass) comboBox.SelectedItem;
            }

            Core.Commands.Distribution.OpenShellDistributionCommand.Execute(selectedDistribution);
            IsExecutable = _ => true;
        }
    }
}