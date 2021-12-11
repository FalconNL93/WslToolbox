using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class RestartDistributionCommand : GenericDistributionCommand
    {
        public RestartDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutable = _ => distributionClass.State == DistributionClass.StateRunning;
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            _ = await TerminateDistributionCommand.Execute((DistributionClass) parameter ?? DistributionClass);
            _ = await Core.Commands.Distribution.StartDistributionCommand.Execute(
                (DistributionClass) parameter ?? DistributionClass);
            IsExecutable = _ => true;
        }
    }
}