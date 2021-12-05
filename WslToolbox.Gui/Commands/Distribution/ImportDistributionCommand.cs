using System;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Collections.Dialogs;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class ImportDistributionCommand : GenericDistributionCommand
    {
        private readonly MainViewModel _viewModel;
        private readonly ContentDialog _waitDialog;
        private readonly WaitHelper _waitHelper;

        public ImportDistributionCommand(DistributionClass distributionClass, MainViewModel viewModel) : base(
            distributionClass)
        {
            _viewModel = viewModel;
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
            RegisterEventHandlers();
            _waitHelper = new WaitHelper
            {
                ProgressRingActive = true
            };
            _waitDialog = _waitHelper.WaitDialog();
        }

        private void RegisterEventHandlers()
        {
            Core.Commands.Distribution.ImportDistributionCommand.DistributionImportStarted += ImportStarted;
            Core.Commands.Distribution.ImportDistributionCommand.DistributionImportFinished += ImportFinished;
        }

        private void ImportStarted(object? sender, EventArgs e)
        {
            _waitDialog.IsPrimaryButtonEnabled = false;
            _waitDialog.IsSecondaryButtonEnabled = false;
            _waitHelper.DialogTitle = "Importing";
            _waitHelper.DialogMessage = "Distribution is being imported, please wait...";
            _waitDialog.CloseButtonText = "Hide";

            if (!_waitDialog.IsVisible)
                _waitDialog.ShowAsync();
        }

        private void ImportFinished(object? sender, EventArgs e)
        {
            if (_waitDialog.IsVisible)
                _waitDialog.Hide();
        }

        public override async void Execute(object parameter)
        {
            var importDistributionDialogCollection = new ImportDistributionDialogCollection();
            var selectDistribution = DialogHelper.ShowContentDialog(
                "Import distribution",
                importDistributionDialogCollection.Items(_viewModel),
                "Import", null, "Cancel");

            selectDistribution.Dialog.SetBinding(ContentDialog.IsPrimaryButtonEnabledProperty,
                BindHelper.BindingObject("DistributionNameIsValid",
                    importDistributionDialogCollection));

            var result = await selectDistribution.Dialog.ShowAsync();

            if (result != ContentDialogResult.Primary) return;
            IsExecutable = _ => false;
            _waitDialog.CloseButtonText = null;
            _waitHelper.DialogTitle = "Importing";
            _waitHelper.DialogMessage = "Initialising...";
            _waitDialog.ShowAsync();
            ToolboxClass.OnRefreshRequired(2000);
            Core.Commands.Distribution.ImportDistributionCommand.Execute(
                importDistributionDialogCollection.DistributionName,
                importDistributionDialogCollection.SelectedBasePath,
                importDistributionDialogCollection.SelectedFilePath);

            IsExecutable = IsExecutableDefault;
        }
    }
}