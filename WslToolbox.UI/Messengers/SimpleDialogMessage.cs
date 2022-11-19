using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Models;

namespace WslToolbox.UI.Messengers;

public class SimpleDialogShowMessage : AsyncRequestMessage<ContentDialogResult>
{
    public SimpleDialogShowMessage(SimpleDialogModel value)
    {
        ViewModel = value;
    }

    public SimpleDialogModel ViewModel { get; set; }
}