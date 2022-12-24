using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Models;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Messengers;

public class StartupDialogShowMessage : AsyncRequestMessage<ContentDialogResult>
{
    public StartupDialogShowMessage(StartupDialogViewModel value)
    {
        ViewModel = value;
    }

    public StartupDialogViewModel ViewModel { get; set; }
}