using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Messengers;

public class UpdateDialogMessage : AsyncRequestMessage<ContentDialogResult>
{
    public UpdateDialogMessage(UpdateViewModel value)
    {
        ViewModel = value;
    }

    public UpdateViewModel ViewModel { get; set; }
}