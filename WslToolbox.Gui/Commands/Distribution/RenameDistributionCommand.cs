using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Collections.Dialogs;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.Helpers.Ui;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class RenameDistributionCommand : GenericDistributionCommand
    {
        public RenameDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public override async void Execute(object parameter)
        {
            var distribution = (DistributionClass) parameter;

            IsExecutable = _ => false;
            var renameDistributionDialogCollection = new RenameDistributionDialogCollection();
            var renameDistribution = DialogHelper.ContentDialog(
                "Rename distribution",
                renameDistributionDialogCollection.Items(distribution),
                "Rename", null, "Cancel");

            renameDistribution.Dialog.SetBinding(ContentDialog.IsPrimaryButtonEnabledProperty,
                BindHelper.BindingObject("DistributionNameIsValid",
                    renameDistributionDialogCollection));

            var result = await renameDistribution.Dialog.ShowAsync();

            if (result != ContentDialogResult.Primary) return;
            var newName = renameDistributionDialogCollection.DistributionName;

            if (ToolboxClass.DistributionByName(newName) != null)
            {
                ContentDialogHandler.ShowDialog(
                    "Error",
                    $"Renaming failed. The name {newName} already exists.",
                    showCloseButton: true,
                    closeButtonText: "Close",
                    waitForUser: true);
                return;
            }

            Core.Commands.Distribution.RenameDistributionCommand.Execute(distribution, newName);
            IsExecutable = _ => true;
        }
    }
}