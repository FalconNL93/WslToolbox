using WslToolbox.UI.Contracts.Views;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class NotificationModal : ModalPage
{
    public NotificationModal()
    {
        ViewModel = App.GetService<NotificationViewModel>();
        InitializeComponent();
    }

    public NotificationViewModel ViewModel { get; }
}