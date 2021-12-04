using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls;

namespace WslToolbox.Gui.Helpers.Ui
{
    public class WaitHelper
    {
        public string DialogTitle { get; set; } = null;
        public string DialogMessage { get; set; } = null;
        public string PrimaryButtonText { get; set; } = null;
        public string SecondaryButtonText { get; set; } = null;
        public string CloseButtonText { get; set; } = null;
        public Window Owner { get; set; } = null;

        public ContentDialog WaitDialog()
        {
            var dialogContent = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = DialogMessage,
                        TextTrimming = TextTrimming.WordEllipsis,
                        TextWrapping = TextWrapping.Wrap
                    }
                }
            };

            var dialog = new ContentDialog
            {
                Title = DialogTitle,
                PrimaryButtonText = PrimaryButtonText,
                PrimaryButtonStyle = ResourceHelper.FindResource("AccentButtonStyle"),
                SecondaryButtonText = SecondaryButtonText,
                CloseButtonText = CloseButtonText,
                Content = new ScrollViewer {Content = dialogContent},
                Owner = Owner
            };

            return dialog;
        }
    }
}