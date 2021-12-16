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

        public ImportDistributionCommand(DistributionClass distributionClass, MainViewModel viewModel) : base(
            distributionClass)
        {
            RegisterEventHandlers();
            _viewModel = viewModel;
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
            DefaultInfoTitle = "Importing";
            DefaultInfoContent = "Importing distribution, please wait...";
        }

        private void RegisterEventHandlers()
        {
            Core.Commands.Distribution.ImportDistributionCommand.DistributionImportStarted += (_, _) =>
            {
                ShowInfo(showHideButton: true);
            };
            Core.Commands.Distribution.ImportDistributionCommand.DistributionImportFinished += (_, _) =>
            {
                HideInfo();
            };
        }

        public override async void Execute(object parameter)
        {
            var importDistributionDialogCollection = new ImportDistributionDialogCollection();
            var selectDistribution = DialogHelper.ContentDialog(
                "Import distribution",
                importDistributionDialogCollection.Items(_viewModel),
                "Import", null, "Cancel");

            selectDistribution.Dialog.SetBinding(ContentDialog.IsPrimaryButtonEnabledProperty,
                BindHelper.BindingObject("DistributionNameIsValid",
                    importDistributionDialogCollection));

            var result = await selectDistribution.Dialog.ShowAsync();

            if (result != ContentDialogResult.Primary) return;
            IsExecutable = _ => false;
            ToolboxClass.OnRefreshRequired(2000);
            Core.Commands.Distribution.ImportDistributionCommand.Execute(
                importDistributionDialogCollection.DistributionName,
                importDistributionDialogCollection.SelectedBasePath,
                importDistributionDialogCollection.SelectedFilePath);

            IsExecutable = IsExecutableDefault;
        }
    }
}