using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.Gui.Helpers.Ui;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class DeleteDistributionCommand : GenericDistributionCommand
    {
        public DeleteDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            RegisterEventHandlers();
            IsExecutableDefault = _ => true;

            IsExecutable = IsExecutable = _ =>
                distributionClass.State != DistributionClass.StateRunning;

            DefaultInfoTitle = "Deleting";
            DefaultInfoContent = $"Deleting {distributionClass.Name}...";
        }

        private void RegisterEventHandlers()
        {
            UnregisterDistributionCommand.DistributionUnregisterStarted +=
                (_, _) => { ShowInfo(); };
            UnregisterDistributionCommand.DistributionUnregisterFinished +=
                (_, _) => { HideInfo(); };
        }

        public override async void Execute(object parameter)
        {
            var deleteDistributionWarning = DialogHelper.ShowMessageBoxInfo("Delete distribution",
                "Are you sure you want to delete this distribution? All data on this distribution will be lost!",
                "Delete", closeButtonText: "Cancel",
                withConfirmationCheckbox: true,
                confirmationCheckboxText: "I confirm i want do delete this distribution.");

            var deleteDistributionWarningResult = await deleteDistributionWarning.ShowAsync();

            if (deleteDistributionWarningResult != ContentDialogResult.Primary) return;
            var distribution = (DistributionClass) parameter;
            ShowInfo();
            await UnregisterDistributionCommand.Execute(distribution);
        }
    }
}