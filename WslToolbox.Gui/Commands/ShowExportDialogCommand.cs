using System;
using WslToolbox.Core;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Commands
{
    public class ShowExportDialogCommand : GenericCommand
    {
        private readonly MainViewModel _mainViewModel;

        public ShowExportDialogCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            IsExecutableDefault = o => _mainViewModel.SelectedDistribution != null;
            IsExecutable = IsExecutableDefault;
        }

        public override async void Execute(object parameter)
        {
            var saveExportDialog = FileDialogHandler.SaveFileDialog();

            if (!(bool) saveExportDialog.ShowDialog()) return;

            var fileName = saveExportDialog.FileName;

            try
            {
                IsExecutable = o => false;
                await ToolboxClass.ExportDistribution(_mainViewModel.SelectedDistribution, fileName)
                    .ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                LogHandler.Log().Error(ex.Message, ex);
            }

            IsExecutable = IsExecutableDefault;
        }
    }
}