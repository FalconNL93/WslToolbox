using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class StopDistributionCommand : GenericDistributionCommand
    {
        public StopDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            RegisterEventHandlers();
            IsExecutable = _ => distributionClass.State == DistributionClass.StateRunning;
        }

        private void RegisterEventHandlers()
        {
            TerminateDistributionCommand.DistributionTerminateStarted += (_, _) =>
            {
                ContentDialogHandler.ShowDialog("Stopping", $"Stopping {DistributionClass.Name}...");
            };
            TerminateDistributionCommand.DistributionTerminateFinished += (_, _) =>
            {
                ContentDialogHandler.HideDialog();
            };
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            _ = await TerminateDistributionCommand.Execute((DistributionClass) parameter ?? DistributionClass);
            IsExecutable = _ => true;
        }
    }
}