using WslToolbox.Core;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class StartDistributionCommand : GenericDistributionCommand
    {
        public StartDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            RegisterEventHandlers();
            IsExecutable = _ => distributionClass.State != DistributionClass.StateRunning;
        }

        private void RegisterEventHandlers()
        {
            Core.Commands.Distribution.StartDistributionCommand.DistributionStartStarted += (_, _) =>
            {
                ProgressDialogHandler.ShowDialog("Starting", $"Starting {DistributionClass.Name}...");
            };
            Core.Commands.Distribution.StartDistributionCommand.DistributionStartFinished += (_, _) =>
            {
                ProgressDialogHandler.HideDialog();
            };
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            _ = await Core.Commands.Distribution.StartDistributionCommand.Execute(
                (DistributionClass) parameter ?? DistributionClass);
            IsExecutable = _ => true;
        }
    }
}