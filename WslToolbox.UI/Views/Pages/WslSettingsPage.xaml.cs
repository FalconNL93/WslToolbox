using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class WslSettingsPage : Page
{
    public WslSettingsPage()
    {
        ViewModel = App.GetService<WslSettingsViewModel>();
        InitializeComponent();
    }

    public WslSettingsViewModel ViewModel { get; }
}