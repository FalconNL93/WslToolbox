using System.Windows.Media.Effects;
using ModernWpf.Controls;
using WslToolbox.Gui.Helpers.Ui;

namespace WslToolbox.Gui.Handlers
{
    public class ProgressDialogHandler : WaitHelper
    {
        private readonly ContentDialog _dialog;

        public ProgressDialogHandler()
        {
            _dialog = WaitDialog();
        }

        public void ShowInfo(string title = null, string content = null, bool showHideButton = false)
        {
            HideInfo();
            CloseButtonText = showHideButton ? "Hide" : null;
            DialogTitle = title;
            DialogMessage = content;
            _dialog.ShowAsync();
        }

        public void HideInfo()
        {
            if (_dialog.IsVisible)
                _dialog.Hide();
        }
    }
}