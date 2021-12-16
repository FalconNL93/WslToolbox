using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls;

namespace WslToolbox.Gui.Helpers.Ui
{
    public class UiDialog
    {
        public ContentDialog Dialog { get; set; }
        public object Content { get; set; }
    }

    public static class DialogHelper
    {
        public static ContentDialog MessageBox(string title, string text,
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
                dialog.SetBinding(ModernWpf.Controls.ContentDialog.IsPrimaryButtonEnabledProperty,
                    BindHelper.BindingObject(nameof(consentCheckbox.IsChecked), consentCheckbox));
            }

            return dialog;
        }

        public static ContentDialog UpdateDialog(string title, string text,
            string primaryButtonText = null,
            string secondaryButtonText = null,
            string closeButtonText = "OK",
            bool withConfirmationCheckbox = false,
            string confirmationCheckboxText = null,
            string changeLog = null,
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

            if (changeLog != null)
                dialogContent.Children.Add(new HyperlinkButton
                {
                    Content = "Show changelog",
                    NavigateUri = new Uri(changeLog),
                    Margin = new Thickness(0, 5, 0, 0)
                });

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

        public static UiDialog ContentDialog(string title, IEnumerable<Control> items,
            string primaryButtonText = null,
            string secondaryButtonText = null,
            string closeButtonText = "OK",
            Window dialogOwner = null
        )
        {
            var dialogContent = new StackPanel {Name = "ContentDialogStackPanel"};
            foreach (var item in items) dialogContent.Children.Add(item);

            var dialog = new ContentDialog
            {
                Title = title,
                PrimaryButtonText = primaryButtonText,
                PrimaryButtonStyle = ResourceHelper.FindResource("AccentButtonStyle"),
                SecondaryButtonText = secondaryButtonText,
                CloseButtonText = closeButtonText,
                Content = new ScrollViewer
                {
                    Name = "ScrollViewer",
                    Content = dialogContent
                },
                Owner = dialogOwner
            };

            return new UiDialog {Dialog = dialog, Content = dialogContent};
        }
    }
}