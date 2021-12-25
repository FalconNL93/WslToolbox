using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.Gui.Handlers;
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
        }

        private void RegisterEventHandlers()
        {
            UnregisterDistributionCommand.DistributionUnregisterStarted +=
                (_, _) => { ProgressDialogHandler.ShowDialog("Deleting", $"Deleting {DistributionClass.Name}..."); };
            UnregisterDistributionCommand.DistributionUnregisterFinished +=
                (_, _) => { ProgressDialogHandler.HideDialog(); };
        }

        public override async void Execute(object parameter)
        {
            var deleteDistributionWarning = DialogHelper.MessageBox("Delete distribution",
                "Are you sure you want to delete this distribution? All data on this distribution will be lost!",
                "Delete", closeButtonText: "Cancel",
                withConfirmationCheckbox: true,
                confirmationCheckboxText: "I confirm i want do delete this distribution.");

            var deleteDistributionWarningResult = await deleteDistributionWarning.ShowAsync();

            if (deleteDistributionWarningResult != ContentDialogResult.Primary) return;
            var distribution = (DistributionClass) parameter;
            await UnregisterDistributionCommand.Execute(distribution);
        }
    }
}