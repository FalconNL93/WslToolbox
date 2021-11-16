using System;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.Gui.Helpers;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class DeleteDistributionCommand : GenericDistributionCommand
    {
        private readonly object _view;

        public DeleteDistributionCommand(DistributionClass distributionClass, object view) : base(
            distributionClass)
        {
            _view = view;
            IsExecutableDefault = _ => true;

            IsExecutable = IsExecutable = _ =>
                distributionClass.State != DistributionClass.StateRunning;
        }

        public static event EventHandler DistributionDeleted;

        public override async void Execute(object parameter)
        {
            var resetSettings = UiHelperDialog.ShowMessageBoxInfo("Delete distribution",
                "Are you sure you want to delete this distribution? All data on this distribution will be lost!",
                "Delete", closeButtonText: "Cancel",
                withConfirmationCheckbox: true,
                confirmationCheckboxText: "I confirm i want do delete this distribution.");

            var resetSettingsResult = await resetSettings.ShowAsync();

            if (resetSettingsResult != ContentDialogResult.Primary) return;
            var distribution = (DistributionClass) parameter;

            await UnregisterDistributionCommand.Execute(distribution);
            DistributionDeleted?.Invoke(this, EventArgs.Empty);
        }
    }
}