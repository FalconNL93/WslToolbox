using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.ViewModels;

public partial class NotificationViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ProgressModel _progress = new();

    public NotificationViewModel(IMessenger messenger)
    {
        messenger.Register<ProgressIndicatorChangedMessage>(this, (_, message) =>
        {
            Progress = message.Value;
        });
    }
}