using System;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class StopDistributionCommand : GenericDistributionCommand
    {
        public StopDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutable = _ => distributionClass.State == DistributionClass.StateRunning;
        }

        public static event EventHandler DistributionStopped;

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            _ = await TerminateDistributionCommand.Execute((DistributionClass) parameter);
            IsExecutable = _ => true;

            DistributionStopped?.Invoke(this, EventArgs.Empty);
        }
    }
}