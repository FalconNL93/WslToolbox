using System;
using WslToolbox.Core;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class StartDistributionCommand : GenericDistributionCommand
    {
        public StartDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutable = _ => distributionClass.State != DistributionClass.StateRunning;
        }

        public static event EventHandler DistributionStarted;

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            _ = await Core.Commands.Distribution.StartDistributionCommand.Execute((DistributionClass) parameter);
            IsExecutable = _ => true;

            DistributionStarted?.Invoke(this, EventArgs.Empty);
        }
    }
}