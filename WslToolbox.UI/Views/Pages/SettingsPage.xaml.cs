using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<InfoBarChangedMessage>(this, OnShowInfoBar);
        WeakReferenceMessenger.Default.Register<UpdateInfoBarChangedMessage>(this, OnShowUpdateInfoBar);
    }

    public SettingsViewModel ViewModel { get; }

    private void OnShowInfoBar(object recipient, InfoBarChangedMessage message)
    {
        var infoBarModel = message.Value;
        SettingsInfoBar.IsOpen = infoBarModel.IsOpen;
        SettingsInfoBar.IsClosable = infoBarModel.IsClosable;
        SettingsInfoBar.IsIconVisible = infoBarModel.IsIconVisible;
        SettingsInfoBar.Severity = infoBarModel.Severity;
        SettingsInfoBar.Title = infoBarModel.Title;
        SettingsInfoBar.Message = infoBarModel.Message;
    }
    
    private void OnShowUpdateInfoBar(object recipient, UpdateInfoBarChangedMessage message)
    {
        var infoBarModel = message.Value;
        UpdateInfoBar.IsOpen = infoBarModel.IsOpen;
        UpdateInfoBar.IsClosable = infoBarModel.IsClosable;
        UpdateInfoBar.IsIconVisible = infoBarModel.IsIconVisible;
        UpdateInfoBar.Severity = infoBarModel.Severity;
        UpdateInfoBar.Title = infoBarModel.Title;
        UpdateInfoBar.Message = infoBarModel.Message;
    }

    private async void OnThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var theme = ThemeSelector.SelectedItem;

        if (theme is null)
        {
            return;
        }

        var selectedTheme = theme switch
        {
            "Default" => ElementTheme.Default,
            "Dark" => ElementTheme.Dark,
            "Light" => ElementTheme.Light,
            _ => ElementTheme.Default
        };

        await ViewModel.ThemeChangeCommand.ExecuteAsync(selectedTheme);
    }
}