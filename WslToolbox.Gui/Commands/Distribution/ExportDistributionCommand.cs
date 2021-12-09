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
        private readonly ContentDialog _waitDialog;
        private readonly WaitHelper _waitHelper;

        public ExportDistributionCommand(DistributionClass distributionClass, MainViewModel model) : base(
            distributionClass)
        {
            RegisterEventHandlers();
            _model = model;
            IsExecutableDefault = _ => distributionClass != null;
            IsExecutable = IsExecutableDefault;
            _waitHelper = new WaitHelper
            {
                ProgressRingActive = true
            };

            _waitDialog = _waitHelper.WaitDialog();
        }

        private void RegisterEventHandlers()
        {
            Core.Commands.Distribution.ExportDistributionCommand.DistributionExportStarted += ExportStarted;
            Core.Commands.Distribution.ExportDistributionCommand.DistributionExportFinished += ExportFinished;
        }

        private void ExportStarted(object sender, EventArgs e)
        {
            _waitHelper.CloseButtonText = "Hide";
            _waitDialog.IsPrimaryButtonEnabled = false;
            _waitDialog.IsSecondaryButtonEnabled = false;
            _waitHelper.DialogTitle = "Exporting";
            _waitHelper.DialogMessage = "Exporting, please wait...";

            if (!_waitDialog.IsVisible)
                _waitDialog.ShowAsync();
        }

        private void ExportFinished(object sender, EventArgs e)
        {
            if (_waitDialog.IsVisible)
                _waitDialog.Hide();
        }


        public override async void Execute(object parameter)
        {
            if (!_model.Config.Configuration.HideExportWarning)
            {
                var warningDialog = await ShowExportWarning().ShowAsync();
                if (warningDialog != ContentDialogResult.Primary) return;
            }

            var exportDirectory = SelectExportDirectory();
            if (exportDirectory == null) return;

            try
            {
                IsExecutable = _ => false;
                _waitDialog.CloseButtonText = null;
                _waitHelper.DialogTitle = "Exporting";
                _waitHelper.DialogMessage = "Initialising...";
                _waitDialog.ShowAsync();
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
            var exportDistributionWarning = DialogHelper.ShowMessageBoxInfo("Export distribution",
                "During the export the distribution will shutdown and all unsaved work will be lost.",
                "Export", closeButtonText: "Cancel");

            return exportDistributionWarning;
        }
    }
}