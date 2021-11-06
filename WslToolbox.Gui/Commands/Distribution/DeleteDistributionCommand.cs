using System;
using System.Windows;
using SourceChord.FluentWPF;
using WslToolbox.Core;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class DeleteDistributionCommand : GenericDistributionCommand
    {
        private readonly object _view;

        public DeleteDistributionCommand(DistributionClass distributionClass, object view) : base(
            distributionClass)
        {
            _view = view;
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public static event EventHandler DistributionDeleted;

        public override async void Execute(object parameter)
        {
            var deleteConfirmation = AcrylicMessageBox.Show((MainView) _view,
                "Are you sure you want to remove this distribution?",
                "Warning", MessageBoxButton.YesNo);

            if (deleteConfirmation != MessageBoxResult.Yes) return;
            var distribution = (DistributionClass) parameter;

            await ToolboxClass.UnregisterDistribution(distribution);
            DistributionDeleted?.Invoke(this, EventArgs.Empty);
        }
    }
}