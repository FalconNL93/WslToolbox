using System;
using System.IO;
using System.Linq;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Collections.Dialogs;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class ImportDistributionCommand : GenericDistributionCommand
    {
        private readonly MainViewModel _viewModel;
        private string _distributionName;

        public ImportDistributionCommand(DistributionClass distributionClass, MainViewModel viewModel) : base(
            distributionClass)
        {
            RegisterEventHandlers();
            _viewModel = viewModel;
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        private void RegisterEventHandlers()
        {
            Core.Commands.Distribution.ImportDistributionCommand.DistributionImportStarted += (_, _) =>
            {
                ContentDialogHandler.ShowDialog("Importing", "Importing distribution, please wait..");
            };
            Core.Commands.Distribution.ImportDistributionCommand.DistributionImportFinished += async (_, _) =>
            {
                ContentDialogHandler.HideDialog();
                if (!_viewModel.Config.Configuration.GeneralConfiguration.ImportStartDistribution) return;

                await Core.Commands.Distribution.StartDistributionCommand.Execute(
                    _viewModel.DistributionList.First(x => x.Name == _distributionName));

                if (!_viewModel.Config.Configuration.GeneralConfiguration.ImportStartTerminal) return;
                Core.Commands.Distribution.OpenShellDistributionCommand.Execute(
                    _viewModel.DistributionList.First(x => x.Name == _distributionName));
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

            _distributionName = importDistributionDialogCollection.DistributionName;
            if (!Directory.Exists(importDistributionDialogCollection.SelectedBasePath))
                try
                {
                    Directory.CreateDirectory(importDistributionDialogCollection.SelectedBasePath);
                }
                catch (Exception e)
                {
                    LogHandler.Log().Error(e,
                        "There was an error installing the distribution in the selected directory");
                    await DialogHelper.MessageBox("Error",
                            $"There was an error installing the distribution in the selected directory.\n\n{e.Message}")
                        .ShowAsync();
                    return;
                }

            ToolboxClass.OnRefreshRequired(2000);
            Core.Commands.Distribution.ImportDistributionCommand.Execute(
                _distributionName,
                importDistributionDialogCollection.SelectedBasePath,
                importDistributionDialogCollection.SelectedFilePath);

            IsExecutable = IsExecutableDefault;
        }
    }
}