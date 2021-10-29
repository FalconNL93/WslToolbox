using System;
using System.Windows;
using WslToolbox.Core;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class ShowImportDialogCommand : GenericDistributionCommand
    {
        public ShowImportDialogCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public override async void Execute(object parameter)
        {
            var openExportDialog = FileDialogHandler.OpenFileDialog();

            if (!(bool) openExportDialog.ShowDialog()) return;

            var fileName = openExportDialog.FileName;

            ImportView importDistributionWindow = new(fileName);
            importDistributionWindow.ShowDialog();

            if (!(bool) importDistributionWindow.DialogResult) return;

            try
            {
                IsExecutable = _ => false;
                await ToolboxClass.ImportDistribution((DistributionClass) parameter,
                    importDistributionWindow.DistributionName,
                    importDistributionWindow.DistributionSelectedDirectory, fileName).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            IsExecutable = IsExecutableDefault;
        }
    }
}