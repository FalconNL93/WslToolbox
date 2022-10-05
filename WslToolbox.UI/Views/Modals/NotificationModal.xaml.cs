using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Contracts.Views;
using WslToolbox.UI.Services;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class NotificationModal : ModalPage
{
    public NotificationViewModel ViewModel { get; }
    public NotificationModal()
    {
        ViewModel = App.GetService<NotificationViewModel>();
        InitializeComponent();
    }
}