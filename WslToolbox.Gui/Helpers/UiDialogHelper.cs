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
        public ContentDialog Dialog { get; set; }
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
                Content = new ScrollViewer {Content = dialogContent}
            };

            var showDialog = await dialog.ShowAsync();
            var dialogContents = (ScrollViewer) dialog.Content;

            return new UiDialog
            {
                DialogResult = showDialog
            };
        }

        public static ContentDialog ShowMessageBoxInfo(string title, string text,
            string primaryButtonText = null,
            string secondaryButtonText = null,
            string closeButtonText = "OK",
            bool withConfirmationCheckbox = false,
            string confirmationCheckboxText = null,
            Window dialogOwner = null
        )
        {
            var dialogContent = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = text,
                        TextTrimming = TextTrimming.WordEllipsis,
                        TextWrapping = TextWrapping.Wrap
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
                Content = new ScrollViewer {Content = dialogContent},
                Owner = dialogOwner
            };

            if (withConfirmationCheckbox)
            {
                var consentCheckbox = new CheckBox
                {
                    Margin = new Thickness(0, 10, 0, 0),
                    Content = confirmationCheckboxText
                };

                dialogContent.Children.Add(consentCheckbox);
                dialog.SetBinding(ContentDialog.IsPrimaryButtonEnabledProperty,
                    BindHelper.BindingObject(nameof(consentCheckbox.IsChecked), consentCheckbox));
            }

            return dialog;
        }

        public static ContentDialog ShowMessageBoxSelectable(string title, string text, string selectableContent,
            string primaryButtonText = null,
            string secondaryButtonText = null,
            string closeButtonText = "OK",
            bool withConfirmationCheckbox = false,
            string confirmationCheckboxText = null,
            Window dialogOwner = null
        )
        {
            var dialogContent = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = text,
                        TextTrimming = TextTrimming.WordEllipsis,
                        TextWrapping = TextWrapping.Wrap
                    },
                    new TextBox
                    {
                        Text = selectableContent,
                        IsReadOnly = true,
                        MinHeight = 145
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
                Content = new ScrollViewer {Content = dialogContent},
                Owner = dialogOwner
            };

            if (withConfirmationCheckbox)
            {
                var consentCheckbox = new CheckBox
                {
                    Margin = new Thickness(0, 10, 0, 0),
                    Content = confirmationCheckboxText
                };

                dialogContent.Children.Add(consentCheckbox);
                dialog.SetBinding(ContentDialog.IsPrimaryButtonEnabledProperty,
                    BindHelper.BindingObject(nameof(consentCheckbox.IsChecked), consentCheckbox));
            }

            return dialog;
        }

        public static ContentDialog ShowContentDialog(string title, UIElementCollection items,
            string primaryButtonText = null,
            string secondaryButtonText = null,
            string closeButtonText = "OK",
            Window dialogOwner = null
        )
        {
            var dialogContent = new StackPanel();
            foreach (UIElement item in items) dialogContent.Children.Add(item);

            var dialog = new ContentDialog
            {
                Title = title,
                PrimaryButtonText = primaryButtonText,
                PrimaryButtonStyle = ResourceHelper.FindResource("AccentButtonStyle"),
                SecondaryButtonText = secondaryButtonText,
                CloseButtonText = closeButtonText,
                Content = new ScrollViewer {Content = dialogContent},
                Owner = dialogOwner
            };

            return dialog;
        }
    }
}