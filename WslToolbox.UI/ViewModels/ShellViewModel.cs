using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml.Navigation;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Helpers;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.Views.Pages;

namespace WslToolbox.UI.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    private readonly IOptionsMonitor<UserOptions> _userOptions;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private bool _isBackEnabled;

    [ObservableProperty]
    private object? _selected;

    public ShellViewModel(
        INavigationService navigationService,
        INavigationViewService navigationViewService,
        IOptionsMonitor<UserOptions> userOptions,
        IMessenger messenger
    )
    {
        _userOptions = userOptions;
        _messenger = messenger;
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;

        ApplyUserConfiguration(userOptions.CurrentValue);
        userOptions.OnChange(OnUserConfigurationChanged);

        WeakReferenceMessenger.Default.Register<RequestUserOptions>(this, (_, message) =>
        {
            message.Reply(userOptions.CurrentValue);
        });
    }

    private void OnUserConfigurationChanged(UserOptions userOptions)
    {
        _messenger.UserOptionsChanged(userOptions);
    }

    private void ApplyUserConfiguration(UserOptions userOptions)
    {
        if (userOptions.UseSystemTray)
        {
            _messenger.ShowTrayIcon();
        }
        else
        {
            _messenger.HideTrayIcon();
        }
    }

    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}