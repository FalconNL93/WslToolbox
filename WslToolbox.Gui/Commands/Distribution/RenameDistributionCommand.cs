using System;
using System.Linq;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.Properties;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class RenameDistributionCommand : GenericDistributionCommand
    {
        public RenameDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public static event EventHandler DistributionRenamed;

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            var showDialog = await UiHelperDialog.ShowInputDialog("Rename",
                "Enter a new name for this distribution. The distribution will be restarted after renaming.\n\n" +
                "You can only use alphanumeric characters and must contain 3 characters or more. Spaces are not allowed.",
                "Rename", closeButtonText: "Cancel");

            if (showDialog.DialogResult == ContentDialogResult.Primary)
            {
                var newName = (string) showDialog.UserInput;

                if (newName is not null)
                    try
                    {
                        if (newName.All(char.IsLetterOrDigit) && newName.Length >= 3)
                        {
                            Core.Commands.Distribution.RenameDistributionCommand.Execute((DistributionClass) parameter,
                                newName);
                            await UiHelperDialog.ShowMessageBoxInfo("Rename",
                                $"Distribution has been renamed to {newName}.").ShowAsync();
                        }
                        else
                        {
                            await UiHelperDialog.ShowMessageBoxInfo(Resources.ERROR,
                                Resources.ERROR_ONLY_ALPHANUMERIC).ShowAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        await UiHelperDialog.ShowMessageBoxInfo(Resources.ERROR, e.Message).ShowAsync();
                    }
            }

            IsExecutable = _ => true;
            DistributionRenamed?.Invoke(this, EventArgs.Empty);
        }
    }
}