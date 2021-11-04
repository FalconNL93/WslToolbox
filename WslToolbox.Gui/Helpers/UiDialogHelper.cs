using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls;

namespace WslToolbox.Gui.Helpers
{
    public class UiDialog
    {
        public object UserInput { get; set; }
        public ContentDialogResult DialogResult { get; set; }
    }

    public static class UiHelperDialog
    {
        public static async Task<UiDialog> ShowInputDialog(string title, string text,
            string primaryButtonText = "OK",
            string secondaryButtonText = null,
            string closeButtonText = "Close")
        {
            var dialogContent = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = text
                    },
                    new TextBox
                    {
                        Name = "dialogInputField"
                    }
                }
            };


            var dialog = new ContentDialog
            {
                Title = title,
                PrimaryButtonText = primaryButtonText,
                PrimaryButtonStyle = ResourceHelper.FindResource("AccentButtonStyle"),
                SecondaryButtonText = secondaryButtonText,
                CloseButtonText = closeButtonText,
                Content = dialogContent
            };

            var showDialog = await dialog.ShowAsync();
            var dialogContents = (StackPanel) dialog.Content;
            var userInputElement = dialogContents.Children.OfType<TextBox>().FirstOrDefault();
            var userInputValue = userInputElement?.Text;

            return new UiDialog
            {
                UserInput = userInputValue,
                DialogResult = showDialog
            };
        }

        public static async Task<UiDialog> ShowMessageBox(string title, string text,
            string primaryButtonText = null,
            string secondaryButtonText = null,
            string closeButtonText = "OK")
        {
            var dialogContent = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = text
                    }
                }
            };

            var dialog = new ContentDialog
            {
                Title = title,
                PrimaryButtonText = primaryButtonText,
                PrimaryButtonStyle = ResourceHelper.FindResource("AccentButtonStyle"),
                SecondaryButtonText = secondaryButtonText,
                CloseButtonText = closeButtonText,
                Content = dialogContent
            };


            var showDialog = await dialog.ShowAsync();

            return new UiDialog
            {
                UserInput = null,
                DialogResult = showDialog
            };
        }
    }
}