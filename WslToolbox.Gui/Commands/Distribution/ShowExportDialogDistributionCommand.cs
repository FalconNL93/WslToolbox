using System;
using System.Threading.Tasks;
using WslToolbox.Core;
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
        
        public static event EventHandler DistributionExporting;
        public static event EventHandler DistributionExported;

        public override async void Execute(object parameter)
        {
            var saveExportDialog = FileDialogHandler.SaveFileDialog();

            if (!(bool) saveExportDialog.ShowDialog()) return;

            var fileName = saveExportDialog.FileName;

            try
            {
                IsExecutable = _ => false;
                await ToolboxClass.ExportDistribution((DistributionClass) parameter, fileName)
                    .ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                LogHandler.Log().Error(ex.Message, ex);
            }

            DistributionExported?.Invoke(this, EventArgs.Empty);
            IsExecutable = IsExecutableDefault;
        }
    }
}