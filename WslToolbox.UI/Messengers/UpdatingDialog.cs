using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Messengers;

public class UpdatingDialogMessage : AsyncRequestMessage<ContentDialogResult>
{
    public UpdatingDialogMessage(UpdatingDialogViewModel value)
    {
        ViewModel = value;
    }

    public UpdatingDialogViewModel ViewModel { get; set; }
}