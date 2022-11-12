using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.Models;

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
}