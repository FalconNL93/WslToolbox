using System;
using WslToolbox.Core;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class StartDistributionCommand : GenericDistributionCommand
    {
        public StartDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public static event EventHandler DistributionStarted;

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            _ = await ToolboxClass.StartDistribution((DistributionClass) parameter);
            IsExecutable = _ => true;

            DistributionStarted?.Invoke(this, EventArgs.Empty);
        }
    }
}