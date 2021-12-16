using System;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Collections.Dialogs;
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

        public static event EventHandler DistributionRenamed;

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

            IsExecutable = _ => true;
            DistributionRenamed?.Invoke(this, EventArgs.Empty);
        }
    }
}