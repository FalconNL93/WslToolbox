using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Contracts.Views;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class NotificationModal : ModalPage
{
    public string Message { get; set; }
    public NotificationModal(string message)
    {
        Message = message;
        
        InitializeComponent();
    }
}