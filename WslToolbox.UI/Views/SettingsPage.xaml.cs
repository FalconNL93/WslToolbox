using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    public SettingsViewModel ViewModel { get; }
}