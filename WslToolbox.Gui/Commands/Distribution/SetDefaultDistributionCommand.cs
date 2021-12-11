using WslToolbox.Core;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class SetDefaultDistributionCommand : GenericDistributionCommand
    {
        public SetDefaultDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            RegisterEventHandlers();
            IsExecutableDefault = _ => false;
            IsExecutable = _ => !distributionClass?.IsDefault ?? false;
            DefaultInfoTitle = "Default distribution";
            DefaultInfoContent = $"Changing default distribution to {distributionClass.Name}";
        }

        private void RegisterEventHandlers()
        {
            Core.Commands.Distribution.SetDefaultDistributionCommand.DistributionDefaultSetStarted +=
                (_, _) => { ShowInfo(); };
            Core.Commands.Distribution.SetDefaultDistributionCommand.DistributionDefaultSetFinished +=
                (_, _) => { HideInfo(); };
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            ShowInfo();
            _ = await Core.Commands.Distribution.SetDefaultDistributionCommand.Execute((DistributionClass) parameter);
            IsExecutable = _ => true;
        }
    }
}