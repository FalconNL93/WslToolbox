using System;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers.Ui;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class ExportDistributionCommand : GenericDistributionCommand
    {
        public ExportDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutableDefault = _ => distributionClass != null;
            IsExecutable = IsExecutableDefault;
        }

        public override async void Execute(object parameter)
        {
            var warningDialog = await ShowExportWarning().ShowAsync();
            if (warningDialog != ContentDialogResult.Primary) return;

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