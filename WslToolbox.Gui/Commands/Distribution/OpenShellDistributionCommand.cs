using WslToolbox.Core;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class OpenShellDistributionCommand : GenericDistributionCommand
    {
        public OpenShellDistributionCommand(DistributionClass distributionClass) : base(distributionClass)
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutable = _ => distributionClass.State == DistributionClass.StateRunning;
        }

        public override void Execute(object parameter)
        {
            if (DistributionClass.State == DistributionClass.StateRunning)
                Core.Commands.Distribution.OpenShellDistributionCommand.Execute((DistributionClass) parameter);
        }
    }
}