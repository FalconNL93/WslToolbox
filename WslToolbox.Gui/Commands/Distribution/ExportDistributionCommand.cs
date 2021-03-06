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
            RegisterEventHandlers();
            _model = model;
            IsExecutableDefault = _ => distributionClass != null;
            IsExecutable = IsExecutableDefault;
        }

        private void RegisterEventHandlers()
        {
            Core.Commands.Distribution.ExportDistributionCommand.DistributionExportStarted +=
                (_, _) => { ContentDialogHandler.ShowDialog("Exporting", $"Exporting {DistributionClass.Name}..."); };
            Core.Commands.Distribution.ExportDistributionCommand.DistributionExportFinished +=
                (_, _) => { ContentDialogHandler.HideDialog(); };
        }


        public override async void Execute(object parameter)
        {
            if (!_model.Config.Configuration.GeneralConfiguration.HideExportWarning)
            {
                var warningDialog = await ShowExportWarning().ShowAsync();
                if (warningDialog != ContentDialogResult.Primary) return;
            }

            var exportDirectory = SelectExportDirectory();
            if (string.IsNullOrEmpty(exportDirectory)) return;

            try
            {
                IsExecutable = _ => false;

                ToolboxClass.OnRefreshRequired(2000);
                Core.Commands.Distribution.ExportDistributionCommand.Execute((DistributionClass) parameter,
                    exportDirectory);
            }
            catch (Exception ex)
            {
                LogHandler.Log().Error("{Message}", ex.Message);
            }

            IsExecutable = IsExecutableDefault;
        }

        private static string SelectExportDirectory()
        {
            var distributionPathDialog = FileDialogHandler.SaveFileDialog();

            return distributionPathDialog.ShowDialog() == null
                ? null
                : distributionPathDialog.FileName;
        }

        private static ContentDialog ShowExportWarning()
        {
            var exportDistributionWarning = DialogHelper.MessageBox("Export distribution",
                "During the export the distribution will shutdown and all unsaved work will be lost.",
                "Export", closeButtonText: "Cancel");

            return exportDistributionWarning;
        }
    }
}