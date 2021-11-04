using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using MahApps.Metro.Controls.Dialogs;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Properties;
using WslToolbox.Gui.Views;

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
            var showDialog = await Helpers.UiHelperDialog.ShowInputDialog("Rename",
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
                            ToolboxClass.RenameDistribution((DistributionClass) parameter, newName);
                            await Helpers.UiHelperDialog.ShowMessageBox("Rename",
                                $"Distribution has been renamed to {newName}.");
                        }
                        else
                        {
                            await Helpers.UiHelperDialog.ShowMessageBox(Resources.ERROR,
                                Resources.ERROR_ONLY_ALPHANUMERIC);
                        }
                    }
                    catch (Exception e)
                    {
                        await Helpers.UiHelperDialog.ShowMessageBox(Resources.ERROR, e.Message);
                    }
            }

            IsExecutable = _ => true;
            DistributionRenamed?.Invoke(this, EventArgs.Empty);
        }
    }
}