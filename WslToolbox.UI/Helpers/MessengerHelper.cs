using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.Models;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Helpers;

public static class MessengerHelper
{
    public static void ShowInfoBar(this IMessenger messenger, string title, string message, InfoBarSeverity severity = InfoBarSeverity.Informational)
    {
        messenger.Send(new InfoBarChangedMessage(new InfoBarModel
        {
            IsOpen = true,
            IsClosable = true,
            IsIconVisible = true,
            Severity = severity,
            Title = title,
            Message = message
        }));
    }

    public static async Task<ContentDialogResult> ShowDialog(this IMessenger messenger, SimpleDialogModel viewModel)
    {
        return await messenger.Send(new SimpleDialogShowMessage(viewModel));
    }

    public static async Task<InputDialogModel> ShowInputDialog(this IMessenger messenger, InputDialogModel viewModel)
    {
        var dialogMessenger = messenger.Send(new InputDialogMessage(viewModel));

        return dialogMessenger.ViewModel;
    }

    public static async Task<ContentDialogResult> ShowUpdateDialog(this IMessenger messenger, UpdateViewModel viewModel)
    {
        return await messenger.Send(new UpdateDialogMessage(viewModel));
    }
    
    public static void ShowUpdateInfoBar(this IMessenger messenger, string title, string message, InfoBarSeverity severity = InfoBarSeverity.Informational)
    {
        messenger.Send(new UpdateInfoBarChangedMessage(new InfoBarModel
        {
            IsOpen = true,
            IsClosable = true,
            IsIconVisible = true,
            Severity = severity,
            Title = title,
            Message = message
        }));
    }
}