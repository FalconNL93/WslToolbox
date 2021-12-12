using System;
using System.Windows.Controls;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Core.EventArguments;
using WslToolbox.Core.Helpers;
using WslToolbox.Gui.Collections.Dialogs;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class InstallDistributionCommand : GenericCommand
    {
        public enum Parameters
        {
            ClearCache
        }

        private readonly MainViewModel _viewModel;
        private readonly ContentDialog _waitDialog;
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

        private void RegisterEventHandlers()
        {
            DistributionFetcherHelper.FetchStarted += FetchStarted;
            DistributionFetcherHelper.FetchFailed += FetchFailed;
            DistributionFetcherHelper.FetchSuccessful += FetchSuccessful;
        }

        public override async void Execute(object parameter)
        {
            if (parameter != null)
            {
                ParameterHandler((Parameters) parameter);
                return;
            }

            IsExecutable = _ => false;
            RegisterEventHandlers();

            if (_viewModel.InstallableDistributions != null)
                LogHandler.Log().Information("Using cache for online distribution list");

            _viewModel.InstallableDistributions ??=
                await DistributionClass.ListAvailableDistributions(_viewModel.DistributionList);

            if (_viewModel.InstallableDistributions != null && _errors == 0)
                SelectDialog();

            IsExecutable = _ => true;
        }

        private void ParameterHandler(Parameters parameter)
        {
            switch (parameter)
            {
                case Parameters.ClearCache:
                    _viewModel.InstallableDistributions = null;
                    LogHandler.Log().Information("Manually cleared cache for online distribution list");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter), parameter, null);
            }
        }

        private async void SelectDialog()
        {
            DistributionClass selectedDistribution = null;
            ComboBox combo = null;

            var selectDistribution = DialogHelper.ShowContentDialog(
                "Install Distribution",
                InstallDistributionDialogCollection.Items(_viewModel.InstallableDistributions),
                "Install", null, "Cancel");

            var result = await selectDistribution.Dialog.ShowAsync();
            if (result != ContentDialogResult.Primary) return;

            var stack = (StackPanel) selectDistribution.Content;

            foreach (Control item in stack.Children)
            {
                if (item.Name != "InstallDistributionList") continue;

                combo = (ComboBox) item;
                selectedDistribution = (DistributionClass) combo.SelectedItem;
            }

            Core.Commands.Distribution.OpenShellDistributionCommand.Execute(selectedDistribution);
        }

        private void FetchSuccessful(object sender, EventArgs e)
        {
            var fetchEvent = (FetchEventArguments) e;
            LogHandler.Log().Information("Fetched distribution list from {Url}", fetchEvent.Url);
            _waitDialog.Hide();
        }

        private void FetchFailed(object sender, EventArgs e)
        {
            _errors++;
            var fetchEvent = (FetchEventArguments) e;
            LogHandler.Log().Error("Error fetching distribution list from {Url}", fetchEvent.Url);
            _waitDialog.Hide();
            DialogHelper.ShowMessageBoxInfo(
                "Error", $"Could not fetch online distribution list.\n\n{fetchEvent.Message}").ShowAsync();
        }

        private void FetchStarted(object sender, EventArgs e)
        {
            var fetchEvent = (FetchEventArguments) e;
            LogHandler.Log().Information("Fetching distribution list from {Url}", fetchEvent.Url);

            _waitDialog.IsPrimaryButtonEnabled = false;
            _waitDialog.IsSecondaryButtonEnabled = false;
            _waitDialog.CloseButtonText = null;

            _waitDialog.ShowAsync();
        }
    }
}