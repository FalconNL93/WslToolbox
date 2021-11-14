using System;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class ShowExportDialogDistributionCommand : GenericDistributionCommand
    {
        public ShowExportDialogDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutableDefault = _ => distributionClass != null;
            IsExecutable = IsExecutableDefault;
        }

        public override async void Execute(object parameter)
        {
            var saveExportDialog = FileDialogHandler.SaveFileDialog();

            if (!(bool) saveExportDialog.ShowDialog()) return;

            var fileName = saveExportDialog.FileName;

            try
            {
                IsExecutable = _ => false;
                await ExportDistributionCommand
                    .Execute((DistributionClass) parameter, fileName)
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