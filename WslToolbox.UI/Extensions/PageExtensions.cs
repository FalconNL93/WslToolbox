using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Contracts.Views;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.Models;
using WslToolbox.UI.Services;
using WslToolbox.UI.Views.Modals;

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

    public static async Task<ModalResult> ShowProgressModal(this Page page, ProgressModel progressModel)
    {
        var modalPage = App.GetService<NotificationModal>();
        var contentDialog = new ContentDialog
        {
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = progressModel.Title,
            IsPrimaryButtonEnabled = progressModel.IsPrimaryButtonEnabled,
            IsSecondaryButtonEnabled = progressModel.IsSecondaryButtonEnabled,
            CloseButtonText = progressModel.ShowClose ? "Close" : string.Empty,

            XamlRoot = page.XamlRoot,
            Content = modalPage
        };

        modalPage.ViewModel.Progress = progressModel;

        return new ModalResult
        {
            ContentDialogResult = await contentDialog.ShowAsync(),
            Modal = modalPage
        };
    }

    public static async Task<ModalResult> ShowError(this Page page, string message, string title)
    {
        return await page.ShowModal<ErrorModal>(
            message,
            title,
            "Close",
            string.Empty,
            string.Empty);
    }

    public static void UpdateModal(this Page page, string message, bool showProgress = false)
    {
        var messenger = App.GetService<IMessenger>();
        messenger.Send(new ProgressIndicatorChangedMessage(new ProgressModel
        {
            Message = message,
            IsIndeterminate = false,
            ShowProgress = showProgress
        }));
    }

    public static void ShowInfoBar(this Page page, string title, string message)
    {
        var messenger = App.GetService<IMessenger>();
        messenger.Send(new InfoBarChangedMessage(new InfoBarModel
        {
            Title = title,
            Message = message
        }));
    }
}