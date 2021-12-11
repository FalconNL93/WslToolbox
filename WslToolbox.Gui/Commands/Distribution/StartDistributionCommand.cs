using WslToolbox.Core;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class StartDistributionCommand : GenericDistributionCommand
    {
        public StartDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            RegisterEventHandlers();
            IsExecutable = _ => distributionClass.State != DistributionClass.StateRunning;
            DefaultInfoTitle = "Starting";
            DefaultInfoContent = $"Starting {distributionClass.Name}...";
        }

        private void RegisterEventHandlers()
        {
            Core.Commands.Distribution.StartDistributionCommand.DistributionStartStarted += (_, _) => { ShowInfo(); };
            Core.Commands.Distribution.StartDistributionCommand.DistributionStartFinished += (_, _) => { HideInfo(); };
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            ShowInfo();
            _ = await Core.Commands.Distribution.StartDistributionCommand.Execute(
                (DistributionClass) parameter ?? DistributionClass);
            IsExecutable = _ => true;
        }
    }
}