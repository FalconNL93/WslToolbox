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

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            _ = await ToolboxClass.SetDefaultDistribution((DistributionClass) parameter);
            IsExecutable = _ => true;
        }
    }
}