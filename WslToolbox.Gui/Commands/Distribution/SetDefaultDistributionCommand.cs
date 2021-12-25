using WslToolbox.Core;
using WslToolbox.Gui.Handlers;

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
        }

        private void RegisterEventHandlers()
        {
            Core.Commands.Distribution.SetDefaultDistributionCommand.DistributionDefaultSetStarted +=
                (_, _) =>
                {
                    ProgressDialogHandler.ShowDialog("Changing default",
                        $"Changing default to {DistributionClass.Name}...");
                };
            Core.Commands.Distribution.SetDefaultDistributionCommand.DistributionDefaultSetFinished +=
                (_, _) => { ProgressDialogHandler.HideDialog(); };
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            _ = await Core.Commands.Distribution.SetDefaultDistributionCommand.Execute((DistributionClass) parameter);
            IsExecutable = _ => true;
        }
    }
}