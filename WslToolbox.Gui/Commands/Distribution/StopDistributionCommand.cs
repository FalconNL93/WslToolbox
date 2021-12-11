using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class StopDistributionCommand : GenericDistributionCommand
    {
        public StopDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            RegisterEventHandlers();
            IsExecutable = _ => distributionClass.State == DistributionClass.StateRunning;
            DefaultInfoTitle = "Stopping";
            DefaultInfoContent = $"Stopping {distributionClass.Name}...";
        }

        private void RegisterEventHandlers()
        {
            TerminateDistributionCommand.DistributionTerminateStarted += (_, _) => { ShowInfo(); };
            TerminateDistributionCommand.DistributionTerminateFinished += (_, _) => { HideInfo(); };
        }

        public override async void Execute(object parameter)
        {
            ShowInfo();
            IsExecutable = _ => false;
            _ = await TerminateDistributionCommand.Execute((DistributionClass) parameter ?? DistributionClass);
            IsExecutable = _ => true;
        }
    }
}