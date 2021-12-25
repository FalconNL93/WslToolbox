using System;
using System.Threading.Tasks;
using System.Windows;
using ModernWpf.Controls;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Handlers
{
    public class ProgressDialogEventArguments : EventArgs
    {
        public Visibility ProgressBarVisibility { get; set; }
        public bool ShowCloseButton { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Progress { get; set; }
        public object Owner { get; set; }
        public string CloseButtonText { get; set; }
        public int CloseDelay { get; set; }
    }

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

        public static event EventHandler<ProgressDialogEventArguments> UpdateProgressDialogEvent;
        public static event EventHandler<ProgressDialogEventArguments> HideProgressDialogEvent;

        public void Show(string title = null,
            string content = null,
            Visibility progressBarVisibility = Visibility.Collapsed,
            bool showCloseButton = false,
            string closeButtonText = null)
        {
            CloseButtonText = showCloseButton ? closeButtonText : null;
            DialogTitle = title;
            DialogMessage = content;
            ProgressBarVisibility = progressBarVisibility;

            if (!_dialog.IsVisible)
                _dialog.ShowAsync();
        }

        public static async void ShowDialog(string title,
            string content,
            Visibility progressBarVisibility = Visibility.Collapsed,
            bool showCloseButton = false,
            string closeButtonText = null,
            int progressValue = 0,
            object owner = null,
            int closeDelay = 0
        )
        {
            OnUpdateProgressDialogEvent(new ProgressDialogEventArguments
            {
                Title = title,
                Content = content,
                Owner = owner ?? nameof(MainViewModel),
                ShowCloseButton = showCloseButton,
                ProgressBarVisibility = progressBarVisibility,
                CloseButtonText = closeButtonText,
                Progress = progressValue
            });

            if (closeDelay <= 0) return;
            await Task.Delay(closeDelay);
            HideDialog(owner);
        }

        public static void HideDialog(object owner = null)
        {
            OnHideProgressDialogEvent(new ProgressDialogEventArguments
            {
                Owner = owner ?? nameof(MainViewModel)
            });
        }

        private static void OnUpdateProgressDialogEvent(ProgressDialogEventArguments e)
        {
            UpdateProgressDialogEvent?.Invoke(null, e);
        }

        private static void OnHideProgressDialogEvent(ProgressDialogEventArguments e)
        {
            HideProgressDialogEvent?.Invoke(null, e);
        }
    }
}