using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using ModernWpf.Controls;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Handlers
{
    public class ContentDialogEventArguments : EventArgs
    {
        public Visibility ProgressBarVisibility { get; set; }
        public bool ShowCloseButton { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Progress { get; set; }
        public object Owner { get; set; }
        public string CloseButtonText { get; set; }
        public int CloseDelay { get; set; }
        public bool WaitForUser { get; set; }
    }

    public class ContentDialogHandler : ContentDialogHelper, IDisposable
    {
        private readonly ContentDialog _dialog;

        public ContentDialogHandler()
        {
            _dialog = ContentDialog();
        }

        public void Dispose()
        {
            if (_dialog.IsVisible)
                _dialog.Hide();
        }

        public static event EventHandler<ContentDialogEventArguments> UpdateContentDialogEvent;
        public static event EventHandler<ContentDialogEventArguments> HideContentDialogEvent;

        public async void Show(string title = null,
            string content = null,
            Visibility progressBarVisibility = Visibility.Collapsed,
            bool showCloseButton = false,
            string closeButtonText = null,
            bool waitForUser = false
        )
        {
            CloseButtonText = showCloseButton ? closeButtonText : null;
            DialogTitle = title;
            DialogMessage = content;
            ProgressBarVisibility = progressBarVisibility;

            if (_dialog.IsVisible)
                if (waitForUser)
                {
                    Debug.WriteLine("Queued dialog.");

                    while (_dialog.IsVisible)
                        await Task.Delay(10);
                }

            if (waitForUser)
                await _dialog.ShowAsync();
            else
                _dialog.ShowAsync();
        }

        public static async void ShowDialog(string title,
            string content,
            Visibility progressBarVisibility = Visibility.Collapsed,
            bool showCloseButton = false,
            string closeButtonText = null,
            int progressValue = 0,
            object owner = null,
            int closeDelay = 0,
            bool waitForUser = false
        )
        {
            OnUpdateContentDialogEvent(new ContentDialogEventArguments
            {
                Title = title,
                Content = content,
                Owner = owner ?? nameof(MainViewModel),
                ShowCloseButton = showCloseButton,
                ProgressBarVisibility = progressBarVisibility,
                CloseButtonText = closeButtonText,
                Progress = progressValue,
                WaitForUser = waitForUser
            });

            if (closeDelay <= 0) return;
            await Task.Delay(closeDelay);
            HideDialog(owner);
        }

        public static void HideDialog(object owner = null)
        {
            OnHideContentDialogEvent(new ContentDialogEventArguments
            {
                Owner = owner ?? nameof(MainViewModel)
            });
        }

        private static void OnUpdateContentDialogEvent(ContentDialogEventArguments e)
        {
            UpdateContentDialogEvent?.Invoke(null, e);
        }

        private static void OnHideContentDialogEvent(ContentDialogEventArguments e)
        {
            HideContentDialogEvent?.Invoke(null, e);
        }
    }
}