using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Contracts.Views;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class NotificationModal : ModalPage
{
    public string Message { get; set; }
    public NotificationModal(string message)
    {
        Message = message;
        WeakReferenceMessenger.Default.Register<ProgressIndicatorChangedMessage>(this, Handler);
        InitializeComponent();
    }

    private void Handler(object recipient, ProgressIndicatorChangedMessage message)
    {
        Message = message.Value.Message;
        
    }
}