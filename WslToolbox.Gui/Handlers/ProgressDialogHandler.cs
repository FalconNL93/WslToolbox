using System;
using ModernWpf.Controls;
using WslToolbox.Gui.Helpers.Ui;

namespace WslToolbox.Gui.Handlers
{
    public class ProgressDialogHandler : WaitHelper, IDisposable
    {
        private readonly ContentDialog _dialog;

        public ProgressDialogHandler()
        {
            _dialog = WaitDialog();
        }

        public void Dispose()
        {
            if (_dialog.IsVisible)
                _dialog.Hide();
        }

        public void Show(
            string title = null,
            string content = null,
            bool showHideButton = false)
        {
            Dispose();
            CloseButtonText = showHideButton ? "Hide" : null;
            DialogTitle = title;
            DialogMessage = content;
            _dialog.ShowAsync();
        }
    }
}