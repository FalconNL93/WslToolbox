using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Contracts.Views;
using WslToolbox.UI.Models;

namespace WslToolbox.UI.Extensions;

public static class PageExtensions
{
    public static async Task<ModalResult> ShowModal<T>(this Page page,
        object parameter,
        string title,
        string primaryButtonText,
        string secondaryButtonText = "",
        string closeButtonText = "Cancel"
    ) where T : ModalPage
    {
        var modalPage = (T) Activator.CreateInstance(typeof(T), parameter);
        var contentDialog = new ContentDialog
        {
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = title,
            PrimaryButtonText = primaryButtonText,
            SecondaryButtonText = secondaryButtonText,
            IsPrimaryButtonEnabled = true,
            IsSecondaryButtonEnabled = true,
            CloseButtonText = closeButtonText,
            DefaultButton = ContentDialogButton.Primary,
            XamlRoot = page.XamlRoot,
            Content = modalPage
        };

        return new ModalResult
        {
            ContentDialogResult = await contentDialog.ShowAsync(),
            Modal = modalPage
        };
    }
    
    public static async Task<ModalResult> ShowProgressModal<T>(this Page page, string title, string message, bool enableCancel = false) where T : ModalPage
    {
        var modalPage = (T) Activator.CreateInstance(typeof(T), message);
        var contentDialog = new ContentDialog
        {
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = title,
            IsPrimaryButtonEnabled = false,
            IsSecondaryButtonEnabled = false,
            XamlRoot = page.XamlRoot,
            Content = modalPage
        };

        if (enableCancel)
        {
            contentDialog.CloseButtonText = "Cancel";
        }

        return new ModalResult
        {
            ContentDialogResult = await contentDialog.ShowAsync(),
            Modal = modalPage
        };
    }
}