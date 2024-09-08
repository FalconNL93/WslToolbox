using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Messengers;

public class WhatsNewDialogShowMessage : AsyncRequestMessage<ContentDialogResult>
{
    public WhatsNewDialogShowMessage(WhatsNewDialogViewModel value)
    {
        ViewModel = value;
    }

    public WhatsNewDialogViewModel ViewModel { get; set; }
}