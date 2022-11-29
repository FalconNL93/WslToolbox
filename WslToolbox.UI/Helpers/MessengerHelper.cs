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

    public static async Task<ContentDialogResult> ShowDialog(this IMessenger messenger, SimpleDialogModel dialogModel)
    {
        return await messenger.Send(new SimpleDialogShowMessage(dialogModel));
    }

    public static async Task<InputDialogModel> ShowInputDialog(this IMessenger messenger, InputDialogModel dialogModel)
    {
        var dialogMessenger = messenger.Send(new InputDialogMessage(dialogModel));

        return dialogMessenger.ViewModel;
    }
    
    public static async Task<UpdateViewModel> ShowUpdateDialog(this IMessenger messenger, UpdateViewModel viewModel)
    {
        var dialogMessenger = messenger.Send(new UpdateDialogMessage(viewModel));

        return dialogMessenger.ViewModel;
    }
}