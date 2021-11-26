using System;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Collections.Dialogs;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.Helpers.Ui;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class ImportDistributionCommand : GenericDistributionCommand
    {
        public ImportDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public static event EventHandler DistributionImported;

        public override async void Execute(object parameter)
        {
            var importDistributionDialogCollection = new ImportDistributionDialogCollection();
            var selectDistribution = DialogHelper.ShowContentDialog(
                "Import distribution",
                importDistributionDialogCollection.Items(),
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

            DistributionImported?.Invoke(this, EventArgs.Empty);
            IsExecutable = IsExecutableDefault;
        }
    }
}