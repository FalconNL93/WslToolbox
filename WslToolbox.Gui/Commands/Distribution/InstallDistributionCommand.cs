using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ContentDialog _waitDialog;
        private List<DistributionClass> _installableDistributions;
        private int _errors;

        public InstallDistributionCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;

            _waitDialog = DialogHelper.ShowMessageBoxInfo(
                "Please wait",
                "Fetching online distribution list...");
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;

            DistributionFetcherHelper.FetchStarted += FetchStarted;
            DistributionFetcherHelper.FetchFailed += FetchFailed;
            DistributionFetcherHelper.FetchSuccessful += FetchSuccessful;

            _installableDistributions = await DistributionClass.ListAvailableDistributions();

            if (_installableDistributions != null && _errors == 0)
                SelectDialog();

            IsExecutable = _ => true;
        }

        private async void SelectDialog()
        {
            DistributionClass selectedDistribution = null;
            var selectDistribution = DialogHelper.ShowContentDialog(
                "Install Distribution",
                InstallDistributionDialogCollection.Items(_installableDistributions),
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
        }

        private async void FetchSuccessful(object? sender, EventArgs e)
        {
            _waitDialog.Hide();
        }

        private async void FetchFailed(object? sender, EventArgs e)
        {
            _errors++;
            var args = (FetchEventArguments) e;
            _waitDialog.Hide();
            var dialog = DialogHelper.ShowMessageBoxInfo(
                "Error",
                $"Could not fetch online distribution list.\n\n{args.Message}").ShowAsync();
        }

        private void FetchStarted(object? sender, EventArgs e)
        {
            _waitDialog.IsPrimaryButtonEnabled = false;
            _waitDialog.IsSecondaryButtonEnabled = false;
            _waitDialog.CloseButtonText = null;

            _waitDialog.ShowAsync();
        }
    }
}