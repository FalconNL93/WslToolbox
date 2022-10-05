using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.ViewModels;

public class NotificationViewModel : ObservableRecipient
{
    private ProgressModel _progress = new();

    public ProgressModel Progress
    {
        get => _progress;
        set => SetProperty(ref _progress, value);
    }

    public NotificationViewModel(IMessenger messenger)
    {
        messenger.Register<ProgressIndicatorChangedMessage>(this, (_, message) =>
        {
            Progress = message.Value;
        });
    }
}