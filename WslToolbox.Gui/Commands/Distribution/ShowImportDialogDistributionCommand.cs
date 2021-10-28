using System;
using System.Windows;
using WslToolbox.Core;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.ViewModels;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class ShowImportDialogCommand : GenericCommand
    {
        private readonly MainViewModel _mainViewModel;

        public ShowImportDialogCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            IsExecutableDefault = o => true;
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
                IsExecutable = o => false;
                await ToolboxClass.ImportDistribution(_mainViewModel.SelectedDistribution,
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