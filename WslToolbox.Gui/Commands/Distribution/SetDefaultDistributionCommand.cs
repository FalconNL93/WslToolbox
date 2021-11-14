using System;
using WslToolbox.Core;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class SetDefaultDistributionCommand : GenericDistributionCommand
    {
        public SetDefaultDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutableDefault = _ => false;
            IsExecutable = _ => !distributionClass?.IsDefault ?? false;
        }

        public static event EventHandler DistributionDefaultChanged;

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            _ = await Core.Commands.Distribution.SetDefaultDistributionCommand.Execute((DistributionClass) parameter);
            IsExecutable = _ => true;

            DistributionDefaultChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}