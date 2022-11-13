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

        WeakReferenceMessenger.Default.Register<InfoBarChangedMessage>(this, (_, message) =>
        {
            var infoBar = message.Value;
            SettingsInfoBar.IsOpen = infoBar.IsOpen;
            SettingsInfoBar.Title = infoBar.Title;
            SettingsInfoBar.Message = infoBar.Message;
            SettingsInfoBar.Severity = infoBar.Severity;
            SettingsInfoBar.IsClosable = infoBar.IsClosable;
            SettingsInfoBar.IsIconVisible = infoBar.IsIconVisible;
        });
    }

    public SettingsViewModel ViewModel { get; }
}