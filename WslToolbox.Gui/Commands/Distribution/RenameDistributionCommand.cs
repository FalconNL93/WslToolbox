using System;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using WslToolbox.Core;
using WslToolbox.Gui.Properties;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class RenameDistributionCommand : GenericDistributionCommand
    {
        private readonly MainView _mainView;

        public RenameDistributionCommand(DistributionClass distributionClass, MainView mainView) : base(
            distributionClass)
        {
            _mainView = mainView;
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            var newName = await _mainView.ShowInputAsync(
                "Rename",
                "Enter a new name for this distribution. The distribution will be restarted after renaming.\n\n" +
                "You can only use alphanumeric characters and must contain 3 characters or more. Spaces are not allowed.");

            if (newName is not null)
                try
                {
                    if (newName.All(char.IsLetterOrDigit) && newName.Length >= 3)
                    {
                        ToolboxClass.RenameDistribution((DistributionClass) parameter, newName);
                        await _mainView.ShowMessageAsync("Rename", $"Distribution has been renamed to {newName}.");
                    }
                    else
                    {
                        await _mainView.ShowMessageAsync(Resources.ERROR, Resources.ERROR_ONLY_ALPHANUMERIC);
                    }
                }
                catch (Exception e)
                {
                    await _mainView.ShowMessageAsync(Resources.ERROR, e.Message);
                }

            IsExecutable = _ => true;
        }
    }
}