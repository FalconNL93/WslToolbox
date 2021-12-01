using System;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class ExportDistributionCommand : GenericDistributionCommand
    {
        private readonly MainViewModel _model;

        public ExportDistributionCommand(DistributionClass distributionClass, MainViewModel model) : base(
            distributionClass)
        {
            _model = model;
            IsExecutableDefault = _ => distributionClass != null;
            IsExecutable = IsExecutableDefault;
        }

        public override async void Execute(object parameter)
        {
            if (!_model.Config.Configuration.HideExportWarning)
            {
                var warningDialog = await ShowExportWarning().ShowAsync();
                if (warningDialog != ContentDialogResult.Primary) return;
            }

            var saveExportDialog = FileDialogHandler.SaveFileDialog();
            if (!(bool) saveExportDialog.ShowDialog()) return;
            var fileName = saveExportDialog.FileName;

            try
            {
                IsExecutable = _ => false;
                Core.Commands.Distribution.ExportDistributionCommand.Execute((DistributionClass) parameter, fileName);
            }
            catch (Exception ex)
            {
                LogHandler.Log().Error(ex.Message, ex);
            }

            IsExecutable = IsExecutableDefault;
        }

        private static ContentDialog ShowExportWarning()
        {
            var exportDistributionWarning = DialogHelper.ShowMessageBoxInfo("Export distribution",
                "During the export the distribution will shutdown and all unsaved work will be lost.",
                "Export", closeButtonText: "Cancel");

            return exportDistributionWarning;
        }
    }
}